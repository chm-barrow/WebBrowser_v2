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

    public delegate void EventFavorite(string data);

    public partial class Form2 : Form
    {

        public event EventFavorite ChildEvent;
        private Dictionary<string, string> dict = new Dictionary<string, string>();
        private bool delete; // False if we access a favorite, true if we remove one.

        public Form2(bool delete)
        {
            InitializeComponent();
            SetPage();
            this.delete = delete;
        }

        private void SetPage()
        {
            /* The value in App.Config is in the following format : [name;url,name;url,...name;url] */
            string favorites = ConfigurationManager.AppSettings["favorites"];

            string [] favorites_array = favorites.Split(',');

            for (int i = 0; i<favorites_array.Length; i++)
            {
                string [] fav_couple = favorites_array[i].Split(';');
                try
                {
                    this.dict.Add(fav_couple[0], fav_couple[1]);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                this.listBox1.Items.Add(fav_couple[0]);

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChildEvent != null) // Check if there are subscribers
            {
                // Raise the event with the selected item as data
                if (this.listBox1.SelectedItem != null)
                {
                    if (this.delete == false)
                    {
                        this.ChildEvent(this.dict[this.listBox1.SelectedItem.ToString()]);
                        this.Close();
                    }
                    else
                    {
                        string favorites = ConfigurationManager.AppSettings["favorites"];
                        string[] favorites_array = favorites.Split(',');

                        string url = dict[this.listBox1.SelectedItem.ToString()];
                        string output = "";
                        for (int i = 0; i<favorites_array.Length; i++)
                        {
                            string [] fav_couple = favorites_array[i].Split(';');
                            if (fav_couple[0] != this.listBox1.SelectedItem.ToString())
                            {
                                output += favorites_array[i];
                            }

                        }


                        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.AppSettings.Settings.Remove("favorites");
                        config.AppSettings.Settings.Add("favorites", output);
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");


                        this.Close();
                    }
                }
                else
                {
                    // Handle the case where the selected item is null
                }
            }
            
        }
    }
}
