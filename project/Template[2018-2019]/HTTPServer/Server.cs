﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            IPEndPoint IPE = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portNumber);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(IPE);
        }

        public void StartServer()
        {
            LoadRedirectionRules(Configuration.RedirectDictionarytFilePath);
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(10000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                Socket clientSocket = this.serverSocket.Accept();
                //TODO: accept connections and start thread for each accepted connection.
                Thread NewThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                NewThread.Start(clientSocket);
            }
            //To keep the app alive until all threads have finished.
            //Console.ReadLine();

        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Socket clientSock = (Socket)obj;
            clientSock.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            int receivedLength = 0;
            byte[] data=new byte[100000000];
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    receivedLength = clientSock.Receive(data);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        // TODO: close client socket
                        clientSock.Close();
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    string RequestString = Encoding.ASCII.GetString(data, 0, receivedLength);
                    Request NewRequest = new Request(RequestString);
                    // TODO: Call HandleRequest Method that returns the response
                    Response NewResponse = HandleRequest(NewRequest);
                    // TODO: Send Response back to client

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

         
        }

        Response HandleRequest(Request request )
        {
            
            string content;
            StatusCode status=StatusCode.OK;
            try
            {
                //TODO: check for bad request 
                if (!request.ParseRequest())
                {
                    status = StatusCode.BadRequest;
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
                string Path = Configuration.RootPath + request.relativeURI;
                //TODO: check for redirect
                LoadRedirectionRules(Configuration.RedirectDictionarytFilePath);
                if (Configuration.RedirectionRules.ContainsKey(request.relativeURI)) //KEY NEEDS TO CHANGE ! 
                {
                    status = StatusCode.Redirect;
                    request.relativeURI = Configuration.RedirectionRules[request.relativeURI]; //Also needs to change
                }
                //TODO: check file exists
                if (File.Exists(Path))
                {
                    // Create OK response
                    status = StatusCode.OK;
                }
                //TODO: read the physical file

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                // TODO: in case of exception, return Internal Server Error. 
                Logger.LogException(ex);
            }
            Response response = new Response(request.httpVersion, status, "", "", "");
            return response;
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                string[] Lines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach(string line in Lines)
                {
                    string[] files = line.Split(new char[] { ',' }, 2);
                    Configuration.RedirectionRules[files[0]] = files[1];
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
