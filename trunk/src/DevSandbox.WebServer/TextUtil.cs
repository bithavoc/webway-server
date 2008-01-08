using System;

namespace DevSandbox.WebServer
{
	
	
	internal struct TextUtil
	{
		
	public const string Coma =",";
        public const char UriSeparatorCh = '/';
        public const string UriSeparatorStr = "/";
        public static char[] UriSeparatorChArr = new char[] { '/'};
        public const char WhiteSpace = ' ';
        public const char Equal = '=';

        public const string HttpMethodGet = "GET";
        public const string HttpMethodPost = "POST";

        public const string HttpResponseReason_OK = "OK";

        public const string HttpHeaderField_SetCookie_Case = "Set-Cookie";
        public const string HttpHeaderField_Cookie_Case = "Cookie";
        public const string HttpHeaderField_Cookie = "COOKIE";

        public const string HttpProtocol1_1= "HTTP/1.1";
        public const string HttpHeaderField_Connection = "CONNECTION";
        public const string HttpHeaderField_Connection_Case = "Connection";
        public const string HttpHeaderField_KeepAlive = "KEEP-ALIVE";
        public const string HttpHeaderField_KeepAlive_Case = "Keep-Alive";
        public const string HttpHeaderField_Date__Case= "Date";

        public const string HttpHeaderField_ContentType = "CONTENT-TYPE";
        public const string HttpHeaderField_ContentType__Case = "Content-Type";

        public const string HttpHeaderField_ContentLength = "CONTENT-LENGTH";
        public const string HttpHeaderField_ContentLength__Case = "Content-Length";
            public const string HttpHeaderField_Server__Case = "Server";

        public const string Semicolon= ":";
        public const char Comma = ';';

        public const string HttpServerDateFormat = "ddd, dd MMM yyyy HH:mm:ss 'GMT'";

        public const string HttpResponseFormat_TextXml_Case = "text/html";
        public const string HttpResponseFormat_AppXWWWFormUrlEncoded_Case = "application/x-www-form-urlencoded";
        public const string Slash = "/";
        public static string GlobalNewLineString
        {
            get
            {
                return Environment.NewLine;
            }
        }
        //TODO: UTF8 Siempre?
        public static System.Text.Encoding GlobalEncoding
        {
            get
            {
                return  System.Text.Encoding.UTF8;
            }
        }
	}
}
