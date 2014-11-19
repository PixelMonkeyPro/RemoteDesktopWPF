﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

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
        public ServerEntity( string name)
        {
            this.name = name;
            IPAddress[] ipaddrs;
            ipaddrs = Dns.GetHostAddresses(this.name);
            //risolvo l'indirizzo ipv4 in fase di costruzione
            this.ipAddress = ipaddrs.First(a => a.AddressFamily == AddressFamily.InterNetwork);
            this.password = "TODO";
            this.remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void setPassword(string password)
        {
            this.password = password;
        }

    }
}