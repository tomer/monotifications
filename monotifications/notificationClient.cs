// Standard header for Monotification project
// 
using System;
using System.Threading;

namespace monotifications
{
	public class notificationClient
	{
		public monotifications.networking network;
		public monotifications.Configuration config;// = new monotifications.configuration ("client.ini");
		
		private string serverAddress, address, port, grp; // These fields would be set only when registering
		private int serverPort;
		
/*		public notificationClient ()
		{
//			monotifications.configuration config = new monotifications.configuration ("client.ini");
//			network.listenPort = int.Parse (config ["client"] ["recipientPort"]);			
//			this.notificationClient("client.ini");
		}*/
		
		public notificationClient () : this("client.ini")
		{
		}
		
		public notificationClient (string clientINI)
		{
			config = new Configuration (clientINI);			
			network = new monotifications.networking ();
			network.listenPort = int.Parse (port);
		}
		
		public notificationClient (string clientINI, int listenPort)
		{	
			config = new Configuration (clientINI);			
			network = new monotifications.networking ();
			network.listenPort = listenPort;
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
			this.config.Save ();
		}
		
		protected void registerOnServer (string serverAddress, string address, string port, string grp)
		{
			Message m = new Message ();
			m ["content"] = "register";
			m ["type"] = "-1"; // Internal commands channel
			m ["myPort"] = this.port = port; //config ["client"] ["recipientPort"];
			m ["myAddress"] = this.address = address; // config ["client"] ["recipientAddress"];
			m ["subscription"] = this.grp = grp; //config ["client"] ["subscription"];
			network.talker (this.serverAddress = serverAddress, this.serverPort = int.Parse (config ["server"] ["serverPort"]), m.ToString ());
			TriggerACK();
		}

		private Timer ACKScheduler;
		public void TriggerACK ()
		{

			ACKScheduler = new Timer (ACKTrigger, null, 1000 * 5, 1000 * 5);
		}
		
		
		
		private void ACKTrigger (object state)
		{
			Message m = new Message ();
			m ["content"] = "keep-alive";
			m ["type"] = "-1"; // Internal commands channel
			m ["myPort"] = port; //config ["client"] ["recipientPort"];
			m ["myAddress"] = address; // config ["client"] ["recipientAddress"];
			m ["subscription"] =  grp; //config ["client"] ["subscription"];
			network.talker (serverAddress, serverPort, m.ToString ());
		}
		public void StartListener ()
		{			
			ThreadStart job = new ThreadStart (network.listen);
			Thread thread = new Thread (job);
			thread.Start ();
			//registerOnServer ();	
		}
	}

}

