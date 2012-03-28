using System;
using monotifications;
using System.Collections.Generic;
using System.Threading;

namespace monotifications
{
	public class NotificationServer : monotifications.notificationClient
	{
		protected Configuration machines;
		
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

/*		public notificationServerReceiver ()
		{
			config = new monotifications.configuration ("server.ini");
			network.listenPort = int.Parse (config ["server"] ["port"]);			
			network.setReceiveAction (receiver);			
		}*/
		
		protected void PurgeMachines(object state) {
			PurgeMachines();
		}
		
		protected void PurgeMachines ()
		{
			DateTime minimum = DateTime.Now;
			minimum = minimum.Subtract (TimeSpan.FromSeconds (updateInterval * 5));
			foreach (string key in list_machines()) {
				DateTime lastseen = DateTime.Parse (machines [key] ["lastseen"]);
				//if (lastseen < DateTime.Now )
				//Console.WriteLine ("{0} {1} {2}", key, lastseen, DateTime.Compare (lastseen, minimum));
				if (DateTime.Compare (lastseen, minimum) == -1)
					UnregisterClient (key);
			}
		}
		
		private Timer autoPurgeScheduler;
		public void TriggerAutoPurge ()
		{
			autoPurgeScheduler = new Timer (PurgeMachines, null, 1000 * updateInterval, 1000 * updateInterval);
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
							RegisterClient (msg ["myAddress"], msg ["myPort"], msg ["subscription"]);
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
		protected string[] list_machines ()
		{
			return machines.Keys;
		}
		
		// Get list of all registered machines with specified subscription group
		protected string[] list_machines (string grp)
		{
			List<string > keys = new List<string> ();
			
			foreach (string item in list_machines()) {
				if (machines [item] ["grp"] == grp) {
					keys.Add (item);
				}
			}
			string[] machinesArray = keys.ToArray ();
			return machinesArray;
		}
		
		protected string[] list_groups ()
		{
			List<string > groups = new List<string> ();
			
			foreach (string item in list_machines()) { 
				if (!groups.Contains (machines [item] ["grp"]))
					groups.Add (machines [item] ["grp"]);
			}
			
			string[] groupsArray = groups.ToArray ();
			return groupsArray;
		}
		
		public void RegisterClient (string addr, string port, string grp)
		{
			string id = addr +":"+ port;
			machines [id] ["address"] = addr;
			machines [id] ["port"] = port;
			machines [id] ["grp"] = grp;
			machines [id] ["lastseen"] = DateTime.Now.ToString ();
			//machines.TriggerSave ();
			machines.Save ();
		}
		
		public void UnregisterClient(string client) {
			machines.unset(client);
		}
		
		public static void __Main (string[] args)
		{
			NotificationServer server = new NotificationServer ("server.ini");
			server.StartListener ();
			server.TriggerAutoPurge();
			server.config.TriggerSave ();
		}
	}
}