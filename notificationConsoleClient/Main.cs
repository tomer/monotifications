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
						
			this.config ["client"] ["recipientPort"] = "7777";
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
			if (content [0] == '<')
				Console.WriteLine ("New text message: {0}, content");
			else {			
				Message msg = new Message ();
				msg.parse (content);
			
				Console.WriteLine ("New message receivd: {0}", msg ["content"]);
			}
		}
		
		~notificationConsoleClient ()
		{
			shutdown();
		}
		
		public void shutdown ()
		{
			Console.WriteLine ("Shutting down");
			this.config.save ();
		}
		
		public void console ()
		{
			string cmd = "";
			while (cmd != "exit") {
				Console.Write ("> ");
				cmd = Console.ReadLine();
				
				switch (cmd) {
					case "exit": shutdown(); break;
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