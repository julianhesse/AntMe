using System;
using System.Collections.Generic;
using System.Text;

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
    }
}