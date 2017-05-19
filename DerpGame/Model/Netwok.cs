using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Microsoft.Xna.Framework;


namespace DerpGame.Model
{
    public class Network
    {
        private TimeSpan lastSync;
        private TimeSpan send;
        public List<List<Object>> data;
        List<Object> args = new List< Object > ();
        List<Object> objects = new List< Object > ();
        List<Object> players = new List<Object>();
        Thread Sever;
        Thread Cli;


		public Network(GameTime last) 
        {
            send = TimeSpan.FromMilliseconds(1);
            lastSync = last.TotalGameTime;
            data = new List<List<Object>>();
            data.Add(args);
            data.Add(objects);
            data.Add(players);

        }

        public void Client()
        {
            
                Console.WriteLine("We init");
                var Client = new UdpClient();
                var RequestData = ObjectToByteArray(data);
                var ServerEp = new IPEndPoint(IPAddress.Any, 0);
                Client.EnableBroadcast = true;
                Console.WriteLine("We init");
                    Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
                Console.WriteLine("We init");
                    var ServerResponseData = Client.Receive(ref ServerEp);
                    Console.WriteLine("We init");
                    var ServerResponse = ByteArrayToObject(ServerResponseData);
                    Console.WriteLine("We init");
                    Console.WriteLine("Recived from " + ServerEp.Address.ToString());
                    data = (List<List<Object>>)ServerResponse;
                    Console.WriteLine("We Finish");

                Client.Close();

        }

        private void killSever()
        {
            if (Sever == null || !Sever.IsAlive)
            {
                ThreadStart severStart = new ThreadStart(Server);
                Sever = new Thread(severStart);
                Sever.Start();
                int mili = 0;
                while (Sever.IsAlive)
                {
                    if(100 < mili)
                    {
                        Sever.Abort();
                    }
                    Thread.Sleep(1);
                    mili++;
                }
            }
        }
        private void killClient()
        {
            if (Cli == null || !Cli.IsAlive)
            {
                ThreadStart severStart = new ThreadStart(Client);
                Cli = new Thread(severStart);
                Cli.Start();
                int mili = 0;
                while (Cli.IsAlive)
                {
                    if (100 < mili)
                    {
                        Cli.Abort();
                    }
                    Thread.Sleep(1);
                    mili++;
                }
            }
        }

        public void StartClient(GameTime time)
        {
			if (lastSync.TotalMilliseconds + send.TotalMilliseconds < time.TotalGameTime.TotalMilliseconds)
			{

				Console.WriteLine("we Start");
				lastSync = time.TotalGameTime;
                ThreadStart start = new ThreadStart(killClient);
				Thread kill = new Thread(start);
				kill.Start();
			}
			else
			{
				Console.WriteLine("ok");
			}
        }
        public void StartSever(GameTime time)
        {
            if (lastSync.TotalMilliseconds + send.TotalMilliseconds < time.TotalGameTime.TotalMilliseconds) {

				Console.WriteLine("we Start");
                lastSync = time.TotalGameTime;
                ThreadStart start = new ThreadStart(killSever);
                Thread kill = new Thread(start);
                kill.Start();
        }
            else
            {
                Console.WriteLine("ok");
            }
            
        }
        private void Server()
        {
            try
            {
                Console.WriteLine("we Start");
                var Server = new UdpClient(8888);
                var ClientEp = new IPEndPoint(IPAddress.Any, 0);

                Console.WriteLine("we Start");
                var ClientRequestData = Server.Receive(ref ClientEp);
                Console.WriteLine("we Start");
                var ClientRequest = ByteArrayToObject(ClientRequestData);

                Console.WriteLine("we Start");
                List<List<Object>> recivedList = (List<List<Object>>)ClientRequest;
                Console.WriteLine("we Start");
                byte[] ResponseData;
                if (recivedList[0].Count == 0)
                {
                    ResponseData = ObjectToByteArray(data);
                }
                else
                {
                    Console.WriteLine("we Start");
                    List<String> args = new List<string>();
                    for (int index = 0; index < recivedList[0].Count; index++)
                    {
                        args.Add(((String)recivedList[0][index]).ToLower());
                    }
                    if (args.Contains("init"))
                    {
                        ResponseData = ObjectToByteArray(1);
                    }
                    else
                    {
                        ResponseData = ObjectToByteArray(data);
                    }
                    Console.WriteLine("we Start");

            Server.Close();
                }
                Server.Send(ResponseData, ResponseData.Length, ClientEp);
            }
            catch
            {
                
            }

        }

		private byte[] ObjectToByteArray(Object obj)
		{
			if (obj == null)
				return null;

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, obj);

			return ms.ToArray();
		}

		// Convert a byte array to an Object
		private Object ByteArrayToObject(byte[] arrBytes)
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			Object obj = (Object)binForm.Deserialize(memStream);

			return obj;
		}
    }
}
