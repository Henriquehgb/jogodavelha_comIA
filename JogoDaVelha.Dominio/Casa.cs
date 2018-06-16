using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoJogoDaVelha.Dominio
{
    public class Casa
    {
        public static implicit operator Marca(Casa casa)
        {
            return casa.Marca;
        }

        public Casa(int posicao)
        {
            Posicao = posicao;
        }
        public int Posicao { get; set; }
        public Marca Marca { get; set; }

        public override string ToString()
        {
            return Posicao + " - " + Marca;
        }
    }
}
