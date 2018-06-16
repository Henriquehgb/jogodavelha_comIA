using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoJogoDaVelha.Dominio
{
    public class Jogador
    {
        public string Nome { get; set; }

        public Marca MinhaMarca { get; set; }

        public override string ToString()
        {
            return Nome;
        }

        public virtual void Jogar(JogoDaVelha jogoDaVelha)
        {

        }

        public virtual void ObservarFimDaJogada(JogoDaVelha jogoDaVelha)
        {

        }

        public virtual void ObservarReinicioDeJogo(JogoDaVelha jogo)
        {
            
        }
    }
}
