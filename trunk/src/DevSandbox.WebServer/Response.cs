using System;
using System.Text;
using System.IO;

namespace DevSandbox.WebServer
{
	public class Response
	{
		private Connection conn;
		private string responseFormat = "text/html";
		private int statusCode = 200;
		private string statusReason = "OK";
		private bool headerWasSent = false;
		private MemoryStream buffer;
		private Encoding responseEncoding;
		private HttpContext context;
		internal Response(Connection conn)
		{
			this.conn = conn;
			this.buffer = new MemoryStream();
			this.responseEncoding = Encoding.UTF8;
		}
		
		public void Flush()
		{
			if(!this.headerWasSent)
			{
				this.sendHeader();
			}
			if(this.buffer.Length > 0)
			{
				this.conn.Write(this.buffer.ToArray());
				this.buffer.SetLength(0);
			}
            
		}
		
		public HttpContext Context
		{
			get
			{
				return this.context;
			}
			internal set
			{
				this.context = value;
			}
		}
		
		public void End()
		{
			this.Flush();
			if(this.context.Request.Headers.Contains("Keep-Alive"))
			{
				InternalDebug.trace("Client is trying to keep alive but we dont support this yet");
			}
			this.conn.Dispose();
            this.conn = null;
            System.GC.WaitForPendingFinalizers();
            System.Threading.Thread.CurrentThread.Abort();
			InternalDebug.trace("Response Flushed and Ended");
		}
		 private static string formatResponseDate()
        {

            string dtStr = System.DateTime.Now.ToString(TextUtil.HttpServerDateFormat, System.Globalization.CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
            return dtStr;
        }
		private void sendHeader()
		{
			 //WRITE STATUS LINE
            StringBuilder sBuilder =  new StringBuilder();
            sBuilder.Append("HTTP/1.1");//write Http version.
            sBuilder.Append(TextUtil.WhiteSpace);
            sBuilder.Append(statusCode.ToString());
            sBuilder.Append(TextUtil.WhiteSpace);
            sBuilder.Append(statusReason);
            sBuilder.Append(TextUtil.GlobalNewLineString);
			
			//SERVER DATE
            sBuilder.Append(TextUtil.HttpHeaderField_Date__Case);
            sBuilder.Append(TextUtil.Semicolon);
            sBuilder.Append(TextUtil.WhiteSpace);
            string serverDateString = formatResponseDate();
            sBuilder.Append(serverDateString);
            serverDateString = null;
            sBuilder.Append(TextUtil.GlobalNewLineString);			

            //SERVER ID
            sBuilder.Append(TextUtil.HttpHeaderField_Server__Case);
            sBuilder.Append(TextUtil.Semicolon);
            sBuilder.Append(TextUtil.WhiteSpace);
            sBuilder.Append("WebWay 1.0/Unix");//write Http Server NAME.
            sBuilder.Append(TextUtil.GlobalNewLineString);

            //Connection:Close
            sBuilder.Append(TextUtil.HttpHeaderField_Connection_Case);
            sBuilder.Append(TextUtil.Semicolon);
            sBuilder.Append(" close");
            sBuilder.Append(TextUtil.GlobalNewLineString);

            //CONTENT-TYPE
            sBuilder.Append(TextUtil.HttpHeaderField_ContentType__Case);
            sBuilder.Append(TextUtil.Semicolon);
            sBuilder.Append(TextUtil.WhiteSpace);
            sBuilder.Append(this.responseFormat);
            sBuilder.Append(TextUtil.GlobalNewLineString);
			
			//CONTENT-LENGTH
            sBuilder.Append(TextUtil.HttpHeaderField_ContentLength__Case);
            sBuilder.Append(TextUtil.Semicolon);
            sBuilder.Append(TextUtil.WhiteSpace);
            sBuilder.Append(this.buffer.Length.ToString());
            sBuilder.Append(TextUtil.GlobalNewLineString);
            
			//End of header(blank line)
			sBuilder.Append(TextUtil.GlobalNewLineString);
			this.conn.Write(this.responseEncoding.GetBytes(sBuilder.ToString()));
			this.headerWasSent = true;
		}
		
		public void Write(byte[] content)
		{
			this.buffer.Write(content,0,content.Length);
		}//Write
		
		public void Write(string text)
			{
this.Write(this.responseEncoding.GetBytes(text));
}
		
		public string ResponseFormat
		{
			get
			{
				return this.responseFormat;
			}set
			{
				this.responseFormat = value;
			}
		}//ResponseFormat
		
		public int StatusCode
		{
			get
			{
				return this.statusCode;
			}set
			{
				this.statusCode = value;
			}
		}//StatusCode
		
		public string StatusReason
		{
			get
			{
				return this.statusReason;
			}set
			{
				this.statusReason = value;
			}
		}//StatusReason
		
	}
}
