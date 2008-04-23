using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;

namespace DevSandbox.WebServer
{
    internal class Connection : IDisposable
    {
        private Socket socket;
        private const string HeaderSeparator = "\n\r";
        private int bufferSize = 5;
        private int contentBufferSize = 32;
        private Server server;
        private RequestListener listener;
        private bool isClosed;
        private static List<Connection> conns = new List<Connection>();
        public Connection(Socket socket, Server server, RequestListener listener)
        {
            this.socket = socket;
            this.server = server;
            this.listener = listener;

            //Prepare the socket.
            this.socket.LingerState = new LingerOption(true, 60);
            this.socket.Blocking = true;
            conns.Add(this);
        }

        public void Write(byte[] data)
        {
            if (this.isClosed)
            {
                throw new ConnectionClosedException("You can not write on a disposed Connection");
            }
            try
            {
                this.socket.Send(data);
            }
            catch (SocketException sex)
            {
                this.Close();
            }
        }

        [System.Diagnostics.ConditionalAttribute(InternalDebug.DEBUG_SYMBOL)]
        internal static void trace(string val, params object[] vals)
        {
            InternalDebug.trace(val, vals);
        }
        [System.Diagnostics.ConditionalAttribute(InternalDebug.DEBUG_SYMBOL)]
        internal static void traceColor(ConsoleColor color, string val, params object[] vals)
        {
            Console.ForegroundColor = color;
            InternalDebug.trace(val, vals);
            Console.ResetColor();
        }
        internal static string selfTraceByteArr(byte[] buff)
        {
            return selfTraceByteArr(buff, 0, buff.Length);
        }
        internal static string selfTraceByteArr(byte[] buff, int index, int count)
        {
            string s = string.Format("BUFF(totalSize={0},sIndex={1},count={2})", buff.Length, index, count);
            s += "{";
            for (int i = 0; i < count; i++)
            {
                byte b = buff[index + i];
                s += b.ToString();
                if (i != (count - 1))
                {
                    s += ", ";
                }
            }
            s += "}";
            return s;
        }

        void parseHeader(out byte[] initialContentBuffer, Request request)
        {

            HeaderParser.Parse(delegate(out ArraySegment<byte> buffer)
            {
                byte[] receiveBuffer = new byte[this.bufferSize];
                int receiveBufferCount = this.socket.Receive(receiveBuffer);
                if (receiveBufferCount == 0)
                {
                    buffer = new ArraySegment<byte>(receiveBuffer, 0, 0);
                    return false;
                }

                buffer = new ArraySegment<byte>(receiveBuffer, 0, receiveBufferCount);

                return true;

            }, out initialContentBuffer, request);
        }

        private void listenAsyncWork(object obj)
        {
            try
            {
                byte[] initialContentBuffer = null;
                Request request = new Request();
                RequestHeader mh = request.Headers;

                parseHeader(out initialContentBuffer, request);
#if DEBUG
                foreach (HeaderLine h in request.Headers)
                {
                    InternalDebug.trace("HeaderLine='{0}'='{1}'", h.Name, h.Value);
                }
#endif
                int contentLength = mh.Contains("Content-Length") ? int.Parse(request.Headers["Content-Length"].Value) : 0;

                const string FormMime = "application/x-www-form-urlencoded";
                if (contentLength != 0)
                {
                    request.Data = receiveContent(initialContentBuffer, contentLength);
                    trace("Received Content bytes{0}", selfTraceByteArr(request.Data));
                    if (request.ContentType == FormMime) //is a form.
                    {
                        HeaderParser.ParseForm(request);

                    }
                }
                Response response = new Response(this);

                if (string.IsNullOrEmpty(request.Hostname))
                {
                    //Si no hay Hostname(como es el caso de los POST) entonces colocamos la IP.
                    request.Hostname = ((IPEndPoint)this.socket.RemoteEndPoint).Address.ToString();
                }
                //Send the request to the listener.
                bool listenerVHostFound = this.server.__processRequestFromListener(this.listener, request, response);
                if (!listenerVHostFound)
                {
                    this.Dispose();
                }
                else
                {
                    if (request.Headers.Contains("Connection") && request.Headers["Connection"].Value == "keep-alive")
                    {

                    }
                }
                /*if(RequestReceived != null)
                {
                    this.RequestReceived(this,new RequestReceivedEventArgs(request));
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Listen()
        {
            this.isClosed = false;
            ThreadPool.QueueUserWorkItem(new WaitCallback(listenAsyncWork));
        }
        public void Close()
        {
            this.isClosed = true;

            //this.socket.Shutdown(SocketShutdown.Both);
            //this.socket.Close();
            //TODO: Implementar Keep-alive y dejar que la conexion se cierre sola por periodo de tiempo o porque el cliente cerro la conexion.
        }
        byte[] receiveContent(byte[] initialContentBuffer, int contentLength)
        {

            byte[] content = new byte[contentLength];
            int receivedBytesCount = 0;
            if (initialContentBuffer != null)
            {
                initialContentBuffer.CopyTo(content, 0);
                receivedBytesCount += initialContentBuffer.Length;
            }
            trace("Readed content before start reading: {0}", selfTraceByteArr(content));
            int toReceiveCount = 0;
            while (receivedBytesCount < contentLength)
            {
                trace("before: receivedBytesCount: {0}, contentLength={1} ", receivedBytesCount, contentLength);
                toReceiveCount = receivedBytesCount + contentBufferSize > contentLength ? contentLength - receivedBytesCount : contentBufferSize;
                int receiveCount = this.socket.Receive(content, receivedBytesCount, toReceiveCount, SocketFlags.None);
                receivedBytesCount += receiveCount;
                trace("so far: receivedBytesCount: {0}, contentLength={1} ", receivedBytesCount, contentLength);
                trace("Readed content so far: {0}", selfTraceByteArr(content));
            }
            return content;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!this.isClosed)
            {
                this.Close();
            }
            this.socket = null;
        }

        #endregion
    }//Connection
}