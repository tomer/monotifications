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

            NotificationServerGUI server = new NotificationServerGUI("server.ini", 0);
            server.StartListener();
            server.TriggerAutoPurge();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }
    }
}
