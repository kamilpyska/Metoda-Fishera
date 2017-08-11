using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fisher
{
    class SystemDecyzyjny
    {
        public List<WierszSystemu> listaWierszy;
        List<List<Separacja>> listaListSepracjiWszystkichDecyzji;

        public SystemDecyzyjny(string path)
        {
            this.listaListSepracjiWszystkichDecyzji = new List<List<Separacja>>();
            this.listaWierszy = new List<WierszSystemu>();
            string[] wiersze = File.ReadAllLines(path);

            foreach (var wiersz in wiersze.ToList())
            {
                if (wiersz != null && wiersz.Count() >= 2)
                {
                    listaWierszy.Add(new WierszSystemu(wiersz.Trim()));
                }
            }
        }

        public List<Separacja> fisher(int cbox)
        {
            List<int> listaDecyzji = new List<int>();
            List<WierszSystemu> listaWierszyZTaSamaDecyzja = new List<WierszSystemu>();
            List<WierszSystemu> listaWierszyZRoznymiDecyzjami = new List<WierszSystemu>();
            List<WierszSystemu> listaPomocnicza = new List<WierszSystemu>();
            List<Separacja> listaDoWyswietlenia = new List<Separacja>();
            List<int> listaUzytychIndeksow=new List<int>();

            foreach (var wiersz in this.listaWierszy)
            {
                listaDecyzji.Add(wiersz.listaAtrybutow.Last());
                listaPomocnicza.Add(wiersz);
            }
            
            foreach (var decyzja in listaDecyzji.Distinct())
            {
                foreach (var wiersz in listaPomocnicza)
                {
                    if (wiersz.listaAtrybutow.Last() == decyzja)
                    {
                        listaWierszyZTaSamaDecyzja.Add(wiersz);
                    }
                    else
                    {
                        listaWierszyZRoznymiDecyzjami.Add(wiersz);
                    }
                }

                zrobSepracjeKlasyDecyzyjnej(listaWierszyZTaSamaDecyzja, listaWierszyZRoznymiDecyzjami);
                listaWierszyZTaSamaDecyzja.Clear();
                listaWierszyZRoznymiDecyzjami.Clear();
            }

            int licznikStopu = 0;
            //chodzi o to by była tu jakaś wartość, ktora na pewno nie wystapi jako decyzja w systemie decyzyjnym
            int decyzjaPoprzedniego = -10;

            for (int i = 0; i < listaListSepracjiWszystkichDecyzji[0].Count(); i++)
            {
                for (int j = 0; j < listaListSepracjiWszystkichDecyzji.Count(); j++)
                {
                    if (listaListSepracjiWszystkichDecyzji[j][i].decyzja!=decyzjaPoprzedniego)
                    {
                        if (!listaUzytychIndeksow.Contains(listaListSepracjiWszystkichDecyzji[j][i].indeks))
                        {
                            listaUzytychIndeksow.Add(listaListSepracjiWszystkichDecyzji[j][i].indeks);
                            listaDoWyswietlenia.Add(listaListSepracjiWszystkichDecyzji[j][i]);
                            licznikStopu++;
                        }
                    }

                    decyzjaPoprzedniego = listaListSepracjiWszystkichDecyzji[j][i].decyzja;

                    if (licznikStopu == cbox)
                    {
                        j = listaListSepracjiWszystkichDecyzji.Count();
                        i = listaListSepracjiWszystkichDecyzji[0].Count();
                    }
                }
            }

            return listaDoWyswietlenia;
        }

        private void zrobSepracjeKlasyDecyzyjnej(List<WierszSystemu> listaZTaSamaDecyzja, List<WierszSystemu>listaPozostałeWiersze)
        {
            List<Separacja> listaSeparacji = new List<Separacja>();
            
            for (int i = 0; i < listaZTaSamaDecyzja[0].listaAtrybutow.Count() - 1; i++)
            {
                double cardC = listaZTaSamaDecyzja.Count();
                double cardU = listaPozostałeWiersze.Count() + cardC;
                List<double> listaLicznik = new List<double>();
                List<double> listaLicznikRoznaDecyzja = new List<double>();
                List<double> listaLicznikZC = new List<double>();
                List<double> listaLicznikZU = new List<double>();
                List<double> listaStopni = new List<double>();
                
                //C z kreska
                foreach (var wiersz in listaZTaSamaDecyzja)
                {
                    listaLicznik.Add(wiersz.listaAtrybutow[i]);
                }
                double cZKreska = listaLicznik.Sum() / cardC;
                
                //C z daszkiem
                foreach (var wiersz in listaPozostałeWiersze)
                {
                    listaLicznikRoznaDecyzja.Add(wiersz.listaAtrybutow[i]);
                }
                double cZDaszkiem = listaLicznikRoznaDecyzja.Sum() / (cardU-cardC);

                //Z od C z kreska
                foreach (var wiersz in listaZTaSamaDecyzja)
                {
                    listaLicznikZC.Add((wiersz.listaAtrybutow[i] - cZKreska)*(wiersz.listaAtrybutow[i] - cZKreska));
                }
                double zOdCZKreska = listaLicznikZC.Sum() / cardC;
                
                //Z od C z daszkiem
                foreach (var wiersz in listaPozostałeWiersze)
                {
                    listaLicznikZU.Add((wiersz.listaAtrybutow[i] - cZDaszkiem) * (wiersz.listaAtrybutow[i] - cZDaszkiem));
                }
                double zOdCZDaszkiem = listaLicznikZU.Sum() / (cardU - cardC);

                //S
                double s = ((cZKreska - cZDaszkiem) * (cZKreska - cZDaszkiem)) / (zOdCZKreska + zOdCZDaszkiem);
                Separacja obiekt = new Separacja(s,i+1,listaZTaSamaDecyzja[0].listaAtrybutow.Last());
                
                foreach (var wiersz in listaWierszy)
                {
                    obiekt.listaAtrybutow.Add(wiersz.listaAtrybutow[i]);
                }
                listaSeparacji.Add(obiekt);
                listaSeparacji=listaSeparacji.OrderByDescending(x=>x.stopien).ToList();
               
            }

            listaListSepracjiWszystkichDecyzji.Add(listaSeparacji);
        }
    }
}
