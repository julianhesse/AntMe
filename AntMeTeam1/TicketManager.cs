using System;
using System.Collections.Generic;
using System.Text;
using AntMe.Deutsch;
using AntMe.Player.AntMeTeam1;
using System.Linq;

namespace AntMe.Spieler
{
    public class TicketManager
    {
        #region Singleton

        private static TicketManager _instance;

        public static TicketManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TicketManager();
                }
                return _instance;
            }
        }

        #endregion

        private const String obsts = "obst";
        private const String zuckers = "zucker";
        private const String wanzes = "wanze";
        private const String fameises = "ameise";

        private bool hostile = false;
        public Spielobjekt bau = null;
        private int time = 0;

        private List<AntMeTeam1Klasse> ameisen = new List<AntMeTeam1Klasse>(); //freundliche Ameisen
        private List<AntMeTeam1Klasse> army = new List<AntMeTeam1Klasse>(); //freundliche Ameisen
        private List<Ameise> fAmeisen = new List<Ameise>();                    //feindliche Ameisen
        private Dictionary<AntMeTeam1Klasse, Ameise> attackPlan = new Dictionary<AntMeTeam1Klasse, Ameise>();

        private Queue<Ticket> oTickets = new Queue<Ticket>(); //Obsttickets
        private Queue<Ticket> zTickets = new Queue<Ticket>(); //Zuckertickets
        private List<Ticket> fTickets = new List<Ticket>(); //feindliche Ameisentickets
        private List<Ticket> wTickets = new List<Ticket>(); //Wanzentickets

        private List<Zucker> zListe = new List<Zucker>();
        private List<Obst> oListe = new List<Obst>();

        internal int GetTime() {
            return time;
        }

        internal int IncTime() {
            createAttackPlan();
            time++;
            return time;
        }

        internal void createAttackPlan() {
            // entferne gestorbene feindliche Ameisen
            fTickets.RemoveAll(item => item.Ameise.AktuelleEnergie <= 0);
            attackPlan.Clear();

            foreach (var item in fTickets)
            {
                item.Score = score(item.Ameise);
            }

            fTickets.Sort((x, y) => y.Score.CompareTo(x.Score));

            if (fTickets.Count > 1 && fTickets[0].Score < fTickets[1].Score) throw new Exception("wrong order");

            // Suche für jeden Feind die nächste Armeise
            foreach (var feind in fTickets) {
                army.Sort((x,y) => Koordinate.BestimmeEntfernung(x, feind.Ameise).CompareTo(Koordinate.BestimmeEntfernung(feind.Ameise, y)));
                foreach (var a in army)
                {
                    if (attackPlan.ContainsKey(a)) continue;
                    attackPlan[a] = feind.Ameise;
                }
            }
        }

        internal int CountZuckerTickets() {
            return zTickets.Count;
        }

        internal int CountObstTickets() {
            return oTickets.Count;
        }

        internal int CountFTicket() {
            return fTickets.Count;
        }

        internal void ReportHostile()
        {
            hostile = true;
        }

        internal bool ReportSugar(Zucker zucker)
        {
            if (zListe.Contains(zucker)) return false;

            zListe.Add(zucker);

            // throw new Exception(zListe.ToString() + " " + zListe.Count);

            if (zListe.Count > 10) zListe.RemoveAt(0);

            int mengeTickets = zucker.Menge / 10;
            for (int i = 0; i < mengeTickets; i++)
            {
                zTickets.Enqueue(new Ticket() { Zucker = zucker });
            }

            return true;           
        }

        internal bool ReportObst(Obst obst)
        {
            if (oListe.Contains(obst)) return false;

            oListe.Add(obst);

            if (oListe.Count > 10) oListe.RemoveAt(0);

            int mengeTickets = obst.Menge / 10;
            for (int i = 0; i < mengeTickets; i++)
            {
                oTickets.Enqueue(new Ticket() { Obst = obst });
            }

            return true;
        }

        internal void ReportFAmeise(Ameise ameise)
        {
            bool known = false;
            foreach (var ticket in fTickets)
            {
                if (ticket.Ameise == ameise)
                {
                    known = true;
                    break;
                }
            }
            if (!known)
            {
                fTickets.Add(new Ticket() {Ameise = ameise, AngriffsPower = 0});
            }
        }

        internal void ReportWanze(Wanze wanze)
        {
            bool known = false;
            foreach (var ticket in wTickets)
            {
                if (ticket.Wanze == wanze)
                {
                    known = true;
                    break;
                }
            }
            if (!known)
            {
                wTickets.Add(new Ticket() {Wanze = wanze, AngriffsPower = 0});
            }
        }

        internal void RegisterAmeise(AntMeTeam1Klasse ameise)
        {
            if (!ameisen.Contains(ameise))
            {
                ameisen.Add(ameise);
                if (ameise.Kaste == "Fighter") {
                    army.Add(ameise);
                }
            }
        }

        internal void UnregisterAmeise(AntMeTeam1Klasse ameise, Ticket ticket, String ticketType)
        {
            if (!ameise.hatGetragen()) {
                ReturnTicket(ticket, ticketType, ameise.Angriff);
            }
            ameisen.Remove(ameise);
            army.Remove(ameise);
        }

        internal void ReturnTicket(Ticket ticket, String ticketType, int angriff) {
            if (ticket != null)
            {
                int index;
                switch (ticketType)
                {
                    case obsts:
                        oTickets.Enqueue(ticket);
                        break;
                    case zuckers:
                        zTickets.Enqueue(ticket);
                        break;
                    case wanzes:
                        index = wTickets.IndexOf(ticket);
                        if (index != -1 && index < wTickets.Count) {
                            wTickets[index].AngriffsPower -= angriff;
                        }
                        break;
                    case fameises:
                        index = fTickets.IndexOf(ticket);
                        if (index != -1 && index < wTickets.Count) {
                            fTickets[index].AngriffsPower -= 1;
                        }
                        break;

                    default:
                        break;
                }
            }
        }


        internal bool GetHostility()
        {
            return hostile;
        }

        internal Ticket ZGetTicket()
        {
            if (zTickets.Count > 0)
            {
                return zTickets.Dequeue();
            }
            return null;
        }

        internal Ticket OGetTicket()
        {
            if (oTickets.Count > 0)
            {
                return oTickets.Dequeue();
            }
            return null;
        }

        internal void WTot(Ticket ticket) {
            wTickets.Remove(ticket);
        }

        internal Ticket WGetTicket()
        {
            if (wTickets.Count > 0)
            {
                Ticket ticket = null;
                int shortest = int.MaxValue;
                foreach (var t in wTickets) {
                    if (bau == null) {
                        throw new Exception("Bau is null");
                    }
                    if (t.Wanze == null) {
                        throw new Exception("Wanze is null");
                    }
                    int dist = Koordinate.BestimmeEntfernung(t.Wanze, this.bau);

                    if (dist < shortest) {
                        shortest = dist;
                        ticket = t;
                    }
                }

                ticket.AngriffsPower += 20;

                if (ticket.AngriffsPower != wTickets[wTickets.IndexOf(ticket)].AngriffsPower) {
                    throw new Exception("This is bad!");
                }

                return ticket;
            }
            return null;
        }

        internal int score(Ameise ameise) {
            // (b * (k - EntfernungBau)) + (e * Energie) + (a * Angriff)
            int k = 500;
            double b = 0.25; // Gewichtet Entfernung zum Bau
            double e = 0.1; // Gewichtet die Energie
            double a = 1.0; // Gewichtet

            int entfernung = Koordinate.BestimmeEntfernung(ameise, this.bau);

            return (int) (b * (k-entfernung) + e * ameise.AktuelleEnergie + a * ameise.Angriff);
        }

        internal Ameise FAmeise(AntMeTeam1Klasse ameise)
        {
            if (attackPlan.ContainsKey(ameise)) return attackPlan[ameise];

            return null;
            // if (fTickets.Count > 0)
            // {
            //     Ticket ticket = null;

            //     fTickets.RemoveAll(item => item.Ameise.AktuelleEnergie <= 0);

            //     for (int i = 0; i < fTickets.Count; i++)
            //     {
            //         fTickets[i].Score = score(fTickets[i].Ameise);

            //         int f = 10;

            //         if (ticket == null || (ticket.Score - f * ticket.AngriffsPower < fTickets[i].Score - f * fTickets[i].AngriffsPower))
            //         {
            //             ticket = fTickets[i];
            //         }
            //     }

            //     ticket.AngriffsPower += 1;

            //     if (ticket.AngriffsPower != wTickets[wTickets.IndexOf(ticket)].AngriffsPower) {
            //         throw new Exception("This is bad!");
            //     }

            //     return ticket;
            // }
            // return null;
        }
    }

    public class Ticket
    {
        public Zucker Zucker { get; set; }
        public Obst Obst { get; set; }
        public Ameise Ameise { get; set; }
        public Wanze Wanze { get; set; }
        public int AngriffsPower { get; set; }
        public int Score { get; set;}
    }


}