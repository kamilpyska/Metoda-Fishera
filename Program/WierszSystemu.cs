using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fisher
{
    class WierszSystemu
    {
        public List<int> listaAtrybutow;
        public WierszSystemu(string wiersz)
        {
            this.listaAtrybutow = new List<int>();

            string[] split = wiersz.Split(new char[] { ' ' });
            for (int i = 0; i < split.Length; i++)
            {
                listaAtrybutow.Add(Convert.ToInt32(split[i]));
            }
        }
    }
}
