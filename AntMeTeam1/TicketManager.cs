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
        private int speed = -1;

        private List<AntMeTeam1Klasse> ameisen = new List<AntMeTeam1Klasse>(); //freundliche Ameisen
        private List<Ameise> fAmeisen = new List<Ameise>();                    //feindliche Ameisen

        private Queue<Ticket> oTickets = new Queue<Ticket>(); //Obsttickets
        private Queue<Ticket> zTickets = new Queue<Ticket>(); //Zuckertickets
        private Queue<Ticket> fTickets = new Queue<Ticket>(); //feindliche Ameisentickets
        private Queue<Ticket> wTickets = new Queue<Ticket>(); //Wanzentickets


        internal void ReportSpeed(int speed)
        {
            if (this.speed < speed)
            {
                this.speed = speed;
            }
        }

        internal void ReportHostile()
        {
            hostile = true;
        }

        internal void ReportSugar(Zucker zucker)
        {
            bool known = false;
            foreach (var ticket in zTickets)
            {
                if (ticket.Zucker == zucker)
                {
                    known = true;
                    break;
                }
            }
            if (!known)
            {
                int mengeTickets = zucker.Menge / 10;
                for (int i = 0; i < mengeTickets; i++)
                {
                    zTickets.Enqueue(new Ticket() { Zucker = zucker });
                }
            }
        }

        internal void ReportObst(Obst obst)
        {
            bool known = false;
            foreach (var ticket in oTickets)
            {
                if (ticket.Obst == obst)
                {
                    known = true;
                    break;
                }
            }
            if (!known)
            {
                int mengeTickets = 250 / 10;
                for (int i = 0; i < mengeTickets; i++)
                {
                    oTickets.Enqueue(new Ticket() { Obst = obst });
                }
            }
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
                int mengeTickets = ameise.AktuelleEnergie / 30;
                for (int i = 0; i < mengeTickets; i++)
                {
                    fTickets.Enqueue(new Ticket() { Ameise = ameise });
                }
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
                int mengeTickets = wanze.AktuelleEnergie / 30;
                for (int i = 0; i < mengeTickets; i++)
                {
                    wTickets.Enqueue(new Ticket() { Wanze = wanze });
                }
            }
        }

        internal void RegisterAmeise(AntMeTeam1Klasse ameise)
        {
            if (!ameisen.Contains(ameise))
            {
                ameisen.Add(ameise);
            }
        }

        internal void UnregisterAmeise(AntMeTeam1Klasse ameise, Ticket ticket, String ticketType)
        {
            if (ticket != null)
            {
                switch (ticketType)
                {
                    case obsts:
                        oTickets.Enqueue(ticket);
                        break;
                    case zuckers:
                        zTickets.Enqueue(ticket);
                        break;
                    case wanzes:
                        wTickets.Enqueue(ticket);
                        break;
                    case fameises:
                        fTickets.Enqueue(ticket);
                        break;

                    default:
                        break;
                }
            }
            ameisen.Remove(ameise);
        }

        internal int GetSpeed()
        {
            return speed;
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

        internal Ticket WGetTicket()
        {
            if (oTickets.Count > 0)
            {
                return wTickets.Dequeue();
            }
            return null;
        }

        internal Ticket FGetTicket()
        {
            if (oTickets.Count > 0)
            {
                return fTickets.Dequeue();
            }
            return null;
        }
    }

    public class Ticket
    {
        public Zucker Zucker { get; set; }
        public Obst Obst { get; set; }
        public Ameise Ameise { get; set; }
        public Wanze Wanze { get; set; }
    }


}