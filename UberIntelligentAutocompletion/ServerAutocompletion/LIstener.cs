using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using UberIntelligentAutocompletion;

namespace ServerAutocompletion
{
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousSocketListener
    {
        public ManualResetEvent allDone = new ManualResetEvent(false);

        //private readonly IAsyncResult ReceiveCallback;
        //private readonly IAsyncResult SendCallback;

        UberIntelligentAutocompleter Autocompleter;

        public AsynchronousSocketListener(/*IAsyncResult receiveCallback*//*, IAsyncResult sendCallback*/ UberIntelligentAutocompleter autocompleter)
        {
            //ReceiveCallback = receiveCallback;
            //SendCallback = sendCallback;
            Autocompleter = autocompleter;
        }

        public void StartListening(int port)
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList.First();
            var localEndPoint = new IPEndPoint(ipAddress, port);

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Console.WriteLine("Waiting for a connection...");
                while (true)
                {
                    allDone.Reset();

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine(Environment.NewLine + "Press ENTER to continue...");
            Console.Read();

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            var handler = ((Socket)ar.AsyncState).EndAccept(ar);

            var state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            var state = (StateObject)ar.AsyncState;
            var handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                var content = state.sb.ToString();

                string result = null;
                using (var writer = new StringWriter())
                {
                    Autocompleter.Complete(content, writer);

                    result = writer.ToString();
                }

                if (content.IndexOf(Environment.NewLine) > -1)
                {
                    Console.WriteLine($"Received word : {content}");

                    Send(handler, result);
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}