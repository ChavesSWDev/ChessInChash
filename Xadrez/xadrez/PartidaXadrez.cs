using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branco;
            Terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdeMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if(pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        } 

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdeMovimentos();
            if(pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }

            tab.ColocarPeca(p, origem);
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            ConsoleColor corAux = Console.ForegroundColor;
            Peca pecaCapturada = executaMovimento(origem, destino);

            if(estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                Console.ForegroundColor = ConsoleColor.Red;
                throw new TabuleiroExceptions("Você não pode se colocar em Xeque!");
            }

            if(estaEmXeque(adversaria(JogadorAtual)))
            {
                xeque = true;
            } else
            {
                xeque = false;
            }

            if(testeXequemate(adversaria(JogadorAtual)))
            {
                Terminada = true;
            } else
            {
                Turno++;
                mudaJogador();
            }
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            ConsoleColor corAux = Console.ForegroundColor;
            if (tab.peca(pos) == null)
            {
                
                Console.ForegroundColor = ConsoleColor.Red;
                throw new TabuleiroExceptions("\n\tNão existe peça na posição de Origem escolhida!");
                Console.ForegroundColor = corAux;
            }
            Console.ForegroundColor = corAux;
            if (JogadorAtual != tab.peca(pos).Cor)
            {
                corAux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                throw new TabuleiroExceptions("\n\tA peça de Origem escolhida não é sua!");
                Console.ForegroundColor = corAux;
            }
            Console.ForegroundColor = corAux;
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                corAux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                throw new TabuleiroExceptions("\n\tNão há movimentos possíveis para a peça de Origem escolhida!");
                Console.ForegroundColor = corAux;
            }
            Console.ForegroundColor = corAux;
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            ConsoleColor corAux = Console.ForegroundColor;

            if (!tab.peca(origem).movimentoPossivel(destino))
            {
                corAux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                throw new TabuleiroExceptions("\n\tPosição de Destino Inválida!");
            }
        }

        private void mudaJogador()
        {
            if(JogadorAtual == Cor.Branco)
            {
                JogadorAtual = Cor.Preto;
            } else
            {
                JogadorAtual = Cor.Branco;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach(Peca x in capturadas)
            {
                if(x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }
        
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if(cor == Cor.Branco)
            {
                return Cor.Preto;
            } else
            {
                return Cor.Branco;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach(Peca x in pecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if(R == null)
            {
                throw new TabuleiroExceptions("Não tem rei da cor " + cor + " no Tabuleiro!");
            }

            foreach(Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if(!estaEmXeque(cor))
            {
                return false;
            } 

            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for(int i = 0; i < tab.Linhas; i++)
                {
                    for(int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i,j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if(!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new posicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        
        private void colocarPecas()
        {
            //PEÇAS PRETAS
            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preto));
            colocarNovaPeca('e', 8, new Rainha(tab, Cor.Preto));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));

            for (int i = 0; i < 8; i++)
            {
                tab.ColocarPeca(new Peao(tab, Cor.Preto), new Posicao(1, i));
            }

            //PEÇAS BRANCAS
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branco));
            colocarNovaPeca('d', 1, new Rainha(tab, Cor.Branco));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branco));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branco));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branco));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branco));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branco));

            for (int i = 0; i < 8; i++)
            {
                tab.ColocarPeca(new Peao(tab, Cor.Branco), new Posicao(6, i));
            }
        }
    }
}
