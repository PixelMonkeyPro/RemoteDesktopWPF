﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace HookerClient
{
    class ServerEntity
    {
        //usato per connettersi al server
        public TcpClient server;
        //usato per mandare i messaggi al server
        public UdpClient UdpSender;
        public IPAddress ipAddress;
        public string name;
        public string password;
        public IPEndPoint remoteIPEndPoint;
        public NetworkStream stream;
        //usato per clipboard
        public IPEndPoint cbLocal;
        public TcpClient CBClient;
        public int Id; //id usato per creare gli oggetti dinamici 
        public int port_base;
        private int DEFAULT_BASE_PORT = 5143; 
        public ServerEntity( string name)
        {
            try
            {
                this.name = name;
                IPAddress[] ipaddrs;
                ipaddrs = Dns.GetHostAddresses(this.name);
                //risolvo l'indirizzo ipv4 in fase di costruzione
                this.ipAddress = ipaddrs.First(a => a.AddressFamily == AddressFamily.InterNetwork);
                this.password = "TODO";
                this.remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                this.cbLocal = new IPEndPoint(this.ipAddress, 9898);
                // this.cbServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // this.CBClient = new TcpClient(this.ipAddress.ToString(),9898);
            }catch (SocketException se)
            {
                Console.WriteLine("[" + this.name + "]" + se.Message);
            }
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

        private String getPassword()
        {
            return this.password;
        }

        private String getName()
        {
            return this.name;
        }
        private int getId()
        {
            return this.Id;
        }

        internal void setId(int index)
        {
            this.Id = index;
        }
        internal void setPortFromString(String port)
        {
            try
            {
                int convertedPort = Convert.ToInt32(port);
                this.port_base = convertedPort;
            }
            catch (Exception e)
            {
                this.port_base = DEFAULT_BASE_PORT;
            }
        }

        internal bool authenticateWithPassword()
        {
            byte[] b =ObjectToByteArray(this.password);
            UdpSender.Send(b, b.Length);
            byte[] receivedResponse = UdpSender.Receive(ref remoteIPEndPoint);
            bool result = (bool)ByteArrayToObject(receivedResponse);
            return result;
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
