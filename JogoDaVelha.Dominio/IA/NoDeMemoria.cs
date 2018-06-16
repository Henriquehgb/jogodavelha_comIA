using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoJogoDaVelha.Dominio.IA
{
    public class NoDeMemoria
    {
        public NoDeMemoria(NoDeMemoria noPai, Int32 posicao)
        {
            NoPai = noPai;
            Posicao = posicao;
            PesoDeMelhorEscolha = 5;
            NosFilhos = new List<NoDeMemoria>();
        }

        public NoDeMemoria NoPai { get; set; }

        public Int32 Posicao { get; set; }
        public Int32 PesoDeMelhorEscolha { get; set; }

        public IList<NoDeMemoria> NosFilhos { get; set; }

        public NoDeMemoria ObterNoPeloPeso()
        {
            return NosFilhos.First(n => n.PesoDeMelhorEscolha == NosFilhos.Max(x => x.PesoDeMelhorEscolha));
        }

        public NoDeMemoria ObterNoDaPosicao(Int32 posicao)
        {
            return NosFilhos.FirstOrDefault(n => n.Posicao == posicao);
        }

        public override string ToString()
        {
            return NoPai != null ? NoPai + " " + Posicao.ToString() : "Raiz";
        }

        internal NoDeMemoria CarregarNoFilhoDaPosicao(int posicao)
        {
            if (PosicaoJaEstahAcima(posicao))
            {
                throw new Exception("O Computador Aprendiz solicitou uma jogada para carregar um nó filho de uma jogada já feita e por isso o jogo não pode continuar.");
            }

            NoDeMemoria no = ObterNoDaPosicao(posicao);
            if(no==null)
            {
                no = new NoDeMemoria(this,posicao);
                NosFilhos.Add(no);
            }

            return no;
        }

        private bool PosicaoJaEstahAcima(int posicao)
        {
            if (NoPai == null)
                return false;

            return posicao == Posicao || NoPai.PosicaoJaEstahAcima(posicao);
        }
    }
}
