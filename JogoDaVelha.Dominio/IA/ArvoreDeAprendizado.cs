
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoJogoDaVelha.Dominio.IA
{
    public class ArvoreDeAprendizado
    {
        public ArvoreDeAprendizado(int escala)
        {
            MontarArvore(escala);
        }

        private void MontarArvore(int escala)
        {
            var digitos = new Int32[(escala * escala) + 1]; for (Int32 d = 1; d <= (escala * escala); d++) { digitos[d] = d; }

            NoRaiz = new NoDeMemoria(null, 0);

            GerarNos(digitos, NoRaiz);
        }

        public NoDeMemoria NoRaiz { get; set; }

        public IList<NoDeMemoria> Nos { get; set; }

        private void GerarNos(Int32[] digitos, NoDeMemoria noPai)
        {
            foreach (Int32 digito in digitos)
            {
                if (digito != 0)
                {
                    NoDeMemoria novoNoFilho = new NoDeMemoria(noPai, digito);
                    noPai.NosFilhos.Add(novoNoFilho);
                }
            }
        }
    }
}
