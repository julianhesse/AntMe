using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntMe.Spieler;

namespace AntMe.Player.AntMeTeam1
{
    /// <summary>
    /// Diese Datei enthält die Beschreibung für deine Ameise. Die einzelnen Code-Blöcke 
    /// (Beginnend mit "public override void") fassen zusammen, wie deine Ameise in den 
    /// entsprechenden Situationen reagieren soll. Welche Befehle du hier verwenden kannst, 
    /// findest du auf der Befehlsübersicht im Wiki (http://wiki.antme.net/de/API1:Befehlsliste).
    /// 
    /// Wenn du etwas Unterstützung bei der Erstellung einer Ameise brauchst, findest du
    /// in den AntMe!-Lektionen ein paar Schritt-für-Schritt Anleitungen.
    /// (http://wiki.antme.net/de/Lektionen)
    /// </summary>
    [Spieler(
        Volkname = "AntNation",   // Hier kannst du den Namen des Volkes festlegen
        Vorname = "Julian",       // An dieser Stelle kannst du dich als Schöpfer der Ameise eintragen
        Nachname = "Hesse"       // An dieser Stelle kannst du dich als Schöpfer der Ameise eintragen
    )]

    
    /// Kasten stellen "Berufsgruppen" innerhalb deines Ameisenvolkes dar. Du kannst hier mit
    /// den Fähigkeiten einzelner Ameisen arbeiten. Wie genau das funktioniert kannst du der 
    /// Lektion zur Spezialisierung von Ameisen entnehmen (http://wiki.antme.net/de/Lektion7).
    [Kaste(
        Name = "Standard",                  // Name der Berufsgruppe
        AngriffModifikator = -1,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = -1, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = -1,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 2,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = 2,                // Tragkraft einer Ameise
        ReichweiteModifikator = -1,          // Ausdauer einer Ameise
        SichtweiteModifikator = 0           // Sichtweite einer Ameise
    )]

    [Kaste(
        Name = "Sammler",                  // Name der Berufsgruppe
        AngriffModifikator = -1,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = 0, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = -1,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 2,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = 2,                // Tragkraft einer Ameise
        ReichweiteModifikator = -1,          // Ausdauer einer Ameise
        SichtweiteModifikator = -1           // Sichtweite einer Ameise
    )]

    [Kaste(
        Name = "Sucher",                  // Name der Berufsgruppe
        AngriffModifikator = -1,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = -1, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = -1,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 2,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = 0,                // Tragkraft einer Ameise
        ReichweiteModifikator = -1,          // Ausdauer einer Ameise
        SichtweiteModifikator = 2           // Sichtweite einer Ameise
    )]

    [Kaste(
        Name = "Spotter",                  // Name der Berufsgruppe
        AngriffModifikator = -1,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = 0, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = 0,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 1,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = -1,                // Tragkraft einer Ameise
        ReichweiteModifikator = 0,          // Ausdauer einer Ameise
        SichtweiteModifikator = 1           // Sichtweite einer Ameise
    )]

    [Kaste(
        Name = "Fighter",                  // Name der Berufsgruppe
        AngriffModifikator = 2,             // Angriffsstärke einer Ameise
        DrehgeschwindigkeitModifikator = -1, // Drehgeschwindigkeit einer Ameise
        EnergieModifikator = 2,             // Lebensenergie einer Ameise
        GeschwindigkeitModifikator = 0,     // Laufgeschwindigkeit einer Ameise
        LastModifikator = -1,                // Tragkraft einer Ameise
        ReichweiteModifikator = -1,          // Ausdauer einer Ameise
        SichtweiteModifikator = -1           // Sichtweite einer Ameise
    )]


    public class AntMeTeam1Klasse : Basisameise
    {
        #region Self-Made

        // private Spielobjekt ziel = null; //Speichert den Zuckerberg, damit die Ameise später den Zucker wiederfindet
        private int time = -1;
        private Spielobjekt bau = null;
        private Ticket ticket = null;
        private String ticketTyp = null;
        private Ameise farmeise = null;

        private bool traegt = false;
        private Random rnd = new Random();
        private int aSammler = 0;
        private int aFighter = 0;
        private bool wegLaufen = TicketManager.Instance.GetHostility();
        private bool trotzdemAngreifen = false;
   
