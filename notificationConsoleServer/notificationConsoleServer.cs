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
		
		public void ConsoleSingleSend ()
		{
			string destIP;
			string port;
			string text;
			Console.Write ("Destination IP: ");
			destIP = Console.ReadLine ();
			Console.Write ("Destination port: ");
			port = Console.ReadLine ();					
			Console.Write ("Content: ");
			text = Console.ReadLine ();
					
			Message m = new Message ();
			m ["content"] = text;
			m ["type"] = "1";
					
			network.talker (destIP, int.Parse (port), m.ToString ());
		}
		
		public void ConsoleGroupSend ()
		{
			Console.WriteLine ("Available groups:");
			foreach (string row in list_groups())
				Console.WriteLine ("\t" + row);
			Console.Write ("Destination group: ");
			string destGroup = Console.ReadLine ();
			Console.Write ("Content: ");
			string content = Console.ReadLine ();
			
			Message m = new Message ();
			m ["content"] = content;
			m ["type"] = "1";
			
			foreach (string item in list_machines(destGroup)) {
				Console.WriteLine ("Sending message to {0}...", item);
				network.talker (machines [item] ["address"], 
					int.Parse (machines [item] ["port"]), 
					m.ToString ());
			}			
		}
		
		public void ConsoleServer ()
		{
			Console.WriteLine ("Ready on {0}:{1}.", Address, ListenPort);
			
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
						ConsoleSingleSend ();					
						break;
					case "group send":
						ConsoleGroupSend ();
						
						break;
						
					case "machines":
						foreach (string row in list_machines())
							Console.WriteLine ("\t" + row);
						break;
					case "groups":
						foreach (string row in list_groups())
							Console.WriteLine ("\t" + row);
						break;
					case "purge":
						PurgeMachines ();
						break;
					case "save": 
						this.config.Save ();
						this.machines.Save ();
						break;
					
					case "":
						break;
					case "?":
					case "help":
						Console.WriteLine (@"Console server Help

Available commands:
	machines – List machines
	groups – List groups
	send – Send a message to a single machine
	group send – Send a message to a group
	exit – shut down server console
");
						break;
					default:
						Console.WriteLine ("Invalid command. Type \"?\" for help.");
						break;
				}
				
			}
		}

		public static void Main (string[] args)
		{
			notificationConsoleServer server = new notificationConsoleServer ("server.ini", 0);
			server.StartListener ();
			server.TriggerAutoPurge ();
			server.ConsoleServer ();
		}
	}
}
