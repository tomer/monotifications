using System;
using monotifications;

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
		
		public NotificationServer (string serverINI, int listenPort) : base(serverINI, listenPort)
		{
		}
		
		public NotificationServer (string serverINI, int listenPort, string machinesINI)
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
					}
				Console.WriteLine ("XML dump: {0}", content);
				//Console.WriteLine (string.Format ("New message receivd: {0}", msg ["content"]));
			} else
				Console.WriteLine (string.Format ("New text message: {0}", content));	
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
		
		public static void __Main (string[] args)
		{
			NotificationServer server = new NotificationServer ("server.ini", 7778);
			server.StartListener ();
			server.config.TriggerSave();
						
		}
	}
}