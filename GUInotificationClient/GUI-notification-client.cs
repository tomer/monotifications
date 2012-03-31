using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using monotifications;
using System.Windows.Forms;


namespace GUInotificationClient
{
    class GUI_notification_client : notificationClient
    {		
		public GUI_notification_client () : this("client.ini")
		{
		}
		
		public GUI_notification_client (string clientINI) : base(clientINI)
		{
			//Console.WriteLine ("Ready.");
			network.setReceiveAction (MsgNotify);
		}

        public GUI_notification_client (string clientINI, int listenPort)
            : base(clientINI, listenPort)
		{
			Console.WriteLine ("Ready.");
			network.setReceiveAction (MsgNotify);
		}
		
		private void MsgNotify (string content)
		{
			if (content.StartsWith ("<")) {
                monotifications.Message msg = new monotifications.Message();
				msg.parse (content);

                MessageBoxIcon icon = MessageBoxIcon.None;

                switch (msg["type"].ToLower()) {
                    case "asterisk":    icon = MessageBoxIcon.Asterisk; break;
                    case "error":       icon = MessageBoxIcon.Error; break;
                    case "exclamation": icon = MessageBoxIcon.Exclamation; break;
                    case "hand":        icon = MessageBoxIcon.Hand; break;
                    case "information": icon = MessageBoxIcon.Information; break;
                    case "question":    icon = MessageBoxIcon.Question; break;
                    case "stop":        icon = MessageBoxIcon.Stop; break;
                    case "warning":     icon = MessageBoxIcon.Warning; break;
                    case "none": 
                    default: icon = MessageBoxIcon.None; break;
                }

                string title = "Notification!";
                if (msg["title"] != "") title = msg["title"];

                System.Windows.Forms.MessageBox.Show(new Form() { TopMost = true }, msg["content"], title, MessageBoxButtons.OK, icon);

			} else
                System.Windows.Forms.MessageBox.Show(content, "Generic notification", MessageBoxButtons.OK);
		}
		
		public static void __Main (string[] args)
		{
            GUI_notification_client client;
			
			
			
			if (args.Length == 1)
                client = new GUI_notification_client("client.ini", int.Parse(args[0]));
			else
                client = new GUI_notification_client("client.ini");
			
			client.StartListener ();
		}
	}
}