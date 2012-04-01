// Standard header for Monotification project
// 
using System;
using System.Threading;
using System.Net;

namespace monotifications
{
	public class notificationClient
	{
		public monotifications.networking network;
		public monotifications.Configuration config;// = new monotifications.configuration ("client.ini");
		
		public string serverAddress, address, grp, hostname; // These fields would be set only when registering
		public int serverPort, listenPort;
	    
		protected int updateInterval = 120;
		
		public notificationClient () : this("client.ini")
		{
		}
		
		public notificationClient (string clientINI) : this (clientINI, -1)
		{
		}
		
		public notificationClient (string clientINI, int listenPort)
		{	
			config = new Configuration (clientINI);			
			network = new monotifications.networking ();
			
			if (listenPort < 1 && config ["client"] ["recipientPort"] != null)
				listenPort = int.Parse (config ["client"] ["recipientPort"]);
			
			int serverPort = 0;
			if (config ["server"] ["serverPort"] != null && config ["server"] ["serverPort"] != "")
				serverPort = int.Parse (config ["server"] ["serverPort"]);
			else
				serverPort = 7778;
			
			string address;
			if (config ["client"] ["recipientAddress"] == null || config ["client"] ["recipientAddress"] == "0.0.0.0") 
				address = ip.AddressList [0].ToString ();
			else address = config ["client"] ["recipientAddress"];
			
			setProperties (address,
				listenPort,
				config ["client"] ["subscription"],
				config ["server"] ["serverAddress"],
				serverPort);
		}
		
		protected void setProperties (string address, int listenPort, string grp, string serverAddress, int serverPort)
		{
			if (listenPort > 0)
				this.ListenPort = listenPort;//int.Parse (config ["client"] ["recipientPort"]);
			else {
				Random rnd = new Random ();
				this.ListenPort = rnd.Next (7700, 7799);
			}
			
			this.address = address; //config ["client"] ["recipientAddress"];
			if (address == null || address == "" || address == "0.0.0.0" || address == "127.0.0.1") {
				address = ip.AddressList [0].ToString ();
			}
			
			this.Grp = grp;//config ["client"] ["grp"];

			this.ServerAddress = serverAddress;//config ["client"] ["serverAddress"];
			this.ServerPort = serverPort;//int.Parse (config ["client"] ["serverPort"]);
			
		}
		public IPHostEntry ip {
			get {
				string host = Dns.GetHostName ();
				IPHostEntry ip = Dns.GetHostEntry (host);
				//Console.WriteLine (ip.AddressList [0].ToString ());
				return ip;
			}
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
			this.unregisterOnServer();
			this.network.stopListener ();
			this.config.Save ();
		}
		
		protected void registerOnServer ()
		{
			Message m = new Message ();
			m ["content"] = "register";
			m ["type"] = "-1"; // Internal commands channel
			m ["myPort"] = listenPort.ToString();
			m ["myAddress"] = address;
            		m["hostname"] = hostname = Dns.GetHostName();
			m ["subscription"] = grp; ///
			network.talker (this.serverAddress, this.serverPort, m.ToString ());
			TriggerKeepAlive ();
		}
		
		~notificationClient() {
			keepaliveScheduler.Change(Timeout.Infinite, Timeout.Infinite);
			unregisterOnServer();
		}

		protected string Address {
			get {
				return this.address;
			}
			set {
				address = value;
			}
		}

		protected string Grp {
			get {
				return this.grp;
			}
			set {
				grp = value;
			}
		}

		protected int ListenPort {
			get {
				return this.listenPort;
			}
			set {
				listenPort = value;
				network.listenPort = listenPort;
			}
		}

		protected string ServerAddress {
			get {
				return this.serverAddress;
			}
			set {
				serverAddress = value;
			}
		}

		protected int ServerPort {
			get {
				return this.serverPort;
			}
			set {
				serverPort = value;
			}
		}
		
		protected void unregisterOnServer ()
		{
			if (this.serverAddress != "" && this.serverAddress != null) {
				Message m = new Message ();
				m ["content"] = "unregister";
				m ["type"] = "-1"; // Internal commands channel
				m ["myPort"] = this.listenPort.ToString(); //config ["client"] ["recipientPort"];
				m ["myAddress"] = this.address; // config ["client"] ["recipientAddress"];
				m ["subscription"] = this.grp; //config ["client"] ["subscription"];
				m["hostname"] = this.hostname;
				network.talker (this.serverAddress, this.serverPort, m.ToString ());
				this.serverAddress = null;
			}
		}

		private Timer keepaliveScheduler;
		public void TriggerKeepAlive ()
		{
			keepaliveScheduler = new Timer (KeepAliveTrigger, null, updateInterval, 1000 * updateInterval);
		}
		
		
		
		private void KeepAliveTrigger (object state)
		{
			Message m = new Message ();
			m ["content"] = "keep-alive";
			m ["type"] = "-1"; // Internal commands channel
			m ["myPort"] = listenPort.ToString(); //config ["client"] ["recipientPort"];
			m ["myAddress"] = address; // config ["client"] ["recipientAddress"];
			m ["subscription"] =  grp; //config ["client"] ["subscription"];
			network.talker (serverAddress, serverPort, m.ToString ());
		}
		public void StartListener ()
		{			
			ThreadStart job = new ThreadStart (network.listen);
			Thread thread = new Thread (job);
			thread.Start ();
			registerOnServer ();	
		}
	}

}

