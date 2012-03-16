using System;

namespace notificationConsoleServer
{
	class notificationConsoleServer
	{
		private monotifications.notificationClient client = new notificationServerReceiver();
		
		public void startListener ()
		{
			
			client.startListener ();	
		}
		
		public static void _Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
		}
	}
}
