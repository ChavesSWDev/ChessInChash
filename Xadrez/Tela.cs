using System;
using tabuleiro;
using xadrez;
using System.Collections.Generic;

namespace Xadrez
{
    internal class Tela
    {
        public static void imprimirPartida(PartidaXadrez partida)
        {
            ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            imprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("\n\tTurno: " + partida.Turno);
            if(!partida.Terminada)
            {
                Console.WriteLine("\tAguardando jogada: " + partida.JogadorAtual);
                if (partida.xeque)
                {
                    ConsoleColor aux2 = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n\tXEQUE!!");
                    Console.ForegroundColor = aux2;
                }
            } else
            {
                ConsoleColor aux2 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n\tXEQUEMATE!!");
                Console.ForegroundColor = aux2;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\tVENCEDOR: " + partida.JogadorAtual);
                Console.ForegroundColor = aux2;
            }
            
        }

        public static void imprimirPecasCapturadas(PartidaXadrez partida)
        {
            Console.WriteLine("\n\tPeças capturadas: ");
            Console.Write("\tPretas:   [ ");
            imprimirConjunto(partida.pecasCapturadas(Cor.Branco));
            Console.Write(" ]");
            Console.Write("\n\tBrancas:  [ ");
            ConsoleColor corAux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            imprimirConjunto(partida.pecasCapturadas(Cor.Preto));
            Console.ForegroundColor = corAux;
            Console.Write(" ]");
        }

        public static void imprimirConjunto(HashSet<Peca> conjunto)
        {
            foreach(Peca x in conjunto)
            {
                Console.Write(x + " ");
            }
        }

        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            Console.WriteLine();
            ConsoleColor aux2 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t   JOGO DE XADREZ\n\n");
            Console.ForegroundColor = aux2;

            for (int i = 0; i < tab.Linhas; i++)
            {
                
                ConsoleColor aux1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\t" + (8 - i) + "  ");
                Console.ForegroundColor = aux1;
                for (int j = 0; j < tab.Colunas; j++)
                {
                    imprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\t   A B C D E F G H");
            Console.ForegroundColor = aux;
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            Console.WriteLine();
            ConsoleColor aux2 = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t   JOGO DE XADREZ\n\n");
            Console.ForegroundColor = aux2;

            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGreen;

            for (int i = 0; i < tab.Linhas; i++)
            {
                ConsoleColor aux1 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\t" + (8 - i) + "  ");
                Console.ForegroundColor = aux1;
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    } else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    imprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n\t   A B C D E F G H");
            Console.ForegroundColor = aux;
            Console.BackgroundColor = fundoOriginal;
        }

        public static void imprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("  ");
            }
            else
            {
                if (peca.Cor == Cor.Branco)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static posicaoXadrez lerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new posicaoXadrez(coluna, linha);
        }
    }
}
