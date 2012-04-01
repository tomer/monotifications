using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using monotifications;

namespace NotificationServerGUI
{

    public partial class MainForm : Form
    {
        NotificationServerGUI server;

        public MainForm()
        {
            InitializeComponent();
            setMessageIcons();

            server = new NotificationServerGUI("server.ini", 0);
            server.StartListener();
            //server.TriggerAutoPurge();
            refreshGroupsAndComputers();
            
            timer1.Interval = 5000;
            timer1.Start();
        }

        private void refreshGroupsAndComputers ()
		{
			lstGroups.Items.Clear ();
			lstComputers.Items.Clear ();
			
			if (server.list_machines() == null) return;
			
			foreach (string item in server.list_machines()) {
				lstComputers.Items.Add (item);
			}
			
			foreach (string item in server.list_groups()) {
				lstGroups.Items.Add (item);
			}
		}

        private void setMessageIcons()
        {
            cmbIcon.Items.Clear();
            cmbIcon.Items.Add("");
            cmbIcon.Items.Add("Error");
            cmbIcon.Items.Add("Exclamation");
            cmbIcon.Items.Add("Hand");
            cmbIcon.Items.Add("Information");
            //cmbIcon.Items.Add("None");
            cmbIcon.Items.Add("Question");
            cmbIcon.Items.Add("Stop");
            cmbIcon.Items.Add("Warning");
        }

        private void deliverMessages()
        {
            string content = txtContent.Text + "";
            string title = txtTitle.Text + "";
            string icon = cmbIcon.SelectedText;

            List<string> recipients = new List<string>();

            foreach (string key in lstComputers.SelectedItems) recipients.Add(key);
            /*foreach (string grp in lstGroups.SelectedItems)
                foreach (string key in server.list_machines(grp)) 
                    recipients.Add(key);*/

            SendMessage(recipients, content, title, icon);

        }

        private void SendMessage(List<string> list, string content, string title, string icon) {
            /*string messagebox = "Message will be sent to the following destinations:\n ";
            foreach (string item in list) messagebox += item + "\n";
            MessageBox.Show(messagebox);*/
            
            foreach (string item in list)
            {
                sendMessage(server.machines[item]["address"], int.Parse(server.machines[item]["port"]), content, title, icon);
            }
        }

        private void sendMessage(string destination, int destinationPort, string message, string title, string icon) {
            monotifications.Message m = new monotifications.Message();
            m["content"] = message;
            m["type"] = icon;
            m["title"] = title;

            server.network.talker(destination, destinationPort, m.ToString());
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    server.shutdown();
                    break;
            }
        }



        private void btnSubmit_Click(object sender, EventArgs e)
        {
            deliverMessages();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void lstGroups_SelectedIndexChanged (object sender, EventArgs e)
		{
			lstComputers.SelectedItems.Clear ();
			
			foreach (string grp in lstGroups.SelectedItems)
				foreach (string item in server.list_machines(grp))
					lstComputers.SelectedItems.Add(item);
			
			//refreshGroupsAndComputers();
		}

        private void lstComputers_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableDisableSubmitButton();
        }

        void enableDisableSubmitButton()
        {
            if ((lstGroups.SelectedItems.Count + lstComputers.SelectedItems.Count) > 0)
                btnSubmit.Enabled = true;
            else btnSubmit.Enabled = false;
        }

        private void btnReset_Click (object sender, EventArgs e)
		{
			txtContent.Text = "";
			txtTitle.Text = "";
			lstComputers.SelectedItems.Clear ();
			lstGroups.SelectedItems.Clear ();
			cmbIcon.SelectedIndex = 0;
		}

        private void timer1_Tick (object sender, EventArgs e)
		{
			if (lstComputers.SelectedItems.Count == 0 && lstGroups.SelectedItems.Count == 0) {
				refreshGroupsAndComputers ();
			}
			server.PurgeMachines ();
		}
    }
}
