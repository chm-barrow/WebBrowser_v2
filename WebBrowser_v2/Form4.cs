using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebBrowser_v2
{
    public delegate void EventFavoriteData(string data);
    public partial class Form4 : Form
    {
        public event EventFavoriteData ChildEvent;
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( String.IsNullOrEmpty(this.textBox1.Text) )
            {
                this.maskedTextBox1.Text = "The name can not be empty.";
            }
            else if ( !(this.textBox2.Text.StartsWith("http://") || this.textBox2.Text.StartsWith("https://")) )
            {
                this.maskedTextBox1.Text = "The entered URL is invalid.";
            }
            else
            {
                this.ChildEvent(this.textBox1.Text + ";" + this.textBox2.Text);
                this.Hide();

            }

        }
    }
}
