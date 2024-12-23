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
        private static HttpClient httpClient = new HttpClient();
        private static string _cookie;

        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener("DebugLog.txt"));
            Help();
            while (true)
            {
                SetComand();
            }
        }
        public static void WriteLog(string debugContent)
        {
            Debug.WriteLine(debugContent);
            Debug.Flush();
        }
        public static async Task<string> SingIn(string Login, string Password)
        {
            string url = "http://127.0.0.1/ajax/login.php";
            WriteLog($"Выполняем запрос: {url}");
            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("login", Login),
                new KeyValuePair<string, string>("password", Password)
            });

            HttpResponseMessage response = await httpClient.PostAsync(url, postData);
            WriteLog($"Статус выполнения: {response.StatusCode}");
            if (response.IsSuccessStatusCode)
            {
                string cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                if (!string.IsNullOrEmpty(cookies))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Token = cookies.Split(';')[0].Split('=')[1];
                    Console.WriteLine("Печенька: токен = " + Token);
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка выполнения запроса: {response.StatusCode}");
            }
        }
        public static async Task<string> GetContent()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                string url = "http://127.0.0.1/main";
                WriteLog($"Выполняем запрос: {url}");
                httpClient.DefaultRequestHeaders.Add("token", Token);
                HttpResponseMessage response = await HttpClient.GetAsync(url);
                WriteLog($"Статус выполнения: {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка выполнения запроса: {response.StatusCode}");
                    return string.Empty;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка выполнения запроса: не авторизован");
                return string.Empty;
            }
        }
    }
}
