using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fisher
{
    public partial class Form1 : Form
    {
        string sciezka = null;
        SystemDecyzyjny wczytanySystemDecyzyjny;
        int ileWartosciWCBox = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnWczytaj_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                sciezka = openFileDialog1.FileName;
                wczytanySystemDecyzyjny = new SystemDecyzyjny(sciezka);
                ileWartosciWCBox = wczytanySystemDecyzyjny.listaWierszy[0].listaAtrybutow.Count();
                this.cBox.Items.Clear();
                this.cBox.Text = "1";

                for (int i = 1; i < ileWartosciWCBox; i++)
                {
                    this.cBox.Items.Add(i);
                }
            }
        }

        private void btnGeneruj_Click(object sender, EventArgs e)
        {
            List<string> listaItems = new List<string>();
            List<Separacja> listaPrawieSkonczona = new List<Separacja>();
            string wierszListBox = null;
            listBox1.Items.Clear();
            listaPrawieSkonczona = wczytanySystemDecyzyjny.fisher(Convert.ToInt32(cBox.Text));

            for (int i = 0; i < listaPrawieSkonczona[0].listaAtrybutow.Count(); i++)
            {
                foreach (var separacja in listaPrawieSkonczona)
                {
                    wierszListBox += separacja.listaAtrybutow[i].ToString() + " ";
                }

                wierszListBox += wczytanySystemDecyzyjny.listaWierszy[i].listaAtrybutow.Last();
                listBox1.Items.Add(wierszListBox);
                wierszListBox = null;
            }
            wczytanySystemDecyzyjny = new SystemDecyzyjny(sciezka);
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.ToolTipTitle = "Niedozwolone działanie.";
            tooltip.Show("Pole służy tylko do wyświetlenia wyniku.", this.listBox1, 0, -20, 2000);
            e.Handled = true;
        }

        private void cBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
