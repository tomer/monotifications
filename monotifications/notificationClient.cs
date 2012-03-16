// Standard header for Monotification project
// 
using System;
using System.Threading;

namespace monotifications
{
	public class notificationClient
	{
		public monotifications.networking network;
		public monotifications.configuration config;// = new monotifications.configuration ("client.ini");

/*		public notificationClient ()
		{
//			monotifications.configuration config = new monotifications.configuration ("client.ini");
//			network.listenPort = int.Parse (config ["client"] ["recipientPort"]);			
//			this.notificationClient("client.ini");
		}*/
		
		public notificationClient() : this("client.ini") {}
		
		public notificationClient (string clientINI)
		{
			//monotifications.configuration config = new monotifications.configuration (clientINI);
			
			config = new configuration (clientINI);			
			network = new monotifications.networking ();
			network.listenPort = int.Parse (config ["client"] ["recipientPort"]);
			
			/*if (verifyConfig()) {
				network.listenPort = int.Parse (config ["client"] ["recipientPort"]);			
			}
			else 
				Console.WriteLine("Configuration file mismatch!");//*/
		}
		
		
		
		public bool verifyConfig ()
		{
			string[] items = new string [2] {"recipientPort", "recipientAddress"};
			
			foreach (string item in items) {
				//Console.WriteLine("{0}", item);
				//Console.WriteLine ("config [client] [{0}] = ", item, config ["client"] [item]);
				if (config ["client"] [item]== null || config ["client"] [item] == "")
					return false;
			}
			return true;
		}
		
		public void shutdown ()
		{
			Console.WriteLine ("Shutting down");
			this.network.stopListener ();
			this.config.save ();
		}

		public void startListener ()
		{			
			ThreadStart job = new ThreadStart (network.listen);
			Thread thread = new Thread (job);
			thread.Start ();
			//registerOnServer ();	
		}
	}

}

