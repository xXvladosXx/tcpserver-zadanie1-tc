using System;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServerSocketApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var listener = new TcpListener(System.Net.IPAddress.Any, 8000);
            var port = 8000;
            listener.Start();
            Console.WriteLine($"Maker: Hutsenko Vladyslav");
            Console.WriteLine($"Port TCP: {port}");

            while (true)
            {
                Console.WriteLine("Waiting for a connection.");
                var client = listener.AcceptTcpClient();
                Console.WriteLine("Client accepted.");
                using var stream = client.GetStream();
                using var sw = new StreamWriter(stream);
                try
                {
                    var ipAdress = (client.Client.RemoteEndPoint as IPEndPoint)!.Address;
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync($"https://ipapi.co/{ipAdress}/utc_offset");
                    var content = await response.Content.ReadAsStringAsync();
                    DateTime dateTime = default;
                    if (content != "Undefined")
                    {
                        dateTime = DateTime.UtcNow.Add(DateTimeOffset.ParseExact(content.Insert(3, ":"), "zzz", CultureInfo.CurrentCulture).Offset);
                    }

                    var contentToSend = $@"
Hello world!
Your ip: {ipAdress?.ToString()}
Your date time: {dateTime.ToString()}";

                    sw.WriteLine($@"
HTTP/1.1 200 OK 
Content-Type: text/plain 
Content-Length: {contentToSend.Length} 
{contentToSend}");

                    sw.Flush();
                    client.Client.Disconnect(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong.");
                    sw.WriteLine(e.ToString());
                }
            }
        }
    }
}