        private String fighter = "Fighter";
        private const String obsts = "obst";
        private const String zuckers = "zucker";
        private const String wanzes = "wanze";
        private const String fameises = "ameise";

        
        public bool hatGetragen() {
            return traegt;
        }

        private int ZufallsZahl(int wert1, int wert2)
        {
            return rnd.Next(wert1, wert2);
        }

        private void GeheZuZielOptimized(Spielobjekt spielobjekt, int abstand = -5)
        {
            int distance = Koordinate.BestimmeEntfernung(this, spielobjekt);
            //int angle = Koordinate.BestimmeRichtung(this, spielobjekt);
            DreheZuZiel(spielobjekt);
            GeheGeradeaus(distance - abstand);
        }

        private void GeheZuBauOptimized()
        {
            if (TicketManager.Instance.bau != null && EntfernungZuBau >= 10)
            {
                GeheZuZielOptimized(TicketManager.Instance.bau);
            }
            else
            {
                GeheZuBau();
            }
        }

        #endregion Self-Made

        #region Kasten

        /// <summary>
        /// Jedes mal, wenn eine neue Ameise geboren wird, muss ihre Berufsgruppe
        /// bestimmt werden. Das kannst du mit Hilfe dieses Rückgabewertes dieser 
        /// Methode steuern.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:BestimmeKaste
        /// </summary>
        /// <param name="anzahl">Anzahl Ameisen pro Kaste</param>
        /// <returns>Name der Kaste zu der die geborene Ameise gehören soll</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            int sumAnts = anzahl.Skip(1).Sum(x => x.Value);
            aSammler = anzahl["Sammler"];
            aFighter = anzahl[fighter];


            // Bestimme, ob ein Spotter gespawnt werden soll
            if (anzahl["Spotter"] < 7) {
                return "Spotter";
            }

            // return fighter;

            // return "Sammler";
            if (anzahl[fighter] < 20 && anzahl["Sammler"] > 10)
            {
                return fighter;
            }
            else
            {
                if (anzahl["Sammler"] <= 50)
                {
                    return "Sammler";
                }
                else
                {
                    if (anzahl[fighter] > 50)
                    {
                        return fighter;
                    }
                    else
                    {
                        return "Sammler";
                    }

                }
            }
        }

        #endregion

        #region Fortbewegung

        /// <summary>
        /// Wenn die Ameise keinerlei Aufträge hat, wartet sie auf neue Aufgaben. Um dir das 
        /// mitzuteilen, wird diese Methode hier aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Wartet
        /// </summary>
        public override void Wartet()
        {
            /// ::::: Initialisierung :::::
            //Initialisiere Bau
            if (TicketManager.Instance.bau == null)
            {
                GeheZuBau();
                bau = this.Ziel;
                TicketManager.Instance.bau = bau;
                if (bau == null) {
                    throw new Exception("Bau is null!");
                }
                BleibStehen();
                switch (this.Kaste)
                {
                    default:
                        TicketManager.Instance.RegisterAmeise(this);
                        break;
                }
                return;
            }

            //Hole Bau vom TicketManager
            if (bau == null) {
                bau = TicketManager.Instance.bau;
                switch (this.Kaste)
                {
                    default:
                        TicketManager.Instance.RegisterAmeise(this);
                        break;
                }
            }

            /// ::::: Initialisierung - Ende :::::

            if (this.Kaste == "Spotter")
            {
                DreheUmWinkel(Zufall.Zahl(-10, 10));
                GeheGeradeaus(100);
            }

            // Fighter soll sich zurückziehen
            if (this.Kaste == fighter)
            {
                if (Reichweite - ZurückgelegteStrecke - 10 < EntfernungZuBau)
                {
                    GeheZuBauOptimized();
                    TicketManager.Instance.ReturnTicket(ticket, ticketTyp, this.Angriff);
                    ticket = null;
                    Denke("Reichweite " + (Reichweite - ZurückgelegteStrecke));
                    return;
                }

                if (AktuelleEnergie < MaximaleEnergie)
                {
                    GeheZuBauOptimized();
                    TicketManager.Instance.ReturnTicket(ticket, ticketTyp, this.Angriff);
                    ticket = null;
                    Denke("Energie " + AktuelleEnergie);
                    return;
                }
            }

            // Fighter sollen erkunden, wenn es nichts zu tun gibt
            if (this.Kaste == fighter && ticket == null)
            {
                Denke("Fighter " + EntfernungZuBau);
                if (EntfernungZuBau > 350) {
                    GeheZuBauOptimized();
                    return;
                }
                if (ticket == null) {
                    DreheUmWinkel(Zufall.Zahl(-30, 30));
                    GeheGeradeaus(400);
                    return;
                }
            }

            //if (ziel != null && this.Kaste == fighter)
            //{
            //    if(ticket == null)
            //    {
            //        ticket = TicketManager.Instance.FGetTicket();

            //        if(ticket == null)
            //        {
            //            ziel = ticket.Ameise;
            //            ticketTyp = fameises;
            //            GeheZuZielOptimized(ziel);
            //            Denke("FAmeise!!");
            //        }
            //    }
            //    if(ticket == null)
            //    {
            //        ticket = TicketManager.Instance.WGetTicket();

            //        if(ticket == null)
            //        {
            //            ziel = ticket.Wanze;
            //            ticketTyp = wanzes;
            //            GeheZuZielOptimized(ziel);
            //            Denke("FAmeise!!");
            //        }
            //    }
            //}

            //Falls es noch ein Ziel gibt, dann gehe zum Ziel, sont hole dir ein Ticket
            if (this.Kaste == "Sammler")
            {
                if (ticket == null)
                {
                    DreheUmWinkel(ZufallsZahl(-80, 80));
                    GeheGeradeaus(80);
                    ticketTyp = null;
                }
            }
        }

