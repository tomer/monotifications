using System;
using monotifications;
using System.Collections.Generic;

namespace notificationConsoleServer
{
	public class NotificationServer : monotifications.notificationClient
	{
		protected Configuration machines;
		
		public NotificationServer () : this("server.ini")
		{
		}
		
		public NotificationServer (string serverINI) : this(serverINI, 7778)
		{
		}
		
		public NotificationServer (string serverINI, int listenPort) : this(serverINI, listenPort, "machines.ini")
		{
		}
		
		public NotificationServer (string serverINI, int listenPort, string machinesINI) : base(serverINI, listenPort)
		{
			network.setReceiveAction (Receiver);
			machines = new Configuration (machinesINI);

		}

/*		public notificationServerReceiver ()
		{
			config = new monotifications.configuration ("server.ini");
			network.listenPort = int.Parse (config ["server"] ["port"]);			
			network.setReceiveAction (receiver);			
		}*/
		
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
							UnregisterClient (msg ["myAddress"]);
							break;
					}
				//else 
				Console.WriteLine ("XML dump: {0}", content);
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
			machines [addr] ["address"] = addr;
			machines [addr] ["port"] = port;
			machines [addr] ["grp"] = grp;
			machines [addr] ["lastseen"] = DateTime.Now.ToString ();
			//machines.TriggerSave ();
			machines.Save ();
		}
		
		public void UnregisterClient(string client) {
			machines.unset(client);
		}
		
		public static void __Main (string[] args)
		{
			NotificationServer server = new NotificationServer ("server.ini", 7778);
			server.StartListener ();
			server.config.TriggerSave ();
		}
	}
}