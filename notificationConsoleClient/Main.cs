using System;
using monotifications;
using System.Threading;


namespace notificationConsoleClient
{
	class notificationConsoleClient
	{
		monotifications.networking network = new monotifications.networking ();
		monotifications.configuration config = new monotifications.configuration ("client.ini");

		
		public notificationConsoleClient ()
		{
						
			//this.config ["client"] ["recipientPort"] = "7777";
		}
		
		public void startListener ()
		{
			network.listenPort = int.Parse (config ["client"] ["recipientPort"]);
			
			network.setReceiveAction (msgNotify);
			
			
			ThreadStart job = new ThreadStart (network.listen);
			Thread thread = new Thread (job);
			thread.Start ();
			
		}
		
		private void msgNotify (string content)
		{
			if (content.StartsWith ("<")) {	
				Message msg = new Message ();
				msg.parse (content);
			
				Console.WriteLine (string.Format("New message receivd: {0}", msg ["content"]));
			}
			else 	Console.WriteLine (string.Format("New text message: {0}", content));
		}
		
		~notificationConsoleClient ()
		{
			shutdown();
		}
		
		public void shutdown ()
		{
			Console.WriteLine ("Shutting down");
			this.network.stopListener();
			this.config.save ();
		}
		
		public void console ()
		{
			string cmd = "";
			while (cmd != "exit") {
				Console.Write ("> ");
				cmd = Console.ReadLine ();
				
				switch (cmd) {
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
					
					network.talker (destIP, int.Parse (port), content);
					
					break;
				case "":
					break;
				case "?":
				case "help":
					Console.WriteLine ("type \"send\" to send a message.");
					Console.WriteLine ("Type \"exit\" to shutdown application.");
					break;
				default:
					Console.WriteLine("Invalid command. Type \"?\" for help.");
					break;
				}
				
			}
		}
		public void nop(){}
		
		public static void Main (string[] args)
		{
			notificationConsoleClient client = new notificationConsoleClient ();
			
			client.startListener ();
			client.console ();
		}
	}
}