        /// <summary>
        /// Erreicht eine Ameise ein drittel ihrer Laufreichweite, wird diese Methode aufgerufen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:WirdM%C3%BCde
        /// </summary>
        public override void WirdMüde()
        {
        }

        /// <summary> && zuckers == null
        /// Wenn eine Ameise stirbt, wird diese Methode aufgerufen. Man erfährt dadurch, wie 
        /// die Ameise gestorben ist. Die Ameise kann zu diesem Zeitpunkt aber keinerlei Aktion 
        /// mehr ausführen.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:IstGestorben
        /// </summary>
        /// <param name="todesart">Art des Todes</param>
        public override void IstGestorben(Todesart todesart)
        {
            TicketManager.Instance.UnregisterAmeise(this, ticket, ticketTyp);
        }

        /// <summary>
        /// Diese Methode wird in jeder Simulationsrunde aufgerufen - ungeachtet von zusätzlichen 
        /// Bedingungen. Dies eignet sich für Aktionen, die unter Bedingungen ausgeführt werden 
        /// sollen, die von den anderen Methoden nicht behandelt werden.
        /// Weitere Infos unter http://wiki.antme.net/de/API1:Tick
        /// </summary>
        public override void Tick()
        {
            // Finde tick heraus
            if (time == -1 || time < TicketManager.Instance.GetTime())
            {
                time = TicketManager.Instance.GetTime();
            }
            else
            {
                time = TicketManager.Instance.IncTime();
            }

            //Findet heraus, ob das Ticket noch gebraucht wird
            if (ticket != null && AktuelleLast == 0)
            {
                switch (ticketTyp)
                {
                    case obsts:
                        if (!BrauchtNochTräger(ticket.Obst))
                        {
                            ticket = null;
                            traegt = false;
                            BleibStehen();
                        }
                        break;

                    case zuckers:
                        if (ticket.Zucker.Menge <= 0)
                        {
                            ticket = null;
                            traegt = false;
                            BleibStehen();
                        }
                        break;
                    case wanzes:
                        if (ticket.Wanze.AktuelleEnergie == 0) {
                            TicketManager.Instance.WTot(ticket);
                            ticket = null;
                            BleibStehen();
                            Denke("Sieg!!!!");
                        }
                        break;
                    case fameises:
                        if (ticket.Ameise.AktuelleEnergie == 0) {
                            ticket = null;
                            BleibStehen();
                            Denke("Sieg!!!!");
                        }
                        break;
                }
            }


            if (Kaste == "Spotter") {
                Denke("Spotter");
            }

            //Wenn die Ameise Last hat, dann soll sie zum Bau gehen
            if (AktuelleLast != 0 && TicketManager.Instance.bau != null)
            {
                GeheZuBauOptimized();
                return;
            }


            

            if (this.Kaste == fighter)
            {
                // Finde feindliche Armeise
                farmeise = TicketManager.Instance.FAmeise(this);

                if (farmeise != null) {
                    GreifeAn(farmeise);
                    Denke("FAmeise!!  " + farmeise.AktuelleEnergie + "   " + TicketManager.Instance.CountFTicket());
                }

                // Fighter soll sich ein Ticket holen
                if (ticket == null && fameises == null)
                {
                    if (AktuelleEnergie < MaximaleEnergie / 2) {
                        GeheZuBauOptimized();
                        return;
                    }

                    // farmeise = TicketManager.Instance.FAmeise(this);
                    // if (farmeise != null) {
                    //     ticketTyp = fameises;
                    //     GeheZuZielOptimized(farmeise);
                    //     Denke("FAmeise!!  " + ticket.Ameise.AktuelleEnergie);
                    //     return;
                    // }

                    ticket = TicketManager.Instance.WGetTicket();
                    if (ticket != null) {
                        ticketTyp = wanzes;
                        GeheZuZielOptimized(ticket.Wanze);
                        Denke("Wanze!!  " + ticket.Wanze.AktuelleEnergie);
                        return;
                    }
                }
            }


            // Ticket loswerden, wenn Arbeit erledigt ist
            if (Kaste == "Sammler" && traegt && AktuelleLast == 0) {
                ticket = null;
                traegt = false;
            }

            // Sammler holen sich ein Ticket, wenn sie keins haben
            if (Kaste == "Sammler" && ticket == null)
            {
                // Nimm ein Obst Ticket
                ticket = TicketManager.Instance.OGetTicket();

                if (ticket != null)
                {
                    // ziel = ticket.Obst;
                    BleibStehen();
                    ticketTyp = obsts;
                    GeheZuZielOptimized(ticket.Obst);
                    Denke("Obst " + TicketManager.Instance.CountObstTickets());
                    return;
                }

                // Nimm ein Zucker Ticket
                ticket = TicketManager.Instance.ZGetTicket();

                if (ticket != null)
                {
                    // ziel = ticket.Zucker;
                    BleibStehen();
                    ticketTyp = zuckers;
                    GeheZuZielOptimized(ticket.Zucker);
                    Denke("Zucker " + TicketManager.Instance.CountZuckerTickets());
                    return;
                }
            }

        }

