using System;
using monotifications;

namespace notificationConsoleServer
{
	public class notificationServerReceiver : monotifications.notificationClient
	{
		public notificationServerReceiver ()
		{
			monotifications.configuration config = new monotifications.configuration ("server.ini");
			network.listenPort = int.Parse (config ["server"] ["port"]);			
			network.setReceiveAction (receiver);			
		}
		
		
		
		public void receiver (string content)
		{
			if (content.StartsWith ("<")) {	
				Message msg = new Message ();
				msg.parse (content);
				
				if (msg ["type"] == "-1")
					switch (msg ["content"]) {
					case "regiser":
						registerClient(msg["recipientAddress"],msg["recipientPort"], msg["subscription"]);
						break;
					}
				//Console.WriteLine ("XML dump: {0}", content);
				//Console.WriteLine (string.Format ("New message receivd: {0}", msg ["content"]));
			} else
				Console.WriteLine (string.Format ("New text message: {0}", content));
			
		}
		
		public void registerClient (string addr, string port, string grp)
		{
			//TBD
		}
		
		public static void Main (string[] args)
		{
			notificationServerReceiver server = new notificationServerReceiver ();
		}
	}
}

