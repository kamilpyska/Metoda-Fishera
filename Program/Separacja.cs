using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fisher
{
    class Separacja
    {
        public double stopien;
        public int indeks;
        public int decyzja;
        public List<int> listaAtrybutow;
        public Separacja(double stopien,int indeks,int decyzja)
        {
            this.listaAtrybutow = new List<int>();
            this.stopien = stopien;
            this.indeks = indeks;
            this.decyzja = decyzja;
        }
    }
}
