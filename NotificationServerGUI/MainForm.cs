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
            server.TriggerAutoPurge();
            refreshGroupsAndComputers();
        }

        private void refreshGroupsAndComputers()
        {
            lstGroups.Items.Clear();
            foreach (string row in server.list_groups())
                lstGroups.Items.Add(row);

            lstComputers.Items.Clear();
            foreach (string row in server.list_machines())
                lstComputers.Items.Add(row);
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
            string icon = cmbIcon.SelectedValue.ToString();

            /*foreach (string item in lstComputers.SelectedItems)
            {
                //sendMessage(server.machines[item]["recipientAddress"], int.Parse(server.machines[item]["recipientPort"]), txtContent.Text, txtTitle.Text, cmbIcon.SelectedValue.ToString);
                sendMessage(server.machines[item]["recipientAddress"], int.Parse(server.machines[item]["recipientPort"]), content, title, icon);
            }*/

            List<string> recipients = new List<string>();

            foreach (string key in lstComputers.SelectedItems) recipients.Add(key);
            foreach (string grp in lstGroups.SelectedItems)
                foreach (string key in server.list_machines(grp)) 
                    recipients.Add(key);

            SendMessage(recipients, content, title, icon);

        }

        private void SendMessage(List<string> list, string content, string title, string icon) {
            string messagebox = "Message will be sent to the following destinations:\n ";
            foreach (string item in list) messagebox += item + "\n";
            MessageBox.Show(messagebox);
            
            foreach (string item in list)
            {
                sendMessage(server.machines[item]["recipientAddress"], int.Parse(server.machines[item]["recipientPort"]), content, title, icon);
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

        private void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGroups.SelectedItems.Count == 0)                
                lstComputers.Enabled = true;
            else
                lstComputers.Enabled = false;
        }

        private void lstComputers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstComputers.SelectedItems.Count == 0)
                lstGroups.Enabled = true;
            else
                lstGroups.Enabled = false;
        }
    }
}
