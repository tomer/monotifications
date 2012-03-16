using System;
using System.Threading;
using monotifications;


namespace notificationConsoleClient
{
	class notificationConsoleClient : notificationClient
	{		
		public notificationConsoleClient () : this("client.ini")
		{
		}
		
		public notificationConsoleClient(string clientINI) : base(clientINI) {
			Console.WriteLine ("Ready.");
			network.setReceiveAction (msgNotify);
		}
		
		
		/*public void startListener ()
		{			
			ThreadStart job = new ThreadStart (network.listen);
			Thread thread = new Thread (job);
			thread.Start ();
			registerOnServer ();			
		}*/
		
		public void registerOnServer ()
		{
			Message m = new Message ();
			m ["content"] = "register";
			m ["type"] = "-1"; // Internal commands channel
			m ["myPort"] = config ["client"] ["recipientPort"];
			m ["myAddress"] = config ["client"] ["recipientAddress"];
			m ["subscription"] = config ["client"] ["subscription"];			
			
			if (config ["server"] ["serverAddress"] != "" && config ["server"] ["serverPort"] != "") {
				network.talker (config ["server"] ["serverAddress"], int.Parse (config ["client"] ["recipientPort"]), m.ToString ());
				Console.WriteLine ("Registering on {0}", config ["server"] ["serverAddress"]);
			} else
				Console.WriteLine ("No server defined");
		}
		
		private void msgNotify (string content)
		{
			if (content.StartsWith ("<")) {	
				Message msg = new Message ();
				msg.parse (content);
				
				Console.WriteLine ("XML dump: {0}", content);
				Console.WriteLine (string.Format ("New message receivd: {0}", msg ["content"]));
			} else
				Console.WriteLine (string.Format ("New text message: {0}", content));
		}
		
		
/*		~notificationConsoleClient ()
		{
//			shutdown();
		}*/
		
		
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
					
					Message m = new Message ();
					m ["content"] = content;
					m ["type"] = "1";
					
					network.talker (destIP, int.Parse (port), m.ToString ());
					
					break;
				case "save": 
					this.config.save ();
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
			notificationConsoleClient client = new notificationConsoleClient ("client.ini");
			
			client.startListener ();
			client.registerOnServer();
			client.console ();
		}
	}
}