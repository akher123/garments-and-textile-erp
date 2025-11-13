using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCERP.Message.Service
{
    public partial class ServiceForm : Form
    {
        delegate void SetServiceText(string text);
        public ServiceForm()
        {
            InitializeComponent();
        }

        private void btnStartCallBackService_Click(object sender, EventArgs e)
        {
            var endPoints = ServiceContainer.Open();
            foreach (var endpoint in endPoints)
            {
                //txtService.Text += string.Format("{0}{1}", endpoint, Environment.NewLine);
                this.SetText(string.Format("{0}{1}", endpoint, Environment.NewLine));
            }
        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtService.InvokeRequired)
            {
                SetServiceText d = new SetServiceText(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtService.Text = text;
            }
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            ServiceContainer.Close();
        }
    }
}
