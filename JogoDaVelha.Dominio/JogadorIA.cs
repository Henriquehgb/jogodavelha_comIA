using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjetoJogoDaVelha.Dominio.IA;

namespace ProjetoJogoDaVelha.Dominio
{
    public class JogadorIA : Jogador
    {
        ArvoreDeAprendizado ArvoreDeAprendizado { get; set; }

        IList<Casa> CasasDaUltimaJogada { get; set; }

        NoDeMemoria NoDaUltimaJogada { get; set; }


        public JogadorIA(int escala)
        {
            ArvoreDeAprendizado = new ArvoreDeAprendizado(escala);
            NoDaUltimaJogada = ArvoreDeAprendizado.NoRaiz;
            ZerarMemoriaDeTrabalho(escala);
        }

        private void ZerarMemoriaDeTrabalho(int escala)
        {
            MontarCasasDaUltimaJogadaZerada(escala);
            NoDaUltimaJogada = ArvoreDeAprendizado.NoRaiz;
        }

        private void ZerarMemoriaPermanente(int escala)
        {
            ArvoreDeAprendizado = new ArvoreDeAprendizado(escala);
        }

        #region Métodos Principais

        public override void Jogar(JogoDaVelha jogoDaVelha)
        {
            int posicaoDoAdversario = 0;
            if (!EhPrimeiraRodada(jogoDaVelha))
            {
                posicaoDoAdversario = ObterPosicaoDaJogadaAdversaria(jogoDaVelha.Casas);
                NoDaUltimaJogada = NoDaUltimaJogada.CarregarNoFilhoDaPosicao((Int32)posicaoDoAdversario);
            }

            NoDaUltimaJogada = EscolherNoDaProximaJogada(jogoDaVelha);

            ValidacaoDaJogada validacao;

            AtualizarCasasDaUltimaJogadaPelasCasasAtuais(CasasDaUltimaJogada, jogoDaVelha.Casas, posicaoDoAdversario);

            jogoDaVelha.Jogar(NoDaUltimaJogada.Posicao, out validacao);
        }

        private NoDeMemoria EscolherNoDaProximaJogada(JogoDaVelha jogo)
        {
            NoDeMemoria retorno = null;

            int posicao = ObterPosicaoDeJogadaDeAtaqueObvio(jogo);

            if (posicao == 0)
                posicao = ObterPosicaoDeJogadaDeDefesaObvia(jogo);

            if (posicao == 0)
                retorno = ObterNoFilhoPorMelhorEscolhaMemorizada(NoDaUltimaJogada);

            if (posicao == 0 && retorno == null)
                posicao = ObterPrimeiraPosicaoDeJogadaPossivel(jogo);

            retorno = retorno ?? NoDaUltimaJogada.CarregarNoFilhoDaPosicao(posicao);

            return retorno;
        }

        private int ObterPrimeiraPosicaoDeJogadaPossivel(JogoDaVelha jogo)
        {
            Casa casaPossivel = jogo.Casas.Where(casa => casa != null)
                                .FirstOrDefault(casa => casa.Marca == Marca.Vazio 
                                                && !NoDaUltimaJogada.NosFilhos.Any(no=> no.PesoDeMelhorEscolha<5 && no.Posicao == casa.Posicao) );
            int posicao = 0;
            if (casaPossivel != null)
            {
                posicao = casaPossivel.Posicao;
                
            }
            else
            {
                posicao = jogo.Casas.Where(casa => casa != null).First(casa => casa.Marca == Marca.Vazio).Posicao;
            }

            return posicao;
        }

        public override void ObservarFimDaJogada(JogoDaVelha jogoDaVelha)
        {
            RepesarMemoriaDeLongoPrazo(jogoDaVelha);

            ZerarMemoriaDeTrabalho(jogoDaVelha.Escala);
        }

        public override void ObservarReinicioDeJogo(JogoDaVelha jogo)
        {
            ZerarMemoriaDeTrabalho(jogo.Escala);
        }

        private void RepesarMemoriaDeLongoPrazo(JogoDaVelha jogoDaVelha)
        {
            if (jogoDaVelha.ObterVencedor() != Marca.Vazio)
            {
                NoDeMemoria no = NoDaUltimaJogada.NoPai;

                while (no.NoPai != null)
                {
                    Int32 pesoDeMelhorEscolha = jogoDaVelha.Casas[no.Posicao] == jogoDaVelha.ObterVencedor() ? ((Int32)(1)) : ((Int32)(-1));

                    if ((pesoDeMelhorEscolha > 0 && no.PesoDeMelhorEscolha < 10) || (pesoDeMelhorEscolha < 0 && no.PesoDeMelhorEscolha > 0))
                        no.PesoDeMelhorEscolha += pesoDeMelhorEscolha;

                    no = no.NoPai;
                }
            }
        }

