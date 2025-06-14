﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UaiFood.BancoDeDados.UaiFood.BancoDeDados;
using UaiFood.Controller;
using UaiFood.Model;

namespace UaiFood.View
{
    public partial class TelaPrincipalCliente : Form
    {
        int? userId = IdController.GetIdUser();
        public TelaPrincipalCliente()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TelaPesquisa telaPesquisa = new TelaPesquisa();
            telaPesquisa.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TelaPerfilCliente telaPerfilCliente = new TelaPerfilCliente();
            telaPerfilCliente.Show();
            this.Close();
        }


        private void TelaPrincipalCliente_Load(object sender, EventArgs e)
        {
            BancoDados bd = new BancoDados();
            var user = bd.findUserById(userId.Value);
            var userAdress = user.getAddress();
            var restaurantesEncontrados = bd.BuscarRestaurantesProximos(userAdress.getState());
            var pedidos = bd.listarPedidosCliente(user.getUserId());
            flowPanelRestaurantes.Controls.Clear(); // limpa resultados anteriores

            ImageController imgController = new ImageController();

            foreach (var restaurante in restaurantesEncontrados)
            {
                Panel restaurantePanel = new Panel();
                restaurantePanel.Width = 150;
                restaurantePanel.Height = 200;
                restaurantePanel.Margin = new Padding(10);
                restaurantePanel.BackColor = Color.White; // opcional: cor de fundo

                PictureBox pictureBox = new PictureBox();
                pictureBox.Width = 150;
                pictureBox.Height = 120;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                if (restaurante.getImage() != null)
                {
                    pictureBox.Image = imgController.ExibirImage(restaurante.getImage());
                }
                else
                {
                    pictureBox.Image = Properties.Resources.restaurante; // imagem padrão
                }

                pictureBox.Tag = restaurante;
                pictureBox.Cursor = Cursors.Hand;

                pictureBox.Click += (s, args) =>
                {
                    var pic = s as PictureBox;
                    var restauranteSelecionado = pic.Tag as Establishment;
                    TelaExibirRestaurante tela = new TelaExibirRestaurante(restaurante.getId());
                    tela.Show();
                };

                // Label nome
                Label nomeLabel = new Label();
                nomeLabel.Text = restaurante.getNome();
                nomeLabel.TextAlign = ContentAlignment.MiddleCenter;
                nomeLabel.Dock = DockStyle.Bottom;
                nomeLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);

                // Label cidade - estado
                Label localLabel = new Label();
                localLabel.Text = $"{restaurante.getAddressEstablishment().getCity()} - {restaurante.getAddressEstablishment().getState()}";
                localLabel.TextAlign = ContentAlignment.MiddleCenter;
                localLabel.Dock = DockStyle.Bottom;
                localLabel.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                localLabel.ForeColor = Color.Gray;

                restaurantePanel.Controls.Add(pictureBox);
                restaurantePanel.Controls.Add(localLabel);
                restaurantePanel.Controls.Add(nomeLabel);

                flowPanelRestaurantes.Controls.Add(restaurantePanel);
            }

            if (restaurantesEncontrados.Count == 0)
            {
                MessageBox.Show("Nenhum restaurante encontrado.", "Busca", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (var pedido in pedidos)
            {
                var produto = bd.ConsultarProdutoPorId(pedido.getIdProduto());

                Panel pedidoPanel = new Panel();
                pedidoPanel.Width = 150;
                pedidoPanel.Height = 185;
                pedidoPanel.Margin = new Padding(10);
                pedidoPanel.BackColor = Color.White;

                PictureBox pictureBox = new PictureBox();
                pictureBox.Width = 150;
                pictureBox.Height = 130; // Reduzido para abrir espaço para o nome logo abaixo
                pictureBox.Location = new Point(0, 0);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                if (produto.getImagem() != null)
                {
                    pictureBox.Image = imgController.ExibirImage(produto.getImagem());
                }
                else
                {
                    pictureBox.Image = Properties.Resources.restaurante;
                }

                pictureBox.Tag = produto;
                pictureBox.Cursor = Cursors.Hand;
                pictureBox.Click += (s, args) =>
                {
                    var pic = s as PictureBox;
                    var restauranteSelecionado = pic.Tag as Establishment;
                    TelaExibirProduto tela = new TelaExibirProduto(produto.getId());
                    tela.Show();
                };

                Label nomeLabel = new Label();
                nomeLabel.Text = produto.getNome();
                nomeLabel.TextAlign = ContentAlignment.MiddleCenter;
                nomeLabel.Location = new Point(0, 135); // logo abaixo da imagem
                nomeLabel.Size = new Size(150, 40); // largura igual ao panel
                nomeLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                nomeLabel.AutoEllipsis = true;

                pedidoPanel.Controls.Add(pictureBox);
                pedidoPanel.Controls.Add(nomeLabel);

                flowPanelPedidos.Controls.Add(pedidoPanel);
            }

            if (restaurantesEncontrados.Count == 0)
            {
                MessageBox.Show("Nenhum restaurante encontrado.", "Busca", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TelaCarrinho telaCarrinho = new TelaCarrinho();
            telaCarrinho.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TelaPedidosCliente telaPedidosCliente = new TelaPedidosCliente();
            telaPedidosCliente.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TelaQrCode telaQrCode = new TelaQrCode();
            telaQrCode.Show();
        }
    }
}

