using System;
using System.Windows.Forms;
using System.Drawing;

namespace notificationClient
{
    class SysTrayApp : Form
    {
        private NotifyIcon trayIcon;
        private TextBox txtAddress;
        private Label lblAddress;
        private Label lblPort;
        private TextBox textBox1;
        private Label lblServerAddress;
        private TextBox textBox2;
        private Label lblServerPort;
        private TextBox textBox3;  
        private ContextMenu trayMenu;  

    
                 [STAThread]  

         public static void __Main()  
         {  
             Application.Run(new SysTrayApp());
         }  


         public SysTrayApp()  
         {  

             // Create a simple tray menu with only one item.  
             trayMenu = new ContextMenu();
             //trayMenu.MenuItems.Add("Show status", onShowStatus);
             trayMenu.MenuItems.Add("Quit", OnExit);

    

             // Create a tray icon. In this example we use a  
             // standard system icon for simplicity, but you  
             // can of course use your own custom icon too.  
             trayIcon      = new NotifyIcon();  
             trayIcon.Text = "Notification Client";  
             trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);  //TODO: Replace the generic icon with something more meaningful

    

             // Add menu to tray icon and show it.  
             trayIcon.ContextMenu = trayMenu;  
             trayIcon.Visible     = true;


             //notificationGUIClient client;
             //client = new notificationGUIClient("client.ini");

             //client.StartListener();

         }  

        protected override void OnLoad(EventArgs e)  
         {  
             Visible       = false; // Hide form window.  
             ShowInTaskbar = false; // Remove from taskbar.     

             base.OnLoad(e);  
         }  

         private void OnExit(object sender, EventArgs e)  
         {
             //System.Windows.Forms.MessageBox.Show("Hello");
             /*System.Windows.Forms.ques
             Application.Exit();  */

             if (MessageBox.Show("You won't receive any further messages if shutting down.\nAre you sure you want to quit?", "Confirm exit", MessageBoxButtons.YesNo) == DialogResult.Yes)
             {
                 Application.Exit();
                 // a 'DialogResult.Yes' value was returned from the MessageBox
                 // proceed with your deletion
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
             this.txtAddress = new System.Windows.Forms.TextBox();
             this.lblAddress = new System.Windows.Forms.Label();
             this.lblPort = new System.Windows.Forms.Label();
             this.textBox1 = new System.Windows.Forms.TextBox();
             this.lblServerAddress = new System.Windows.Forms.Label();
             this.textBox2 = new System.Windows.Forms.TextBox();
             this.lblServerPort = new System.Windows.Forms.Label();
             this.textBox3 = new System.Windows.Forms.TextBox();
             this.SuspendLayout();
             // 
             // txtAddress
             // 
             this.txtAddress.Location = new System.Drawing.Point(80, 12);
             this.txtAddress.Name = "txtAddress";
             this.txtAddress.ReadOnly = true;
             this.txtAddress.Size = new System.Drawing.Size(100, 20);
             this.txtAddress.TabIndex = 0;
             // 
             // lblAddress
             // 
             this.lblAddress.AutoSize = true;
             this.lblAddress.Location = new System.Drawing.Point(13, 13);
             this.lblAddress.Name = "lblAddress";
             this.lblAddress.Size = new System.Drawing.Size(61, 13);
             this.lblAddress.TabIndex = 1;
             this.lblAddress.Text = "IP Address:";
             // 
             // lblPort
             // 
             this.lblPort.AutoSize = true;
             this.lblPort.Location = new System.Drawing.Point(13, 39);
             this.lblPort.Name = "lblPort";
             this.lblPort.Size = new System.Drawing.Size(60, 13);
             this.lblPort.TabIndex = 3;
             this.lblPort.Text = "Listen Port:";
             // 
             // textBox1
             // 
             this.textBox1.Location = new System.Drawing.Point(80, 38);
             this.textBox1.Name = "textBox1";
             this.textBox1.ReadOnly = true;
             this.textBox1.Size = new System.Drawing.Size(100, 20);
             this.textBox1.TabIndex = 2;
             // 
             // lblServerAddress
             // 
             this.lblServerAddress.AutoSize = true;
             this.lblServerAddress.Location = new System.Drawing.Point(13, 65);
             this.lblServerAddress.Name = "lblServerAddress";
             this.lblServerAddress.Size = new System.Drawing.Size(82, 13);
             this.lblServerAddress.TabIndex = 5;
             this.lblServerAddress.Text = "Server Address:";
             // 
             // textBox2
             // 
             this.textBox2.Location = new System.Drawing.Point(101, 64);
             this.textBox2.Name = "textBox2";
             this.textBox2.ReadOnly = true;
             this.textBox2.Size = new System.Drawing.Size(100, 20);
             this.textBox2.TabIndex = 4;
             // 
             // lblServerPort
             // 
             this.lblServerPort.AutoSize = true;
             this.lblServerPort.Location = new System.Drawing.Point(13, 91);
             this.lblServerPort.Name = "lblServerPort";
             this.lblServerPort.Size = new System.Drawing.Size(63, 13);
             this.lblServerPort.TabIndex = 7;
             this.lblServerPort.Text = "Server Port:";
             // 
             // textBox3
             // 
             this.textBox3.Location = new System.Drawing.Point(101, 91);
             this.textBox3.Name = "textBox3";
             this.textBox3.ReadOnly = true;
             this.textBox3.Size = new System.Drawing.Size(100, 20);
             this.textBox3.TabIndex = 6;
             // 
             // SysTrayApp
             // 
             this.ClientSize = new System.Drawing.Size(292, 273);
             this.Controls.Add(this.lblServerPort);
             this.Controls.Add(this.textBox3);
             this.Controls.Add(this.lblServerAddress);
             this.Controls.Add(this.textBox2);
             this.Controls.Add(this.lblPort);
             this.Controls.Add(this.textBox1);
             this.Controls.Add(this.lblAddress);
             this.Controls.Add(this.txtAddress);
             this.Name = "Notification Client";
             this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
             this.Load += new System.EventHandler(this.SysTrayApp_Load);
             this.ResumeLayout(false);
             this.PerformLayout();

         }

         private void SysTrayApp_Load(object sender, EventArgs e)
         {

         }  
    }
}