using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace WebBrowser_v2
{
    public delegate void EventHistory(string data);

    public partial class Form3 : Form
    {
        public event EventHistory ChildEvent;
        public Form3()
        {
            InitializeComponent();
            SetPage();
        }

        private void SetPage()
        {
            /* The string in App.Config is in the following format : [url,url,...url] */
            string history = ConfigurationManager.AppSettings["history"];

            string[] history_array = history.Split(',');
            for (int i = history_array.Length-1; i > 0 ; i--)
            {
                if (history_array[i] != null || history_array[i] != "")
                {
                    this.listBox1.Items.Add(history_array[i]);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChildEvent != null) // Check if there are subscribers
            {
                // Raise the event with the selected item as data
                if (this.listBox1.SelectedItem != null)
                {
                    this.ChildEvent(this.listBox1.SelectedItem.ToString());
                    this.Close();
                }
                else
                {
                    // Handle the case where the selected item is null
                }
            }
        }
    }
}