        #endregion

        #region Obtendo Memoria De Qual Melhor Jogada
        private int ObterPosicaoDeJogadaDeDefesaObvia(JogoDaVelha jogo)
        {
            var marcaAdversaria = MinhaMarca == Dominio.Marca.Bolinha ? Dominio.Marca.Xzinho : Dominio.Marca.Bolinha;

            int posicaoAJogar = 0;

            posicaoAJogar = ObterPosicaoParaPreencherMarcaEmTodasAsLinhasOuColunas(jogo, marcaAdversaria);

            return posicaoAJogar;
        }

        private int ObterPosicaoDeJogadaDeAtaqueObvio(JogoDaVelha jogo)
        {
            int posicaoAJogar = 0;

            posicaoAJogar = ObterPosicaoParaPreencherMarcaEmTodasAsLinhasOuColunas(jogo, MinhaMarca);

            return posicaoAJogar;
        }

        private static int ObterPosicaoParaPreencherMarcaEmTodasAsLinhasOuColunas(JogoDaVelha jogo, Dominio.Marca marcaParaPreencher)
        {
            int posicaoAJogar = 0;

            foreach (var conjuntoDePosicoesQueFechamVitoria in jogo.ConjuntosDePosicoesQueFechamVitoria)
            {
                int quantidadeDeMarcas = 0;
                int quantidadeDeVazio = 0;
                for (int i = 0; i < jogo.Escala; i++)
                {
                    int posicaoQueFechaVitoria = conjuntoDePosicoesQueFechamVitoria[i];
                    if (jogo.Casas[posicaoQueFechaVitoria].Marca == Dominio.Marca.Vazio)
                    {
                        quantidadeDeVazio++;
                        posicaoAJogar = posicaoQueFechaVitoria;
                    }
                    if (jogo.Casas[posicaoQueFechaVitoria].Marca == marcaParaPreencher)
                    {
                        quantidadeDeMarcas++;
                    }
                }

                if (quantidadeDeMarcas == jogo.Escala - 1 && quantidadeDeVazio == 1)
                    break;
                else
                    posicaoAJogar = 0;
            }

            return posicaoAJogar;
        }

        private NoDeMemoria ObterNoFilhoPorMelhorEscolhaMemorizada(NoDeMemoria noPai)
        {
            return noPai.NosFilhos.Count > 0 ? noPai.NosFilhos.FirstOrDefault(n => n.PesoDeMelhorEscolha == noPai.NosFilhos.Max(p => p.PesoDeMelhorEscolha) && n.PesoDeMelhorEscolha >4) : null;
        }
        #endregion

        private void AtualizarCasasDaUltimaJogadaPelasCasasAtuais(IList<Casa> casasDaUltimaJogada, IList<Casa> casasAtuais, int posicaoDoAdversario)
        {
            if (posicaoDoAdversario != 0)
                casasDaUltimaJogada[posicaoDoAdversario].Marca = casasAtuais[posicaoDoAdversario].Marca;

            casasDaUltimaJogada[NoDaUltimaJogada.Posicao].Marca = MinhaMarca;
        }

        int ObterPosicaoDaJogadaAdversaria(IList<Casa> casasDaJogadaAdversaria)
        {
            var casasNaoMarcadasAntes = CasasDaUltimaJogada.Where(c => c != null && c.Marca == Dominio.Marca.Vazio);
            var casasMarcadasAtualmente = casasDaJogadaAdversaria.Where(c => c != null && c.Marca != Dominio.Marca.Vazio);
            return casasMarcadasAtualmente.Count() > 0 ? casasNaoMarcadasAntes.First(x => casasMarcadasAtualmente.Any(c => x.Posicao == c.Posicao)).Posicao : 0;
        }

        private bool EhPrimeiraRodada(JogoDaVelha jogo)
        {
            bool adversarioNaoJogouNenhumaVez = ObterPosicaoDaJogadaAdversaria(jogo.Casas) == 0;
            return adversarioNaoJogouNenhumaVez;
        }

        void MontarCasasDaUltimaJogadaZerada(int escala)
        {
            CasasDaUltimaJogada = new List<Casa>() { null };
            for (int i = 1; i <= escala * escala; i++)
            {
                CasasDaUltimaJogada.Add(new Casa(i));
            }

        }
    }
}
