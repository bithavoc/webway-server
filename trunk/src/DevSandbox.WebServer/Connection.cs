using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;

namespace DevSandbox.WebServer
{
	internal class Connection
	{
		private Socket socket;
		private const string HeaderSeparator = "\n\r";
		private int bufferSize = 5;
		private int contentBufferSize = 32;
		private HeaderParser headerParser;
		private Server server;
		private RequestListener listener;
		private bool isClosed;
		public Connection(Socket socket,Server server,RequestListener listener)
		{
			this.socket = socket;
			this.server = server;
			this.listener = listener;
			this.headerParser = new HeaderParser();
		}
		public void Write(byte[] data)
		{
			this.socket.Send(data);
		}
		
		[System.Diagnostics.ConditionalAttribute("DEBUG")]
		internal static void trace(string val,params object[] vals)
		{
			InternalDebug.trace(val,vals);
		}
		internal static string selfTraceByteArr(byte[] buff)
		{
			return selfTraceByteArr(buff,0,buff.Length);
		}
		internal static string selfTraceByteArr(byte[] buff,int index,int count)
		{
			string s = string.Format("BUFF(totalSize={0},sIndex={1},count={2})",buff.Length,index,count);
			s+="{";
			for( int i = 0;i < count;i++)
			{
				byte b = buff[index +i];
				s +=b.ToString();
				if(i != (count - 1))
				{
					s+=", ";
				}
			}
			s+="}";
			return s;
		}
		
		void parseHeader(out byte[] initialContentBuffer)
		{
			
			initialContentBuffer = null;
			//Begin: ReadHeader
			//unsafe read buffer.
			byte[] receiveBuffer = null;
			int receiveBufferCount = 0;				
			while(true)
			{				
				receiveBuffer = new byte[this.bufferSize];
				trace("Receiving");
				receiveBufferCount = this.socket.Receive(receiveBuffer);
				if(receiveBufferCount == 0)break;
				
				trace("Received buffer='{0}','{1}'",selfTraceByteArr(receiveBuffer),Encoding.ASCII.GetString(receiveBuffer));
				trace("Parsing Header Lines");
				bool stopAnalizingHeader = this.headerParser.Analize(new ArraySegment<byte>(receiveBuffer,0,receiveBufferCount),out initialContentBuffer);
				receiveBuffer = null;
				if(stopAnalizingHeader)break;
			}// while
		}

		public void Listen()
		{
			this.isClosed = false;
			ThreadPool.QueueUserWorkItem(delegate
			                                              {
				try{
						byte[] initialContentBuffer = null;
						Request request= new Request();
						
						RequestHeader mh = new RequestHeader();
						this.headerParser.Reset(request,mh);
						
						parseHeader(out initialContentBuffer);
						request.InitHeader(mh);
						foreach(HeaderLine h in mh)
						{
							InternalDebug.trace("HeaderLine='{0}'='{1}'",h.Name,h.Value);
						}
						
						int contentLength = mh.Contains("Content-Length")? int.Parse(request.Headers["Content-Length"].Value):0;
						if(contentLength != 0)
						{
							request.Data = receiveContent(initialContentBuffer,contentLength);
							trace("Received Content bytes{0}",selfTraceByteArr(request.Data));
						}
						Response response = new Response(this);
						//Send the request to the listener.
						this.server.__processRequestFromListener(this.listener,request,response);
						/*if(RequestReceived != null)
						{
							this.RequestReceived(this,new RequestReceivedEventArgs(request));
						}*/
					}
					catch(Exception ex)
					{
						Console.WriteLine(ex.ToString());
						Console.WriteLine(ex.StackTrace);
					}
			});
		}
		public void Close()
		{
			this.isClosed = true;
			this.socket.Shutdown(SocketShutdown.Both);
			this.socket.Close();
			this.socket = null;
		}
		byte[] receiveContent(byte[] initialContentBuffer,int contentLength)
		{
			
			byte[] content = new byte[contentLength];
			int receivedBytesCount = 0;
			if(initialContentBuffer != null)
			{
				initialContentBuffer.CopyTo(content,0);
				receivedBytesCount+=initialContentBuffer.Length;
			}
			trace("Readed content before start reading: {0}",selfTraceByteArr(content));
			int toReceiveCount = 0;
			while(receivedBytesCount < contentLength)
			{
				trace("before: receivedBytesCount: {0}, contentLength={1} ",receivedBytesCount,contentLength);
				toReceiveCount = receivedBytesCount+contentBufferSize >  contentLength?contentLength-receivedBytesCount:contentBufferSize;
				int receiveCount = this.socket.Receive(content,receivedBytesCount,toReceiveCount,SocketFlags.None);
				receivedBytesCount+=receiveCount;
				trace("so far: receivedBytesCount: {0}, contentLength={1} ",receivedBytesCount,contentLength);
				trace("Readed content so far: {0}",selfTraceByteArr(content));
			}
			return content;
		}
	}//Connection
}