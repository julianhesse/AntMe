using System;
using System.Collections.Generic;
using System.Text;
using AntMe.Deutsch;
using AntMe.Player.AntMeTeam1;

namespace AntMe.Spieler
{
    public class TicketManager
    {
        #region Singleton

        private static TicketManager _instance;

        public static TicketManager _instance
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

        private List<Zucker> zuckers = new List<Zucker>();
        private List<AntMeTeam1Klasse> ameisen = new List<AntMeTeam1Klasse>();

        private Queue<Ticket> tickets = new Queue<Ticket>();

        internal void ReportSugar(Zucker zucker)
        {
            if (!zuckers.Contains(zucker))
            {
                zuckers.Add(zucker);
                int mengeTickets = zucker.Menge / 10;
                for (int i = 0; i < mengeTickets; i++)
                {
                    tickets.Enqueue(new Ticket() { Zucker = zucker });
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

        internal void UnregisterAmeise(AntMeTeam1Klasse ameise)
        {
            ameisen.Remove(ameise);
        }

        internal Ticket GetTicket()
        {
            if (tickets.Count > 0)
            {
                return tickets.Dequeue();
            }
            return null;
        }
        
    }

    public class Ticket
        {
            public Zucker Zucker { get; set; }
        }

       
}