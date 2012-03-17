using System;
using monotifications;

namespace notificationConsoleServer
{
	class notificationConsoleServer : NotificationServer
	{	
		public notificationConsoleServer () : base("server.ini")
		{
		}
		
		public notificationConsoleServer (string serverINI) : base(serverINI, 7778)
		{
		}
		
		public notificationConsoleServer (string serverINI, int listenPort) : base(serverINI, listenPort)
		{
		}
		

		public void Console ()
		{
			string cmd = "";
			while (cmd != "exit") {
				Console.Write ("> ");
				cmd = Console.ReadLine ();
				
				switch (cmd.ToLower ()) {
				case "quit":
				case "bye":
				case "exit":
					cmd = "exit";
					this.shutdown ();
					break;
				case "send":
					string destIP;
					string port;
					string content;
					Console.Write ("Destination IP: ");
					destIP = Console.ReadLine ();
					Console.Write ("Destination port: ");
					port = Console.ReadLine ();					
					Console.Write ("Content: ");
					content = Console.ReadLine ();
					
					Message m = new Message ();
					m ["content"] = content;
					m ["type"] = "1";
					
					network.talker (destIP, int.Parse (port), m.ToString ());
					
					break;
				case "save": 
					this.config.Save ();
					break;
					
				case "":
					break;
				case "?":
				case "help":
					Console.WriteLine ("type \"send\" to send a message.");
					Console.WriteLine ("Type \"exit\" to shutdown application.");
					break;
				default:
					Console.WriteLine ("Invalid command. Type \"?\" for help.");
					break;
				}
				
			}
		}

		public static void Main (string[] args)
		{
			notificationConsoleServer server = new notificationConsoleServer ("server.ini", 7778);
			server.startListener ();
			server.config.TriggerSave ();
			server.console ();
		}
	}
}
