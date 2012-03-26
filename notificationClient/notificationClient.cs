using System;
using monotifications;


namespace notificationClient
{
    class notificationGUIClient : monotifications.notificationClient
	{		
		public notificationGUIClient () : this("client.ini")
		{
		}
		
		public notificationGUIClient (string clientINI) : base(clientINI)
		{
			Console.WriteLine ("Ready.");
			network.setReceiveAction (MsgNotify);
		}
		
		public notificationGUIClient (string clientINI, int listenPort) : base(clientINI, listenPort)
		{
			Console.WriteLine ("Ready.");
			network.setReceiveAction (MsgNotify);
		}
		
		/*public void RegisterOnServer ()
		{
			if (config ["server"] ["serverAddress"] != "" && config ["server"] ["serverPort"] != "") {
			/*	Console.WriteLine ("Registering on {0}", config ["server"] ["serverAddress"]);
				registerOnServer (config ["server"] ["serverAddress"],
					config ["client"] ["recipientAddress"], 
					config ["client"] ["recipientPort"], 
					config ["client"] ["subscription"]);* /
				base.registerOnServer();
			} else
				Console.WriteLine ("No server defined");
		}*/
		
		private void MsgNotify (string content)
		{
			if (content.StartsWith ("<")) {	
				Message msg = new Message ();
				msg.parse (content);
				
				Console.WriteLine ("XML dump: {0}", content);
				Console.WriteLine (string.Format ("New message receivd: {0}", msg ["content"]));
			} else
				Console.WriteLine (string.Format ("New text message: {0}", content));
		}
		
		/*public void ConsoleSingleSend ()
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
		}*/
		
		/*public void ConsoleClient ()
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
					case "save": 
						this.config.Save ();
						break;
					
					case "":
						break;
					case "?":
					case "help":
						Console.WriteLine (@"Console client Help

Available commands:
	send – Send a message to a single machine
	exit – shut down server console
");
						break;
					default:
						Console.WriteLine ("Invalid command. Type \"?\" for help.");
						break;
				}
				
			}
		}*/
		
		public static void Main (string[] args)
		{
			notificationGUIClient client;// = new notificationConsoleClient ("client.ini");
			
			if (args.Length == 1)
				client = new notificationGUIClient ("client.ini", int.Parse(args [0]));
			else
				client = new notificationGUIClient ("client.ini");
			
			client.StartListener ();
			client.RegisterOnServer();
			//client.ConsoleClient ();
		}
	}
}