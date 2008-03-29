using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace DevSandbox.WebServer
{
    static class HeaderParser
    {
        private static Regex statusLineRegex;
        private static Regex PairHeaderLineRegex;

        static HeaderParser()
        {
            HeaderParser.statusLineRegex = new Regex(@"(.+) \s*(.+) \s*(.+)", RegexOptions.Singleline);
            HeaderParser.PairHeaderLineRegex = new Regex(@"([^:]+):\s*(.+)", RegexOptions.Singleline);
        }

        public const byte END_OF_LINE_BYTE = 10; //10=NewLine(ASCII)
        public delegate bool RequestingDataHandler(out ArraySegment<byte> buffer);
        public static void Parse(
            RequestingDataHandler moreData, 
            out byte[] remainBuffer,Request request)
        {
            ArraySegment<byte> buffer;
            bool lastCharWasEndOfLine = false;
            bool lastCharWasBeginOfLine = false;
            StringBuilder lineBuffer = new StringBuilder();
            while (moreData(out buffer))
            {
                int byteCount = 0;
                int maxByteIndex = buffer.Count - 1;
                for (int currentByteIndex = 0; currentByteIndex <= maxByteIndex; currentByteIndex++)
                {
                    byte currentByteVal = buffer.Array[currentByteIndex];
                    if (currentByteVal == END_OF_LINE_BYTE)
                    {
                        if (lastCharWasBeginOfLine && lastCharWasEndOfLine) //Bytes 13 y 10
                        {
                            //Fin del Encabezado.
                            if (currentByteIndex != maxByteIndex)
                            {
                                int diffSize = maxByteIndex - currentByteIndex;
                                remainBuffer = new byte[diffSize];
                                Array.Copy(buffer.Array,
                                    currentByteIndex + 1 //This byte is END_OF_LINE_BYTE, wee need to copy from the next one.
                                    , remainBuffer, 0, diffSize);
                            }
                            else
                            {
                                remainBuffer = null;
                            }
                            lineBuffer = null;
                            return;
                        }
                        else
                        {
                            lastCharWasEndOfLine = true;
                            //Agregamos la linea que se ha creado.
                            addHeaderLineSingle(lineBuffer, request);
                            //Reset line buffer.
                            lineBuffer = new StringBuilder();
                        }
                    }
                    else if (currentByteVal == 13)
                    {
                        lastCharWasBeginOfLine = true;
                    }
                    else
                    {
                        lastCharWasBeginOfLine = false;
                        lastCharWasEndOfLine = false;
                        //any other byte, just add to the currentLine that is been builded.
                        lineBuffer.Append((char)currentByteVal);

                    }
                    byteCount++;
                }
            }//While
            remainBuffer = null;
        }//Parse

        internal static  void ParseForm(Request request)
        {
           
            Regex parametersRegex = new Regex("([^=]+)=(.+)", RegexOptions.Singleline);
            string dataS = Encoding.UTF8.GetString(request.Data);

            string[] paramsPairs = dataS.Split('&');
            foreach (string paramsPair in paramsPairs)
            {
                //([^=]+)=(.+)
                Match paramMatch = parametersRegex.Match(paramsPair);
                string value = System.Web.HttpUtility.UrlDecode(paramMatch.Groups[2].Value);
                request.PostParameters.Add(paramMatch.Groups[1].Value, value);
            }
        
        }

        internal static void addHeaderLineSingle(StringBuilder line,Request request)
        {
            RequestHeader header = request.Headers;
            Connection.trace("Trying to add '{0}' as a Header line", line.ToString());
            string sline = line.ToString();
            if (PairHeaderLineRegex.IsMatch(sline))
            {
                Match m = PairHeaderLineRegex.Match(sline);
                HeaderLine hl = new HeaderLine(m.Groups[1].Value, m.Groups[2].Value);
                if (hl.Name == "Host")
                {
                    if (!PairHeaderLineRegex.IsMatch(hl.Value))
                    {
                        throw new Exception("Host header line has no valid format");
                    }
                    m = PairHeaderLineRegex.Match(hl.Value);
                    request.Hostname = m.Groups[1].Value;

                    request.Port = int.Parse(m.Groups[2].Value);
                }
                header.Add(hl);
            }
            else
            {
                Match m = statusLineRegex.Match(sline);
                request.MethodName = m.Groups[1].Value;
                request.ResourcePath = m.Groups[2].Value;
                request.ProtocolId = m.Groups[3].Value;
                m = null;
            }
        }
    }
}
