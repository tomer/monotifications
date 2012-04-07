using System;
using monotifications;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace monotifications
{
	public class NotificationServer : monotifications.notificationClient
	{
		public Configuration machines;
		
		public NotificationServer () : this("server.ini")
		{
		}
		
		public NotificationServer (string serverINI) : this(serverINI, 0)
		{
		}
		
		public NotificationServer (string serverINI, int listenPort) : this(serverINI, listenPort, "machines.ini")
		{
		}
		
		public NotificationServer (string serverINI, int listenPort, string machinesINI)// : base(serverINI, listenPort)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo ("en-US");
			network.setReceiveAction (Receiver);
			config = new Configuration (serverINI);
			machines = new Configuration (machinesINI);
			
			if (listenPort < 1 && config ["server"] ["serverPort"] != null)
				listenPort = int.Parse (config ["server"] ["serverPort"]);
			
			/*if (config ["server"] ["serverPort"] != null && config ["server"] ["serverPort"] != "")
				serverPort = int.Parse (config ["server"] ["serverPort"]);
			else
				serverPort = 7778;*/
			
			if (config ["server"] ["serverAddress"] == null || config ["client"] ["serverAddress"] == "0.0.0.0") 
				address = ip.AddressList[0].ToString();
			
			setProperties (address,
				listenPort,
				"server",
				address,
				listenPort);
		}
		
		protected void PurgeMachines (object state)
		{
			PurgeMachines ();
		}
		
		public void PurgeMachines ()
		{

            if (machines.Count() != 0) {
                DateTime minimum = DateTime.Now;
			    minimum = minimum.Subtract (TimeSpan.FromSeconds (updateInterval * 5));
                string[] m = list_machines();
                foreach (string key in m)
                {
                    string tmp = machines[key]["lastseen"];
                    tmp = tmp.Trim();
                    DateTime lastseen = DateTime.Parse(tmp);
                    
                    switch (doubleCompare(lastseen.ToBinary(), minimum.ToBinary()))
                    {
                        case -1: //lastseen is earlier
                            UnregisterClient(key);
                            break;
                        case 1: //lastseen is later 
                            break;
                        case 0: //lastseen and minimum are equal 
                            break;
                        default: // Should never happen!
                            break;
                    }
                }
            }
		}

        private int doubleCompare(double a, double b)
        { // Helper method because the DateTime.Compare method doesn't work well to me (issue #2)
            double diff = a - b;
            if (diff > 0) return 1;
            else if (diff < 0) return -1;
            else return 0;
        }


		private Timer autoPurgeScheduler;
		public void TriggerAutoPurge ()
		{
			//int updateInterval = 5; 
			autoPurgeScheduler = new Timer (PurgeMachines, null, 1000, 1000 * updateInterval);
		}
		
		public void Receiver (string content)
		{
			if (content.StartsWith ("<")) {	
				Message msg = new Message ();
				msg.parse (content);
				
				if (msg ["type"] == "-1")
					switch (msg ["content"]) {
						case "regiser":
						case "keep-alive":
							if (msg["hostname"] != "")
                                RegisterClient (msg["hostname"]+":"+msg["myPort"], msg ["myAddress"], msg ["myPort"], msg ["subscription"]);
                            else RegisterClient (msg ["myAddress"], msg ["myPort"], msg ["subscription"]);
							break;
						case "unregister":
							UnregisterClient (msg ["myAddress"] +":"+ msg ["myPort"]);
							break;
					}
				//else 
				// Console.WriteLine ("XML dump: {0}", content);
				//Console.WriteLine (string.Format ("New message receivd: {0}", msg ["content"]));
			} else
				Console.WriteLine (string.Format ("New text message: {0}", content));	
		}
		
		// Get list of all registered machines
		public string[] list_machines ()
		{
            if (machines.Keys.Length ==0) return null;

			return machines.Keys;
		}
		
		// Get list of all registered machines with specified subscription group
		public string[] list_machines (string grp)
		{
			List<string > keys = new List<string> ();
			
			foreach (string item in list_machines()) {
				if (machines [item] ["grp"] == grp) {
					keys.Add (item);
				}
			}
            if (keys.Count < 1) return null;
            else
            {
                string[] machinesArray = keys.ToArray();
                return machinesArray;
            }
		}
		
		public string[] list_groups ()
		{
			List<string> groups = new List<string> ();
			
			foreach (string item in list_machines()) { 
				if (!groups.Contains (machines [item] ["grp"]))
					groups.Add (machines [item] ["grp"]);
			}
			
            		if (groups.Count == 0) return null;

			string[] groupsArray = groups.ToArray ();
			return groupsArray;
		}

        public void RegisterClient(string addr, string port, string grp)
        {
            string id = addr +":"+ port;
            RegisterClient(id, addr, port, grp);
        }
		public void RegisterClient (string id, string addr, string port, string grp)
		{
			//string id = addr +":"+ port;
			machines [id] ["address"] = addr;
			machines [id] ["port"] = port;
			machines [id] ["grp"] = grp;
			machines [id] ["lastseen"] = DateTime.Now.ToString ("MM/dd/yyyy hh:mm:sszzz");
			machines.TriggerSave ();
			//machines.Save ();
		}
		
		public void UnregisterClient (string client)
		{
			Console.WriteLine ("unregister " + client);
			machines.unset (client);
			machines.TriggerSave ();
		}
		
		public static void __Main (string[] args)
		{
			NotificationServer server = new NotificationServer ("server.ini");
			server.StartListener ();
			//server.TriggerAutoPurge();
			server.config.TriggerSave ();
		}
	}
}