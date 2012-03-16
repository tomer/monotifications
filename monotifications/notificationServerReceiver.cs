using System;
using monotifications;

namespace notificationConsoleServer
{
	public class notificationServerReceiver : monotifications.notificationClient
	{
		protected Configuration machines;
		
		public notificationServerReceiver () : this("server.ini")
		{
		}
		
		public notificationServerReceiver (string serverINI) : this(serverINI, 7778)
		{
		}
		
		public notificationServerReceiver (string serverINI, int listenPort) : base(serverINI, listenPort)
		{
		}
		
		public notificationServerReceiver (string serverINI, int listenPort, string machinesINI)
		{
			network.setReceiveAction (receiver);
			machines = new Configuration (machinesINI);

		}				

/*		public notificationServerReceiver ()
		{
			config = new monotifications.configuration ("server.ini");
			network.listenPort = int.Parse (config ["server"] ["port"]);			
			network.setReceiveAction (receiver);			
		}*/
		
		
		
		public void receiver (string content)
		{
			if (content.StartsWith ("<")) {	
				Message msg = new Message ();
				msg.parse (content);
				
				if (msg ["type"] == "-1")
					switch (msg ["content"]) {
					case "regiser":
					case "keep-alive":
						registerClient (msg ["myAddress"], msg ["myPort"], msg ["subscription"]);
						break;
					}
				Console.WriteLine ("XML dump: {0}", content);
				//Console.WriteLine (string.Format ("New message receivd: {0}", msg ["content"]));
			} else
				Console.WriteLine (string.Format ("New text message: {0}", content));	
		}
		
		public void registerClient (string addr, string port, string grp)
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
			notificationServerReceiver server = new notificationServerReceiver ("server.ini", 7778);
			server.startListener ();
			server.config.TriggerSave();
						
		}
	}
}