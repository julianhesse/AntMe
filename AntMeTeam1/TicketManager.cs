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

        private Bau iBau = null;

        private List<AntMeTeam1Klasse> ameisen = new List<AntMeTeam1Klasse>(); //freundliche Ameisen
        private List<Ameise> fAmeisen = new List<Ameise>();                    //feindliche Ameisen

        private Queue<Ticket> Tickets = new Queue<Ticket>(); //Obst- und Zuckertickets
        private Queue<Ticket> fTickets = new Queue<Ticket>(); //feindliche Ameisentickets
        private Queue<Ticket> wTickets = new Queue<Ticket>(); //Wanzentickets


        #region Report

        internal void ReportBau(Bau bau)
        {
            iBau = bau;
        }

        internal void ReportSugar(Zucker zucker)
        {
            bool known = false;
            foreach (var ticket in Tickets)
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
                    Tickets.Enqueue(new Ticket() { Zucker = zucker });
                }
            }
        }

        internal void ReportObst(Obst obst)
        {
            bool known = false;
            foreach (var ticket in Tickets)
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
                    Tickets.Enqueue(new Ticket() { Obst = obst });
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

        #endregion


        internal void ClearEnemyTickets()
        {
            fTickets.Clear();
            wTickets.Clear();
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
                        Tickets.Enqueue(ticket);
                        break;
                    case zuckers:
                        Tickets.Enqueue(ticket);
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

        internal Ticket ZGetTicket()
        {
            if (Tickets.Count > 0)
            {
                return Tickets.Dequeue();
            }
            return null;
        }

        internal Ticket OGetTicket()
        {
            if (Tickets.Count > 0)
            {
                return Tickets.Dequeue();
            }
            return null;
        }

        internal Ticket WGetTicket(AntMeTeam1Klasse ameise)
        {

            if (wTickets.Count > 0)
            {
                return wTickets.Dequeue();
            }
            return null;
        }

        internal Ticket FGetTicket(AntMeTeam1Klasse ameise)
        {

            if (fTickets.Count > 0)
            {
                return fTickets.Dequeue();
            }
            return null;
        }

        internal Bau GetBau()
        {
            return iBau;
        }

        //Sortiert das Ticket Queue so, dass für die spezifische Ameise die Tickets, die am dichtesten sind, zuerst abgearbeitet werden
        internal Queue<Ticket> SortTickets(Queue<Ticket> tickets, String ticketType, AntMeTeam1Klasse ameise)
        {
            List<Ticket> ticketList = new List<Ticket>();
            int counter = 0;     //Zählt die druchlaufsrunden
            int index = 0;      //Index des am kürzesten enfernten Tickets
            int sDistance = -1; //kürzeste Distanz zwischen Ameise und ticket

            //Schaut für jedes Ticket, welche Distanz zwischen Ameise und Ticket herrscht
            foreach (Ticket ticket in tickets)
            {
                Spielobjekt spielobjekt = null;
                switch (ticketType)
                {
                    case fameises:
                        spielobjekt = ticket.Ameise;
                        int distance1 = Koordinate.BestimmeEntfernung(ameise, spielobjekt);
                        if (distance1 < sDistance)
                        {
                            sDistance = distance1;
                            index = counter;
                        }
                        ticketList.Add(ticket);
                        break;

                    case wanzes:
                        spielobjekt = ticket.Wanze;
                        int distance2 = Koordinate.BestimmeEntfernung(ameise, spielobjekt);
                        if (distance2 < sDistance)
                        {
                            sDistance = distance2;
                            index = counter;
                        }
                        ticketList.Add(ticket);
                        break;


                    default:
                        ticketList.Add(ticket);
                        break;
                }
                counter++;
                ticketList.Add(ticket);
            }

            //Entlehre den Queue
            tickets.Clear();

            //Fügt das Ticket, das am nähsten ist zuerst ein
            if(sDistance != -1)
            {
                tickets.Enqueue(ticketList[index]);
                ticketList.RemoveAt(index);
            }

            //Fügt die restlichen Tickets wieder ein
            foreach (Ticket ticket in ticketList)
            {
                tickets.Enqueue(ticket);
            }

            return tickets;
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