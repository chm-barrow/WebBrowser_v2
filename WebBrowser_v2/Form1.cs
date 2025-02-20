using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration; // To use the App.Config functionalities.
using System.IO; // To read from a file.

namespace WebBrowser_v2
{
    public partial class Form1 : Form
    {
        private HTTP_Requests http = new HTTP_Requests();
        private string fav_url;
        private string histo_url;
        private string favorite;

        public Form1()
        {
            InitializeComponent();
            Display(ConfigurationManager.AppSettings["homepage"]); // This is okay since nothing happens after this.
        }

        private async Task<int> Display(string url)
        {
            // Initialize HTTP Client and needed info.
            int check = await http.DownloadPage(url);
            
            if (http.getStatus().Equals("OK"))
            {
                this.textBox2.Text = http.getContent();
            }
            else
            {
                this.textBox2.Text = http.getStatus();
            }

            if (url != ConfigurationManager.AppSettings["homepage"]) // We don't add the homepage to history.
            {
                if (check == 0)
                {
                    string history = ConfigurationManager.AppSettings["history"];

                    history += ',' + url;

                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings.Remove("history");
                    config.AppSettings.Settings.Add("history", history); // (WIP) Need to limit size of history.
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                
            }
            

            // Need to return something to await function.
            return check;
        }

        /* Main button used to search for a website. */
            private async void button1_Click(object sender, EventArgs e)
        {
            string data = textBox1.Text.Trim();
            int check = await Display(data);
            SetTitle(data,check);
        }

        /* Setting up the homepage. */
        private void button2_Click(object sender, EventArgs e)
        {
            if ( !(this.textBox1.Text.Equals("")))
            {
                if (this.textBox1.Text.StartsWith("http://") || this.textBox1.Text.StartsWith("https://"))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    config.AppSettings.Settings.Remove("homepage");
                    config.AppSettings.Settings.Add("homepage", this.textBox1.Text);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    this.textBox2.Text = "The homepage has been set up.";
                }
            }
            else
            {
                this.textBox2.Text = "The URL you have entered is incorrect.";
            }

            
        }

        /* Adding a favorite website. */
        private void button3_Click(object sender, EventArgs e)
        {
            Form4 favorites_form = new Form4();
            favorites_form.MdiParent = this.MdiParent;

            // Subscribe to the ChildEvent event
            favorites_form.ChildEvent += new EventFavoriteData(Data_Favorite);

            // Show the form after subscribing to the event
            favorites_form.ShowDialog();

            string favorites = ConfigurationManager.AppSettings["favorites"];
            if (favorites.Equals(""))
            {
                favorites = this.favorite;
                this.favorite = null;
            }
            else
            {
                favorites += ',' + this.favorite;
                this.favorite = null;
            }
            

            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove("favorites");
            config.AppSettings.Settings.Add("favorites", favorites);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /* Accessing history. */
        private async void button4_Click(object sender, EventArgs e)
        {
            Form3 history_page = new Form3();
            history_page.MdiParent = this.MdiParent;

            // Subscribe to the ChildEvent event
            history_page.ChildEvent += new EventHistory(URL_History);

            // Show the form after subscribing to the event
            history_page.ShowDialog();

            if (this.histo_url != null)
            {
                await Display(this.histo_url);
                this.histo_url = null;
            }
            
        }

        /* Accessing the favorites. */
        public async void button5_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["favorites"].Equals(""))
            {
                this.textBox2.Text = "You have not added any favorites yet.";
            }
            else
            {
                Form2 favorites_page = new Form2(false);
                favorites_page.MdiParent = this.MdiParent;

                // Subscribe to the ChildEvent event
                favorites_page.ChildEvent += new EventFavorite(URL_Favorite);

                // Show the form after subscribing to the event
                favorites_page.ShowDialog();

                if (this.fav_url != null)
                {
                    await Display(this.fav_url);
                    this.fav_url = null;
                }
            }
        }

        /* Used to do some string manipulation to extract website name and append status code. */
        private void SetTitle(string data, int check)
        {
            string temp;
            if (check == 0)
            {
                temp = data.Split('/')[2];
                this.Text = temp + " - " + this.http.getStatus();
            }
            else
            {
                this.Text = this.http.getStatus();
            }
            
        }

        /* WIP (To avoid code repetition). */
        private void SetupConfig(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            if (!key.Equals("history"))
            {
                config.AppSettings.Settings.Remove(key);
            }
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        

        /* Allows to get data from Form2 (favorite page). */
        void URL_Favorite(string data)
        {
            if (data != null)
            {
                this.fav_url = data;
            }
        }

        void Data_Favorite(string data)
        {
            if (data != null)
            {
                this.favorite = data;
            }
        }

        /* Allows to get data from Form3 (history page). */
        void URL_History(string data)
        {
            if (data != null)
            {
                this.histo_url = data;
            }
        }

        /* Removing a favorite website. */
        private void button6_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["favorites"].Equals(""))
            {
                this.textBox2.Text = "You have not added a favorite yet.";
            }
            else
            {
                Form2 favorites_page = new Form2(true);
                favorites_page.MdiParent = this.MdiParent;

                // Subscribe to the ChildEvent event
                favorites_page.ChildEvent += new EventFavorite(URL_Favorite);

                // Show the form after subscribing to the event
                favorites_page.ShowDialog();
            }
        }

        /* Accessing the homepage. */
        private async void button7_Click(object sender, EventArgs e)
        {
            int check = await Display(ConfigurationManager.AppSettings["homepage"]);
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            // Base path: C:\Users\[user]\source\repos\WebBrowser_v2\WebBrowser_v2\bin\Debug
            if (this.textBox1.Text.Equals(""))
            {
                this.textBox2.Text = "Enter a valid path to use the bulk download.";
            }
            else
            {
                try
                {
                    string filename = this.textBox1.Text;
                    string[] fileline = File.ReadAllLines(filename);

                    string bulk_dl = "";



                    for (int i = 0; i < fileline.Length; i++)
                    {
                        int check = await http.DownloadPage(fileline[i]);
                        bulk_dl += http.getStatus() + " - ";
                        bulk_dl += http.getLength() + " - ";
                        bulk_dl += fileline[i] + "\r\n";
                    }
                    this.textBox2.Text = bulk_dl;
                }
                catch (FileNotFoundException es) // In case of error in the name of the file.
                {
                    this.textBox2.Text = "We couldn't find the file you specified.\r\nPlease put your file in the following folder:" + Path.GetDirectoryName(Application.ExecutablePath);
                }
                catch (NotSupportedException)
                {
                    this.textBox2.Text = "This is not an appropriate path.";
                }
                
            }
        }

        /* WIP (To prevent the history to overflow) and to provide a way for the user to delete all history */
        private void clearHistory()
        {

        }
    }
}
