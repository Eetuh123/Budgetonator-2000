using System;

namespace Budgetinator_2000.Mathcalculations
{
    public static class palkanlaskenta
    {
        public static decimal VuosiPalkka(decimal kuukausiPalkka)
        {
            return kuukausiPalkka * 12;
        }

        public static decimal KuukausiPalkka(decimal kuukausiPalkka)
        {
            return TuntiPalkka * 150;
        }

        public static decimal Veropidatys(decimal bruttoTulo, decimal veroProsentti)
        {
            return bruttoTulo * veroProsentti / 100;
        }

        public static decimal NettoPalkka(decimal bruttoTulo, decimal veroPidatys)
        {
            return bruttoTulo - veroPidatys;
        }

        public static decimal Tuntipalkka(decimal kuukausiPalkka, decimal kuukaudenTunnit)
        {
            if (kuukaudenTunnit == 0) throw new DivideByZeroException("Kuukauden tunnit eivät voi olla nolla.");
            return kuukausiPalkka / kuukaudenTunnit;
        }

        public static decimal YlityoKorvaus(decimal tuntipalkka, decimal ylityoProsentti, decimal ylityoTunnit)
        {
            return tuntipalkka * (1 + ylityoProsentti / 100) * ylityoTunnit;
        }
    }
}