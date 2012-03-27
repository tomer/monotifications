using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUInotificationClient
{
    public class SysTrayApp : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new SysTrayApp());
        }

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public SysTrayApp()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Client status", onStatus);
            trayMenu.MenuItems.Add("Test", onSelfTest);
            trayMenu.MenuItems.Add("Quit", OnQuit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Notification Client";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;


            start_notification_client();


        }

        GUI_notification_client client;

        private void start_notification_client()
        {
            client = new GUI_notification_client("client.ini");
            client.StartListener();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void onStatus(object sender, EventArgs e)
        {
            string content = "Client address: " + client.address + ":" + client.listenPort + "\nServer address: " + client.serverAddress + ":" + client.serverPort;
            System.Windows.Forms.MessageBox.Show(content, "Notification status");
        }


        private void onSelfTest(object sender, EventArgs e)
        {
            monotifications.Message msg = new monotifications.Message();
            msg["content"] = "This is a self test";
            msg["title"] = "SELF TEST";
            msg["type"] = "information";

            client.network.talker(client.address, client.listenPort, msg.ToString());
        }
        private void OnQuit(object sender, EventArgs e)
        {
            DialogResult result;
            result = System.Windows.Forms.MessageBox.Show("You won't receive any further notifications. Are you sure you want to quit?", "Confirm exit", MessageBoxButtons.YesNo);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                client.shutdown();
                Application.Exit();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SysTrayApp
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "SysTrayApp";
            this.Load += new System.EventHandler(this.SysTrayApp_Load);
            this.ResumeLayout(false);

        }

        private void SysTrayApp_Load(object sender, EventArgs e)
        {

        }
    }
}
