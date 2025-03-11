using System;
using System.Collections.Generic;

namespace Budgetinator_2000.Mathcalculations
{
    public static class Bujetointikaavoja
    {
        public static decimal LaskeKuukausittaisetSaastot(decimal tulot, decimal kulut)
        {
            return tulot - kulut;
        }

        public static decimal LaskeVuosittaisetSaastot(IEnumerable<decimal> kuukausittaisetS��st�t)
        {
            if (kuukausittaisetS��st�t == null)
                throw new ArgumentException("Kuukausittaisten s��st�jen lista ei saa olla null.");

            decimal summa = 0m;
            bool Tyhj� = true;

            foreach (var s��st� in kuukausittaisetS��st�t)
            {
                summa += s��st�;
                Tyhj� = false;
            }

            if (Tyhj�)
                throw new ArgumentException("Kuukausittaisten s��st�jen lista ei saa olla tyhj�.");

            return summa;
        }

        public static decimal LaskeVuosittaisetKulut(IEnumerable<decimal> kuukausittaisetKulut)
        {
            if (kuukausittaisetKulut == null)
                throw new ArgumentException("Kuukausittaisten kulujen lista ei saa olla null.");

            decimal summa = 0m;
            bool tyhj� = true;

            foreach (var kulu in kuukausittaisetKulut)
            {
                summa += kulu;
                tyhj� = false;
            }

            if (tyhj�)
                throw new ArgumentException("Kuukausittaisten kulujen lista ei saa olla tyhj�.");

            return summa;
        }

        public static decimal KulujenOsuusProsentteina(decimal kulut, decimal tulot)
        {
            if (tulot == 0) throw new DivideByZeroException("Tulot eiv�t saa olla nolla.");
            return (kulut / tulot) * 100;
        }

        public static decimal VelkaAste(decimal velatYhteens�, decimal varatYhteens�)
        {
            if (varatYhteens� == 0) throw new DivideByZeroException("Varat eiv�t voi olla nolla.");
            return (velatYhteens� / varatYhteens�) * 100;
        }

        public static decimal LaskeKuukaudenTulot(IEnumerable<Transaction> tapahtumat, int vuosi, int kuukausi)
        {
            if (tapahtumat == null)
                throw new ArgumentException("Tapahtumalista ei saa olla null.");

            decimal summa = 0m;
            foreach (var tapahtuma in tapahtumat)
            {
                if (tapahtuma.Type == TransactionType.Income &&
                    tapahtuma.Date.Year == vuosi &&
                    tapahtuma.Date.Month == kuukausi)
                {
                    summa += tapahtuma.Amount;
                }
            }
            return summa;
        }

        public static decimal LaskeKuukaudenKulut(IEnumerable<Transaction> tapahtumat, int vuosi, int kuukausi)
        {
            if (tapahtumat == null)
                throw new ArgumentException("Tapahtumalista ei saa olla null.");

            decimal summa = 0m;
            foreach (var tapahtuma in tapahtumat)
            {
                if (tapahtuma.Type == TransactionType.Expense &&
                    tapahtuma.Date.Year == vuosi &&
                    tapahtuma.Date.Month == kuukausi)
                {
                    summa += tapahtuma.Amount;
                }
            }
            return summa;
        }
    }
}