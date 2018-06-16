using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoJogoDaVelha.Dominio
{
    public class JogoDaVelha
    {
        enum JogadorDeInicio
        {
            Jogador1 = 1,
            Jogador2 = 0
        }

        public int Escala { get; set; }

        public JogoDaVelha(int escala)
        {
            MontarCasasDoTabuleiro(escala);
            MontarConjuntoDePosicoesQueFechamVitoria(escala);
            Escala = escala;
            Rodada = 1;
            Jogador1 = new Jogador() { MinhaMarca = Marca.Xzinho, Nome = "Jogador 1" };
            Jogador2 = new Jogador() { MinhaMarca = Marca.Bolinha, Nome = "Jogador 2" };
            FatorDeInicio = JogadorDeInicio.Jogador1;
            Placar = new Placar();
        }

        public JogoDaVelha()
            : this(3)
        {

        }

        public JogoDaVelha(Jogador jogador1, int escala)
            : this(escala)
        {
            Jogador1 = jogador1;
            Jogador2 = new JogadorIA(escala) { MinhaMarca = Marca.Bolinha, Nome = "Computador Aprendiz" };
            DefinirMarcasDosJogadores();
        }

        public JogoDaVelha(Jogador jogador1, Jogador jogador2, int escala)
            : this(escala)
        {
            Jogador1 = jogador1;
            Jogador2 = jogador2;
            DefinirMarcasDosJogadores();
        }

        public Placar Placar { get; set; }
        public int Rodada { get; set; }

        //Posicoes UniDimensionais Devido a IA
        public IList<Casa> Casas { get; set; }
        public IList<int[]> ConjuntosDePosicoesQueFechamVitoria { get; set; }

        public Jogador Jogador1 { get; set; }
        public Jogador Jogador2 { get; set; }

        JogadorDeInicio FatorDeInicio { get; set; }

        public Jogador JogadorDaRodada
        {
            get { return Rodada % 2 == (int)FatorDeInicio ? Jogador1 : Jogador2; }
        }

        public Marca MarcaDaRodada
        {
            get { return JogadorDaRodada.MinhaMarca; }
        }

        public Status Status
        {
            get
            {
                Status retorno = Status.Rodando;

                if (ObterVencedor() != Marca.Vazio || TodasAsCasasEstaoPreenchidas())
                    retorno = Status.Acabado;
                else if (TodasAsCasasEstaoVazias())
                    retorno = Status.Zerado;
                else
                    retorno = Status.Rodando;

                return retorno;

            }
        }

        private void DefinirMarcasDosJogadores()
        {
            if (Jogador1.MinhaMarca != Marca.Vazio)
            {
                Jogador2.MinhaMarca = Jogador1.MinhaMarca == Marca.Xzinho ? Marca.Bolinha : Marca.Xzinho;
            }
            else
            {
                Jogador1.MinhaMarca = Marca.Xzinho;
                Jogador2.MinhaMarca = Marca.Bolinha;
            }
        }

        private bool TodasAsCasasEstaoPreenchidas()
        {
            return Casas.Where(casa => casa != null).All(c => c.Marca != Marca.Vazio);
        }

        private bool TodasAsCasasEstaoVazias()
        {
            return Casas.Where(casa => casa != null).All(c => c.Marca == Marca.Vazio);
        }

        public string SituacaoFinal
        {
            get
            {
                if (Status == Dominio.Status.Acabado)
                {
                    if (ObterVencedor() == Marca.Vazio)
                        return " Deu Velha!";
                    else
                        return "Vencedor: " + JogadorVencedor.ToString();
                }
                else
                    return "";
            }
        }

        public bool Jogar(int linha, int coluna, out ValidacaoDaJogada validacao)
        {
            int posicaoUnidimensional = ((linha - 1) * Escala) + coluna;
            return Jogar(posicaoUnidimensional, out validacao);
        }

        public bool Jogar(int posicao, out ValidacaoDaJogada validacao)
        {
            bool retorno = false;
            validacao = ValidacaoDaJogada.JogadaValida;

            if (Status == Dominio.Status.Acabado)
            { validacao = ValidacaoDaJogada.JogoAcabado; retorno = false; }
            else if (Casas[posicao] != Marca.Vazio)
            { validacao = ValidacaoDaJogada.PosicaoJaMarcada; retorno = false; }
            else
            {
                retorno = true;
                validacao = ValidacaoDaJogada.JogadaValida;
                Casas[posicao].Marca = MarcaDaRodada;

                if (Status == Dominio.Status.Rodando)
                {
                    PassarRodada();
                    JogadorDaRodada.Jogar(this);
                }
                else
                {
                    Jogador1.ObservarFimDaJogada(this);
                    Jogador2.ObservarFimDaJogada(this);
                    AtualizarPlacar();
                }
            }

            return retorno;
        }

        private void AtualizarPlacar()
        {
            if (ObterVencedor() == Jogador1.MinhaMarca)
            {
                Placar.Jogador1++;
            }
            else if (ObterVencedor() == Jogador2.MinhaMarca)
            {
                Placar.Jogador2++;
            }
            else
            {
                Placar.Velha++;
            }
        }

        private void PassarRodada()
        {
            Rodada++;
        }

        public void ZerarCasas()
        {
            foreach (var casa in Casas)
            {
                if (casa != null)
                    casa.Marca = Marca.Vazio;
            }
        }

        public Marca ObterVencedor()
        {
            Marca retorno = Marca.Vazio;

            if (MarcaPreencheuTodosOsEspacosParaUmaVitoria(Marca.Xzinho))
                retorno = Marca.Xzinho;
            else if (MarcaPreencheuTodosOsEspacosParaUmaVitoria(Marca.Bolinha))
                retorno = Marca.Bolinha;

            return retorno;
        }

        bool MarcaPreencheuTodosOsEspacosParaUmaVitoria(Marca marca)
        {
            bool preencheuTudo = false;
            foreach (var conjuntoDePosicoesQueFechamVitoria in ConjuntosDePosicoesQueFechamVitoria)
            {
                int quantidadeDeMarcas = 0;

                for (int i = 0; i < Escala; i++)
                {
                    int posicaoQueFechaVitoria = conjuntoDePosicoesQueFechamVitoria[i];
                    if (Casas[posicaoQueFechaVitoria].Marca == marca)
                    {
                        quantidadeDeMarcas++;
                    }
                }

                preencheuTudo = quantidadeDeMarcas == Escala;
                if (preencheuTudo)
                {
                    break;
                }
            }
            return preencheuTudo;
        }

        public Jogador JogadorVencedor
        {
            get
            {
                if (ObterVencedor() == Marca.Vazio)
                {
                    return null;
                }
                return ObterVencedor() == Jogador1.MinhaMarca ? Jogador1 : Jogador2;
            }
        }

        public void Reiniciar()
        {
            if (Status == Status.Acabado)
            {
                TrocarJogadorQueInicia();
            }

            ZerarCasas();

            Jogador1.ObservarReinicioDeJogo(this);
            Jogador2.ObservarReinicioDeJogo(this);

            JogadorDaRodada.Jogar(this);
        }

        private void TrocarJogadorQueInicia()
        {
            FatorDeInicio = FatorDeInicio == JogadorDeInicio.Jogador1
                                    ? JogadorDeInicio.Jogador2
                                    : JogadorDeInicio.Jogador1;
        }


        void MontarCasasDoTabuleiro(int escala)
        {
            Casas = new List<Casa>() { null };
            for (int i = 1; i <= escala * escala; i++)
            {
                Casas.Add(new Casa(i));
            }

        }

        void MontarConjuntoDePosicoesQueFechamVitoria(int escala)
        {

            //Montagem UniDimensional Devido a IA

            ConjuntosDePosicoesQueFechamVitoria = new List<int[]>();

            for (int i = 0; i < escala; i++)
            {
                int[] posicoesQueFechamVitoriaHorizontal = new int[escala];
                int[] posicoesQueFechamVitoriaVertical = new int[escala];

                for (int j = 1; j <= escala; j++)
                {
                    posicoesQueFechamVitoriaHorizontal[j - 1] = i * escala + j;
                    posicoesQueFechamVitoriaVertical[j - 1] = j * escala - i;
                }

                ConjuntosDePosicoesQueFechamVitoria.Add(posicoesQueFechamVitoriaHorizontal);
                ConjuntosDePosicoesQueFechamVitoria.Add(posicoesQueFechamVitoriaVertical);
            }

            int[] posicoesQueFechamVitoriaDiagonaisPrincipais = new int[escala];
            for (int i = 1; i <= escala; i++)
            {
                posicoesQueFechamVitoriaDiagonaisPrincipais[i - 1] = i + (escala * (i - 1));
            }
            ConjuntosDePosicoesQueFechamVitoria.Add(posicoesQueFechamVitoriaDiagonaisPrincipais);

            posicoesQueFechamVitoriaDiagonaisPrincipais = new int[escala];
            for (int i = 1; i <= escala; i++)
            {
                posicoesQueFechamVitoriaDiagonaisPrincipais[i - 1] = (escala * i) - (i - 1);
            }
            ConjuntosDePosicoesQueFechamVitoria.Add(posicoesQueFechamVitoriaDiagonaisPrincipais);

        }

    }
}
