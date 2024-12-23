using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpNewsPAT
{
    public class Program
    {
        private static HttpClient _httpClient;
        private static string _cookie;

        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create(" http://news.permaviat.ru/main ");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            Console.Read();
        }
    }
}
