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

        public static decimal LaskeVuosittaisetSaastot(IEnumerable<decimal> kuukausittaisetSäästöt)
        {
            if (kuukausittaisetSäästöt == null)
                throw new ArgumentException("Kuukausittaisten säästöjen lista ei saa olla null.");

            decimal summa = 0m;
            bool Tyhjä = true;

            foreach (var säästö in kuukausittaisetSäästöt)
            {
                summa += säästö;
                Tyhjä = false;
            }

            if (Tyhjä)
                throw new ArgumentException("Kuukausittaisten säästöjen lista ei saa olla tyhjä.");

            return summa;
        }

        public static decimal LaskeVuosittaisetKulut(IEnumerable<decimal> kuukausittaisetKulut)
        {
            if (kuukausittaisetKulut == null)
                throw new ArgumentException("Kuukausittaisten kulujen lista ei saa olla null.");

            decimal summa = 0m;
            bool tyhjä = true;

            foreach (var kulu in kuukausittaisetKulut)
            {
                summa += kulu;
                tyhjä = false;
            }

            if (tyhjä)
                throw new ArgumentException("Kuukausittaisten kulujen lista ei saa olla tyhjä.");

            return summa;
        }

        public static decimal KulujenOsuusProsentteina(decimal kulut, decimal tulot)
        {
            if (tulot == 0) throw new DivideByZeroException("Tulot eivät saa olla nolla.");
            return (kulut / tulot) * 100;
        }

        public static decimal VelkaAste(decimal velatYhteensä, decimal varatYhteensä)
        {
            if (varatYhteensä == 0) throw new DivideByZeroException("Varat eivät voi olla nolla.");
            return (velatYhteensä / varatYhteensä) * 100;
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