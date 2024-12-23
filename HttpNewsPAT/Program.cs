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
        public static async Task<string> SingIn(string Login, string Password)
        {
            string url = "http://127.0.0.1/ajax/login.php";
            Trace.WriteLine($"Выполняем запрос: {url}");
            var formData = new Dictionary<string, string>
            {
                { "login", Login },
                { "password", Password }
            };
            var content = new FormUrlEncodedContent(formData);
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            Trace.WriteLine($"Статус выполнения: {response.StatusCode}");
            string responseFromServer = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Ответ сервера: {responseFromServer}");
            if (response.Headers.TryGetValues("Set-Cookie", out var cookieValues))
            {
                string cookie = cookieValues.FirstOrDefault();
                return cookie;
            }
            return null;
        }
    }
}
