using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace DerpGame.Model
{
    public class Network
    {
        public    List<List<Object>> data = new List<List<Object>>();
        List<Object> args = new List< Object > ();
        List<Object> objects = new List< Object > ();
        List<Object> players = new List<Object>();
        public Network()
        {
            data = new List<List<Object>>();
            data.Add(args);
            data.Add(objects);
            data.Add(players);
        }
        public void Client()
        {
            var Client = new UdpClient();
            var RequestData = ObjectToByteArray(data);
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);
            Client.EnableBroadcast = true;
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
            var ServerResponseData = Client.Receive(ref ServerEp);
            var ServerResponse = ByteArrayToObject(ServerResponseData);
            Console.WriteLine("Recived from "+ServerEp.Address.ToString());
            data = (List<List<Object>>)ServerResponse; 
            Client.Close();
        }


        public void StartSever()
        {
            ThreadStart severStart = new ThreadStart(new Network().Server);
            Thread sever = new Thread(severStart);
            sever.Start();
        }
        private void Server()
        {
            var Server = new UdpClient(8888);


            while (true)
            {
                var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                var ClientRequestData = Server.Receive(ref ClientEp);
                var ClientRequest = ByteArrayToObject(ClientRequestData);
                List < List<Object> > recivedList = (List<List<Object>>)ClientRequest;

                byte[] ResponseData;
                if (recivedList[0].Count == 0)
                {
                    ResponseData = ObjectToByteArray(data);
                }
                else {
                    List<String> args = new List<string>();
                    for (int index = 0; index < recivedList[0].Count; index++)
                    {
                        args.Add(((String)recivedList[0][index]).ToLower());
                    }
                    if(args.Contains("init"))
                    {
                        ResponseData = ObjectToByteArray(1);
                    }
                    else
                    {
                        ResponseData = ObjectToByteArray(data);
                    }

                }
                   Server.Send(ResponseData, ResponseData.Length, ClientEp);
                
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