        #endregion

        #region Nahrung

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Apfel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt das betroffene Stück Obst.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Obst)"
        /// </summary>
        /// <param name="obst">Das gesichtete Stück Obst</param>
        public override void Sieht(Obst obst)
        {
            //Übergebe Obst an den Ticketmanager
            bool success = TicketManager.Instance.ReportObst(obst);

            Denke(success.ToString());

            if (AktuelleLast == 0 && BrauchtNochTräger(obst) && ticketTyp == obsts)
            {
                GeheZuZiel(obst);
            }
        }

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Zuckerhügel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt der betroffene Zuckerghügel.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:Sieht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der gesichtete Zuckerhügel</param>
        public override void Sieht(Zucker zucker)
        {
            //zuckers = zucker;
            bool success = TicketManager.Instance.ReportSugar(zucker);

            Denke(success.ToString());

            if (AktuelleLast == 0 && ticketTyp == zuckers)
            {
                GeheZuZiel(zucker);
            }
        }

        /// <summary>
        /// Hat die Ameise ein Stück Obst als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:ZielErreicht(Obst)"
        /// </summary>
        /// <param name="obst">Das erreichte Stück Obst</param>
        public override void ZielErreicht(Obst obst)
        {
            //Nur wenn noch Träger gebraucht werden
            if (BrauchtNochTräger(obst) == true && ticketTyp == obsts)
            {
                //SprüheMarkierung(1000, 300);
                Nimm(obst);
                GeheZuBau();
                traegt = true;
                Denke("" + obst.Menge);
            }
        }

        /// <summary>
        /// Hat die Ameise eine Zuckerhügel als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:ZielErreicht(Zucker)"
        /// </summary>
        /// <param name="zucker">Der erreichte Zuckerhügel</param>
        public override void ZielErreicht(Zucker zucker)
        {
            if (ticketTyp == zuckers)
            {
                Nimm(zucker);
                GeheZuBau();
                traegt = true;
            }
        }

        #endregion

        #region Kommunikation

        /// <summary>
        /// Markierungen, die von anderen Ameisen platziert werden, können von befreundeten Ameisen 
        /// gewittert werden. Diese Methode wird aufgerufen, wenn eine Ameise zum ersten Mal eine 
        /// befreundete Markierung riecht.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:RiechtFreund(Markierung)"
        /// </summary>
        /// <param name="markierung">Die gerochene Markierung</param>
        public override void RiechtFreund(Markierung markierung)
        {
            ////Gehe in Richtung Zuckerberg
            //if (markierung.Information < 1000)
            //{
            //    if (Ziel == null)
            //    {
            //        //Drehe Richtung Zuckerberg
            //        DreheInRichtung(markierung.Information);
            //        GeheGeradeaus();
            //    }
            //}

            ////Gehe zu Apfel um zu helfen
            //if (markierung.Information == 1000 && AktuelleLast == 0)
            //{
            //    GeheZuZiel(markierung);
            //}
            if (this.Ziel == null && this.Kaste == fighter && (!wegLaufen || markierung.Information == 100))
            {
                GeheZuZiel(markierung);
            }
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus dem eigenen Volk, so 
        /// wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFreund(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte befreundete Ameise</param>
        public override void SiehtFreund(Ameise ameise)
        {
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem befreundeten Volk 
        /// (Völker im selben Team), so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtVerb%C3%BCndeten(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte verbündete Ameise</param>
        public override void SiehtVerbündeten(Ameise ameise)
        {
        }

        #endregion

        #region Kampf

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem feindlichen Volk, 
        /// so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFeind(Ameise)"
        /// </summary>
        /// <param name="ameise">Erspähte feindliche Ameise</param>
        public override void SiehtFeind(Ameise ameise)
        {
            if(ameise.AktuelleLast != 0)
            {
                trotzdemAngreifen = true;
                SprüheMarkierung(100, 200);
            }
            else
            {
                trotzdemAngreifen = false;
            }

            if (!wegLaufen || trotzdemAngreifen)
            {
                GreifeAn(ameise);
            }
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Wanze, so wird diese Methode aufgerufen.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:SiehtFeind(Wanze)"
        /// </summary>
        /// <param name="wanze">Erspähte Wanze</param>
        public override void SiehtFeind(Wanze wanze)
        {

            TicketManager.Instance.ReportWanze(wanze);

            if (AktuelleLast == 0 && ticket == null)
            {
                GeheWegVon(wanze);
                Denke("Hilfe Wanze!");
                return;
            }

            if (this.Kaste == fighter && ticketTyp == wanzes)
            {
                SprüheMarkierung(0, 150);
                GreifeAn(wanze);
                Denke("Angriff! " + wanze.AktuelleEnergie);
                return;
            }
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine feindliche Ameise angreifen, wird diese Methode hier aufgerufen und die 
        /// Ameise kann entscheiden, wie sie darauf reagieren möchte.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:WirdAngegriffen(Ameise)"
        /// </summary>
        /// <param name="ameise">Angreifende Ameise</param>
        public override void WirdAngegriffen(Ameise ameise)
        {
            TicketManager.Instance.ReportFAmeise(ameise);

            TicketManager.Instance.ReportHostile();

            if(ameise.AktuelleLast != 0)
            {
                trotzdemAngreifen = true;
            }
            else
            {
                trotzdemAngreifen = false;
            }

            //Wenn Sammler angegriffen werden, dann sollen sie ihr Ticket verwerfen
            if (this.Kaste == "Sammler" && AktuelleLast == 0)
            {
                TicketManager.Instance.ReturnTicket(ticket, ticketTyp, this.Angriff);
                ticket = null;
                // ziel = null;
                ticketTyp = null;
                GeheWegVon(ameise);
                return;
            }

            if (this.Kaste == fighter)
            {
                GreifeAn(ameise);
                return;
            }

            if (this.Kaste == "Spotter")
            {
                GeheWegVon(ameise);
            }
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine Wanze angreifen, wird ameisediese Methode hier aufgerufen und die Ameise kann 
        /// entscheiden, wie sie darauf reagieren möchte.
        /// Weitere Infos unter "http://wiki.antme.net/de/API1:WirdAngegriffen(Wanze)"
        /// </summary>
        /// <param name="wanze">Angreifende Wanze</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
            TicketManager.Instance.ReportWanze(wanze);

            if (this.Kaste == fighter && ticketTyp == wanzes)
            {
                GreifeAn(wanze);
            }
            //else
            //{
            //    GeheWegVon(wanze);
            //}
        }

        #endregion
    }
}