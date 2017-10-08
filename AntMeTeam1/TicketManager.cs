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

        private List<AntMeTeam1Klasse> ameisen = new List<AntMeTeam1Klasse>(); //freundliche Ameisen
        private List<Wanze> wanzes = new List<Wanze>();                        //Liste mit allen Wanzen 
        private List<Ameise> fameisen = new List<Ameise>();                    //feindliche Ameisen

        private Queue<Ticket> oTickets = new Queue<Ticket>();
        private Queue<Ticket> zTickets = new Queue<Ticket>();

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
                    case obsts: oTickets.Enqueue(ticket);
                        break;
                    case zuckers: zTickets.Enqueue(ticket);
                        break;
                    default:
                        break;
                }
            }
            ameisen.Remove(ameise);
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
    }

    public class Ticket
    {
        public Zucker Zucker { get; set; }
        public Obst Obst { get; set; }
    }


}