using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UberIntelligentAutocompletion;

namespace ServerAutocompletion
{
    class Program
    {
        static UberIntelligentAutocompleter Autocompleter;

        static void Main(string[] args)
        {
            string path = null;
            int port = default(int);

            var result = Parser.Default.ParseArguments<Options>(args);
            if (!result.Errors.Any())
            {
                path = result.Value.SourceWords;
                port = result.Value.Port;
            }
            else
            {
                foreach (var arg in args)
                {
                    if (default(int) != port)
                    {
                        path = arg;
                    }
                    else
                    {
                        int.TryParse(arg, out port);
                    }
                }
            }

            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                Autocompleter = new UberIntelligentAutocompleter(reader);
            }
                

            var ipHost = Dns.GetHostEntry("localhost");
            var ipAddr = ipHost.AddressList[0];
            var ipEndPoint = new IPEndPoint(ipAddr, port);

            var sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                while (true)
                {
                    Console.WriteLine($"Listing port {ipEndPoint}");

                    var handler = sListener.Accept();
                    string data = null;

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n\n");

                    // Отправляем ответ клиенту\
                    string reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов";
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }

        
    }
}
