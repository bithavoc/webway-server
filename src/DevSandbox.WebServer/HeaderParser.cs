// /home/jhernandez/Projects/MsTCP/NetMTP/HeaderParser.cs created with MonoDevelop
// User: jhernandez at 2:49 PM 11/30/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace DevSandbox.WebServer
{
	public class HeaderParser :IDisposable
	{
		private Request request;
		private RequestHeader header = null;
		private StringBuilder currentLineBuilder;

		private Regex statusLineRegex;
		public HeaderParser()
		{
			this.statusLineRegex = new Regex(@"(.+) \s*(.+) \s*(.+)",RegexOptions.Singleline);
		}
		public void Dispose()
		{
			this.currentLineBuilder = null;
		}
		static object RegexInitLock = new object(); 
		static Regex pairHeaderLineRegex;
		public static Regex PairHeaderLineRegex
		{
			get
			{
				lock(RegexInitLock)
				{
					if(pairHeaderLineRegex == null)
					{
						//
						//pairHeaderLineRegex = new Regex(@"([^:]+):\s*(\S+)");
						pairHeaderLineRegex = new Regex(@"([^:]+):\s*(.+)",RegexOptions.Singleline);
					}
					return pairHeaderLineRegex;
				}
			}
		}
		
		public void Reset(Request request,RequestHeader header)
		{
			this.request = request;
			this.header = header;
			this.currentLineBuilder = new StringBuilder();
		}
		
	
		private void addHeaderLineSingle()
		{
			Connection.trace("Trying to add '{0}' as a Header line",this.currentLineBuilder.ToString());
			if(PairHeaderLineRegex.IsMatch(this.currentLineBuilder.ToString()))
			{
				Match m = PairHeaderLineRegex.Match(this.currentLineBuilder.ToString());
				HeaderLine hl = new HeaderLine(m.Groups[1].Value,m.Groups[2].Value);
				if(hl.Name=="Host")
				{
					if(!PairHeaderLineRegex.IsMatch(hl.Value))
					{
						throw new Exception("Host header line has no valid format");
					}
					m = PairHeaderLineRegex.Match(hl.Value);
					this.request.Hostname = m.Groups[1].Value;
					this.request.Port = int.Parse(m.Groups[2].Value);
				}
				this.header.Add(hl);
			}
			else
			{
				//SimpleHeaderLine sl = new SimpleHeaderLine();
				
				Match m = this.statusLineRegex.Match(this.currentLineBuilder.ToString());
				this.request.MethodName = m.Groups[1].Value;
				this.request.ResourcePath = m.Groups[2].Value;
				this.request.ProtocolId = m.Groups[3].Value;
				m = null;
				/*Console.WriteLine("Method={0}",this.method);
				Console.WriteLine("Resource={0}",this.resourcePath);
				Console.WriteLine("Version={0}",this.protocolId);*/
			}
		}
		
		bool lineBuilderHasContent()
		{
			bool has =this.currentLineBuilder == null?false:this.currentLineBuilder.Length> 1; 
				Connection.trace("currentLineBuilder has content?R={0}",has);
			return has;
		}
		
		void initializeLineBuilder()
		{
			Connection.trace("currentLineBuilder was  forced to be initialized");
			this.currentLineBuilder = new StringBuilder();
		}
		
		void ensureLineBuilder()
		{
			Connection.trace("currentLineBuilder was ensured");
			if(this.currentLineBuilder == null)this.currentLineBuilder = new StringBuilder();
		}
		
		void resetLineBuilder()
		{
			Connection.trace("currentLineBuilder was reseted");
			this.currentLineBuilder = null;
		}
		
		/// <returns>Debe retornar true si alcanzo el final del encabezado, false si necesita otro buffer para analizar</returns>
		public bool Analize(ArraySegment<byte> buffer,out byte[] remainBuffer)
		{
			if(this.header == null)throw new Exception("HeaderParser is not initialized");
			Connection.trace("Running Analize");
			this.ensureLineBuilder();
			int maxByteIndex = buffer.Count -1; 
			bool lineUnderConstruction = false;
			for(int currentByteIndex = 0;currentByteIndex <= maxByteIndex;currentByteIndex++)
			{
				
				byte currentByteVal = buffer.Array[currentByteIndex];
				
				if(currentByteVal == 10)
				{
					Connection.trace("CurrentByteVal=(10),Ignored");
					lineUnderConstruction = true;
				}
				else if(currentByteVal == 13)
				{
					if(!lineBuilderHasContent())
					{
						Connection.trace("End of Header reached and no content found");
						if(currentByteIndex != maxByteIndex)
						{
							Connection.trace("Remain buffer found");
							int remainCount = buffer.Count - (currentByteIndex +1) ;
							remainBuffer = new byte[remainCount];
							Connection.trace("remainCount={0},maxByteIndex={1},currentByteIndex={2}",remainCount,maxByteIndex,currentByteIndex);
							//Array.Copy(receiveBuffer,remainIndex,lastRemainBuffer,0,remains);
							//buffer.Array.CopyTo(remainBuffer,currentByteIndex +1);
							Array.Copy(buffer.Array,currentByteIndex+1,remainBuffer,0,remainCount);
						}
						else
						{
							remainBuffer = null;
						}
						this.resetLineBuilder();
						
						return true;
					}
					else
					{
						Connection.trace("Line ends,'{0}' in the current line Builder",this.currentLineBuilder.ToString());
						addHeaderLineSingle();
						initializeLineBuilder();
						lineUnderConstruction = false;
					}
				}
				else
				{
					Connection.trace("CurrentByteVal={0} ({1}) was added to the line builder",(char)currentByteVal,currentByteVal);
					this.currentLineBuilder.Append((char)currentByteVal);
					lineUnderConstruction = true;
				}
				
			}//for
			
			Connection.trace("End of Analize");
			
			if(lineUnderConstruction  && !this.lineBuilderHasContent())
			{
				Connection.trace("End of header reached after analize");
				this.resetLineBuilder();
				remainBuffer = null;
				return true;
			}
			remainBuffer = null;
			return false;
		}
		public RequestHeader Header
		{
			get
			{
				return this.header;
			}
		}
	}
}
