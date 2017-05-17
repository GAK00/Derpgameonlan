using System;
using System.Net.Sockets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace DerpGame.Model
{
    public class Network
    {
        List<Object> data = new List<Object>();
        public void Client()
        {
            var Client = new UdpClient();
            var RequestData = Encoding.ASCII.GetBytes("YOlo");
            var ServerEp = new IPEndPoint(IPAddress.Any, 0);

            Client.EnableBroadcast = true;
            Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, 8888));

            var ServerResponseData = Client.Receive(ref ServerEp);
            var ServerResponse = Encoding.ASCII.GetString(ServerResponseData);
            Console.WriteLine("Recived {0} from {1}", ServerResponse, ServerEp.Address.ToString());

            Client.Close();
        }


        public void StartSever()
        {
            ThreadStart severStart = new ThreadStart(new Network().Server);
            Thread sever = new Thread(severStart);
        }
        private void Server()
        {
            var Server = new UdpClient(8888);
            var ResponseData = 

            while (true)
            {
                var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                var ClientRequestData = Server.Receive(ref ClientEp);
                var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);

                Console.WriteLine("Recived {0} from {1}, sending response", ClientRequest, ClientEp.Address.ToString());
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
