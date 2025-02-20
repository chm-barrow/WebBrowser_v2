using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebBrowser_v2
{
    public class HTTP_Requests
    {
        private HttpClient client = new HttpClient();
        private string content;
        private string status;
        private string length;
        public async Task<int> DownloadPage(string url)
        {

            // Gets page code source as string.
            try
            {
            var response = await client.GetAsync(url);
            this.status = response.StatusCode.ToString();
            this.length = response.Content.Headers.ContentLength.ToString();
            this.content = await client.GetStringAsync(url);

                // Need to return something to await function.
            return 0;
            }
            catch(InvalidOperationException)
            {
                this.content = "You have not entered a valid URL.";
                return -1;
            }
            catch(UriFormatException)
            {
                this.content = "You have not entered a valid URL. Error message: " + this.status;
                return -1;
            }
            catch(HttpRequestException)
            {
                this.content = "We couldn't find the website you are looking for. Error message: " + this.status;
                return -1;
            }
            
        }

        public string getContent()
        {
            return this.content;
        }

        public string getStatus()
        {
            return this.status;
        }

        public string getLength()
        {
            return this.length;
        }
    }
}