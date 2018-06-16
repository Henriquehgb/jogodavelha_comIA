using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjetoJogoDaVelha.Dominio;
using System.Data;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        JogoDaVelha JogoDaVelha
        {
            get { return (JogoDaVelha)Session["JogoDaVelha"]; }
            set { Session["JogoDaVelha"] = value; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (JogoDaVelha != null)
            {
                RecarregarFormulario();
            }

        }

        public void RecarregarFormulario()
        {
            CarregarGrid();
            CarregarLabels();
           
        }

        void CarregarLabels()
        {
             labelJogadorDaVez.Text = JogoDaVelha.Status != Status.Acabado ? JogoDaVelha.JogadorDaRodada + " é a sua vez." : JogoDaVelha.SituacaoFinal;

            if (JogoDaVelha.Status == Status.Acabado)
            {
                labelMensagemPrincipal.Text = JogoDaVelha.SituacaoFinal;
            }

            LabelJogador1.Text = JogoDaVelha.Jogador1.Nome + ": ";
            LabelJogador2.Text = JogoDaVelha.Jogador2.Nome + ": ";
            LabelPlacarJogador1.Text = JogoDaVelha.Placar.Jogador1.ToString(CultureInfo.InvariantCulture);
            LabelPlacarJogador2.Text = JogoDaVelha.Placar.Jogador2.ToString(CultureInfo.InvariantCulture);
            LabelPlacarVelha.Text = JogoDaVelha.Placar.Velha.ToString(CultureInfo.InvariantCulture);
        }

        private void CarregarGrid()
        {
            DataTable tabuleiro = new DataTable();
            DataRow linha = null;

            for (int i = 0; i < JogoDaVelha.Escala; i++)
            {
                tabuleiro.Columns.Add(new DataColumn(i.ToString(), typeof(string)));

            }

            for (int l = 0; l < JogoDaVelha.Escala; l++)
            {
                linha = tabuleiro.NewRow();

                for (int c = 0; c < JogoDaVelha.Escala; c++)
                {
                    int posicao = (((l)*JogoDaVelha.Escala) + (c + 1));
                    Marca marca = JogoDaVelha.Casas.Where(casa => casa != null).First(casa => casa.Posicao == posicao).Marca;
                    linha[c.ToString()] = ObterMarca(marca);
                }

                tabuleiro.Rows.Add(linha);
            }

            ViewState["CurrentTable"] = tabuleiro;

            GridViewTabuleiro.DataSource = tabuleiro;
            GridViewTabuleiro.DataBind();

        }

        protected void RadioButtonListTipoDeOponente_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PanelDadosDoPanelJogador2.Visible = RadioButtonListTipoDeOponente.SelectedValue.Equals(0.ToString());
        }

        #region Intereção com o Jogo
        private string ObterMarca(Marca marca)
        {
            if (marca == Marca.Xzinho)
                return "X";
            else if (marca == Marca.Bolinha)
                return "0";
            else
                return "";
        }

        private string OberTextoDeValidacao(ValidacaoDaJogada validacao)
        {
            string texto = "";
            switch (validacao)
            {
                case ValidacaoDaJogada.JogadaValida:
                    texto = "";
                    break;
                case ValidacaoDaJogada.PosicaoJaMarcada:
                    texto = "Esta posição no tabuleiro já foi marcada. Marque outra por favor.";
                    break;
                case ValidacaoDaJogada.JogoAcabado:
                    texto = "Fim de Jogo.";
                    break;
                default:
                    texto = "Ops! Validacao não tratada.";
                    break;
            }

            return texto;
        }

        private void JogarNaPosicao(int linha, int coluna)
        {
            ValidacaoDaJogada validacao;
            JogoDaVelha.Jogar(linha, coluna, out validacao);
            labelMensagemPrincipal.Text = OberTextoDeValidacao(validacao);
            RecarregarFormulario();
        }
        #endregion

        #region Botoes
        public void ButtonReiniciar_OnClick(object obj, EventArgs e)
        {
            JogoDaVelha.Reiniciar();
            labelMensagemPrincipal.Text = "";
            RecarregarFormulario();
        }

        protected void ButtonTerminarJogo_OnClick(object sender, EventArgs e)
        {
            labelJogadorDaVez.Text = "";
            labelMensagemPrincipal.Text = "";
            PanelEscolhaDeJogo.Visible = true;
            PanelJogo.Visible = false;
        }

        protected void IniciarJogo_OnClick(object sender, EventArgs e)
        {
            if (RadioButtonListTipoDeOponente.SelectedValue.Equals(1.ToString()))
            {
                JogoDaVelha = new JogoDaVelha(new Jogador() { Nome = TextBoxNomeDoJogador1.Text }, int.Parse(TextBoxEscala.Text));
            }
            else
            {
                JogoDaVelha = new JogoDaVelha(new Jogador() { Nome = TextBoxNomeDoJogador1.Text }, new Jogador() { Nome = TextBoxNomeDoAmigo.Text }, int.Parse(TextBoxEscala.Text));
            }

            PanelEscolhaDeJogo.Visible = false;
            PanelJogo.Visible = true;
            RecarregarFormulario();
            labelMensagemPrincipal.Text = "";
        }
        #endregion

        #region Para a Grid Funcionar
        protected void GridViewTabuleiro_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Colocando formato de click nas cells da grid
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the LinkButton control in the first cell
                LinkButton _singleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
                // Get the javascript which is assigned to this LinkButton
                string _jsSingle = ClientScript.GetPostBackClientHyperlink(
                    _singleClickButton, "");

                // Add events to each editable cell
                for (int columnIndex = 1; columnIndex <
                    e.Row.Cells.Count; columnIndex++)
                {
                    // Add the column index as the event argument parameter
                    string js = _jsSingle.Insert(_jsSingle.Length - 2,
                        columnIndex.ToString());
                    // Add this javascript to the onclick Attribute of the cell
                    e.Row.Cells[columnIndex].Attributes["onclick"] = js;
                    // Add a cursor style to the cells
                    e.Row.Cells[columnIndex].Attributes["style"] +=
                        "cursor:pointer;cursor:hand;width:60px;text-align:center;";
                }
            }
            #endregion
        }

        protected void GridViewTabuleiro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int linha = int.Parse(e.CommandArgument.ToString()) + 1;
            int coluna = int.Parse(Request.Form["__EVENTARGUMENT"]);

            JogarNaPosicao(linha, coluna);
        }
        #endregion
        
    }
}
