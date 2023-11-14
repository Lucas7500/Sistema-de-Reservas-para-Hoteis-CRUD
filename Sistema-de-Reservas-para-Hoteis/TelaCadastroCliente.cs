﻿using Sistema_de_Reservas_para_Hoteis.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_de_Reservas_para_Hoteis
{
    public partial class TelaCadastroCliente : Form
    {
        private readonly Reserva reserva = new();
        const int idNulo = 0;
        public TelaCadastroCliente(Reserva reservaParametro)
        {
            InitializeComponent();
            CaixaSexo.DataSource = Enum.GetValues(typeof(GeneroEnum));

            if (reservaParametro.Id  > idNulo)
            {
                DataCheckIn.MinDate = reservaParametro.CheckIn;
                DataCheckOut.MinDate = reservaParametro.CheckOut;
                PreencherCadastroComDadosDaReserva(reservaParametro);
                reserva = reservaParametro;  
            }
            else
            {
                DataCheckIn.MinDate = DateTime.Now;
                DataCheckOut.MinDate = DateTime.Now;
            }
            
            
        }

        public void LerDadosDaReserva(Reserva reserva)
        {
            reserva.Nome = TextoNome.Text;
            reserva.Cpf = TextoCPF.Text;
            reserva.Telefone = TextoTelefone.Text;
            reserva.Idade = String.IsNullOrWhiteSpace(TextoIdade.Text) ? Validacoes.codigoDeErro : int.Parse(TextoIdade.Text);
            reserva.Sexo = (GeneroEnum)CaixaSexo.SelectedItem;
            reserva.CheckIn = Convert.ToDateTime(DataCheckIn.Value.Date);
            reserva.CheckOut = Convert.ToDateTime(DataCheckOut.Value.Date);
            reserva.PrecoEstadia = String.IsNullOrWhiteSpace(TextoPreco.Text) ? Validacoes.codigoDeErro : ConverterEmDecimalComVirgula(TextoPreco.Text);
            reserva.PagamentoEfetuado = !BotaoTrue.Checked && !BotaoFalse.Checked ? null : BotaoTrue.Checked;
        }

        private void PreencherCadastroComDadosDaReserva(Reserva reserva)
        {
            TextoNome.Text = reserva.Nome;
            TextoCPF.Text = reserva.Cpf;
            TextoTelefone.Text = reserva.Telefone;
            TextoIdade.Text = reserva.Idade.ToString();
            CaixaSexo.SelectedItem = reserva.Sexo;
            DataCheckIn.Value = reserva.CheckIn;
            DataCheckOut.Value = reserva.CheckOut;
            TextoPreco.Text = reserva.PrecoEstadia.ToString();
            if (reserva.PagamentoEfetuado != null)
            {
                BotaoTrue.Checked = (bool)reserva.PagamentoEfetuado;
                BotaoFalse.Checked = (bool)!reserva.PagamentoEfetuado;
            }
        }

        private static void CopiarDadosDeReservas(Reserva reserva1, Reserva reserva2)
        {
            reserva1.Nome = reserva2.Nome;
            reserva1.Cpf = reserva2.Cpf;
            reserva1.Telefone = reserva2.Telefone;
            reserva1.Idade = reserva2.Idade;
            reserva1.Sexo = reserva2.Sexo;
            reserva1.CheckIn = reserva2.CheckIn;
            reserva1.CheckOut = reserva2.CheckOut;
            reserva1.PrecoEstadia = reserva2.PrecoEstadia;
            reserva1.PagamentoEfetuado = reserva2.PagamentoEfetuado;
        }

        private static decimal ConverterEmDecimalComVirgula(string numero)
        {
            if (numero.Contains(','))
            {
                string[] preco = numero.Split(',');
                string CasasDecimais = preco[1];

                switch (CasasDecimais.Length)
                {
                    case 0:
                        numero += "00";
                        return Decimal.Parse(numero);
                    case 1:
                        numero += '0';
                        return Decimal.Parse(numero);
                    case 2:
                        return Decimal.Parse(numero);
                }
            }

            numero += ",00";
            return Decimal.Parse(numero);
        }

        private void AoClicarAdicionarCadastro(object sender, EventArgs e)
        {
            try
            {
                Reserva reservaTemporaria = new();

                if (reserva.Id > idNulo)
                {
                    CopiarDadosDeReservas(reservaTemporaria, reserva);
                }

                LerDadosDaReserva(reservaTemporaria);
                Validacoes.ValidarCampos(reservaTemporaria);
                
                CopiarDadosDeReservas(reserva, reservaTemporaria);
                TelaListaDeReservas.AdicionarReservaNaLista(reserva);
                this.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Erro no Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AoClicarCancelarCadastro(object sender, EventArgs e)
        {
            string mensagem = "Você realmente deseja cancelar?";

            var remover = MessageBox.Show(mensagem, "Confirmação de cancelamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (remover.Equals(DialogResult.Yes))
            {
                this.Close();
            } 
        }

        private void PermitirApenasNumerosNaIdade(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PermitirApenasLetrasNoNome(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsAsciiLetter(e.KeyChar))
            {
                if (e.KeyChar == ' ')
                {
                    return;
                }

                e.Handled = true;
            }
        }

        private void PermitirApenasDecimaisNoPrecoDaEstadia(object sender, KeyPressEventArgs e)
        {
            bool PossuiVirgula = TextoPreco.Text.Contains(',');

            if (e.KeyChar == ',')
            {
                e.Handled = PossuiVirgula;
                return;
            }

            if (PossuiVirgula)
            {
                int IndexCasasDecimais = 1, MaxCasasDecimais = 2;
                string[] preco = TextoPreco.Text.Split(',');
                string CasasDecimais = preco[IndexCasasDecimais];
                bool Possui2CasasDecimais = CasasDecimais.Length == MaxCasasDecimais;
                e.Handled = Possui2CasasDecimais && !char.IsControl(e.KeyChar);
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}