using System;
using System.Collections.Generic;

namespace DevSandbox.WebServer
{
	public class Request
	{
		private RequestHeader header;
		private byte[] data;
		private string method;
		private string resourcePath;
		private string protocolId;
		private string host;
		private int port;
        private Dictionary<string, string> postParameters;

		internal Request()
		{
            this.header = new RequestHeader();
            this.postParameters = new Dictionary<string, string>();

		}

        public Dictionary<string,string> PostParameters
        {
            get 
            { 
                return postParameters; 
            }
        }

		public byte[] Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}
		
		public bool HasHeader
		{
			get
			{
				return this.header != null;
			}
		}
		
		public int ContentLength
		{
			get
			{
				return this.data == null?0:this.data.Length;
			}
		}

        public const string ContentTypeKey = "Content-Type";
        public string ContentType
        {
            get
            {
                
                return this.header.Contains(ContentTypeKey)?this.header[ContentTypeKey].Value:string.Empty;
            }
        }
		
		public RequestHeader Headers
		{
			get
			{
				return this.header;
			}
		}
		public string MethodName
		{
			get
			{
				return this.method;
			}
			internal set
			{
				this.method = value;
			}
		}
		public string Hostname
		{
			get
			{
				return this.host;
			}
			internal set
			{
				this.host = value;
			}
		}
		public int Port
		{
			get
			{
				return this.port;
			}
			internal set
			{
				this.port = value;
			}
		}
		public string ResourcePath
		{
			get
			{
				return this.resourcePath;
			}
			internal set
			{
				this.resourcePath = value;
			}
		}
		public string ProtocolId
		{
			get
			{
				return this.protocolId;
			}
			internal set
			{
				this.protocolId = value;
			}
		}
		public string UserAgent
		{
			get
			{
				return this.header["User-Agent"].Value;
			}
		}
	}//Message
	
	public class RequestHeader : IEnumerable<HeaderLine>
	{
		private Dictionary<string,HeaderLine> list;
		public RequestHeader()
		{
			this.list = new Dictionary<string,HeaderLine>();
		}
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}
		
		public bool Contains(string name)
		{
			return this.list.ContainsKey(name);
		}
		public HeaderLine this[string name]
		{
			get
			{
				if(this.list.ContainsKey(name))
					return this.list[name];
				return HeaderLine.Empty;
			}
		}
		
		/*public void Add(string name)
		{
			this.Add(new SimpleHeaderLine(name));
		}*/
		public void Add(string name,string value)
		{
			this.Add(new HeaderLine(name,value));
		}
		public void Add(HeaderLine line)
		{
			//checkRestrictedName(line.Name);
			this.list.Add(line.Name,line);
		}
		public IEnumerator<HeaderLine> GetEnumerator()
		{
			return this.list.Values.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((System.Collections.IEnumerable)this.list).GetEnumerator();
		}
	}//MessageHeader
	
	public struct HeaderLine
	{
		public static HeaderLine Empty;
		private string name;
		private string value;
		public HeaderLine(string name,string value)
		{
			this.name = name;
			this.value = value;
		}
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.name) && string.IsNullOrEmpty(this.value);
			}
		}
	}//PairHeaderLine
}
