using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monotifications;

namespace NotificationServerGUI
{
    class NotificationServerGUI : NotificationServer
    {	
		public NotificationServerGUI () : base("server.ini")
		{
		}
		
		public NotificationServerGUI (string serverINI) : base(serverINI, 7778)
		{
		}

        public NotificationServerGUI(string serverINI, int listenPort)
            : base(serverINI, listenPort)
		{
		}
		
		public static void _Main (string[] args)
		{
		}
	}
}
