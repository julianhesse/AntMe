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

        private List<Zucker> zuckers = new List<Zucker>();                     //Liste mit allen Zuckerbergen 
        private List<AntMeTeam1Klasse> ameisen = new List<AntMeTeam1Klasse>(); //freundliche Ameisen
        private List<Obst> obsts = new List<Obst>();                           //Liste mit allem Obst 
        private List<Wanze> wanzes = new List<Wanze>();                        //Liste mit allen Wanzen 
        private List<Ameise> fameisen = new List<Ameise>();                    //feindliche Ameisen

        private Queue<Ticket> zTickets = new Queue<Ticket>();                   //Liste mit allen Zuckertickets
        private Queue<Ticket> oTickets = new Queue<Ticket>();                   //Liste mit allen Obsttickets
        private Queue<Ticket> wTickets = new Queue<Ticket>();                   //Liste mit allen Wanzen
        private Queue<Ticket> fTickets = new Queue<Ticket>();                   //Liste mit allen feindlichen Ameisen


        #region Report

        internal void ReportSugar(Zucker zucker)
        {
            if (!zuckers.Contains(zucker))
            {
                zuckers.Add(zucker);
                int mengeTickets = zucker.Menge / 10;
                for (int i = 0; i < mengeTickets; i++)
                {
                    zTickets.Enqueue(new Ticket() { Zucker = zucker });
                }
            }
        }
        internal void ReportObst(Obst obst)
        {
            if (!obsts.Contains(obst))
            {

            }
        }

        internal void ReportWanze(Wanze wanze)
        {
            if (!wanzes.Contains(wanze))
            {

            }
        }

        internal void ReportAmeise(Ameise ameise)
        {
            if (!fameisen.Contains(ameise))
            {

            }
        }

        #endregion Report


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
            if (zTickets.Count > 0)
            {
                return zTickets.Dequeue();
            }
            return null;
        }

    }

    public class Ticket
    {
        public Zucker Zucker { get; set; }
    }


}