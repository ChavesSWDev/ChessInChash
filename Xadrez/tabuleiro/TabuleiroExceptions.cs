using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro 
{
    internal class TabuleiroExceptions : Exception
    {
        public TabuleiroExceptions(string msg) : base(msg)
        {

        }
    }
}
