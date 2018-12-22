using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
       // StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            responseString = "";
            string StatusLine=GetStatusLine(code);
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            responseString += StatusLine+"\r\n";
            responseString += "Content-Type: " + contentType + "\r\n";
            responseString += "Content-Length: "+ content.Length + "\r\n";
            responseString += "Date: " + DateTime.Now + "\r\n";
            if (redirectoinPath != "")
            {
                responseString += "Redirected-To: " + redirectoinPath + "\r\n";
            }
            responseString += "";
            responseString += content;


            // TODO: Create the request string

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            string SCode = ((int)code).ToString();
            statusLine += Configuration.ServerHTTPVersion + " "+SCode+" ";

            if (code == StatusCode.BadRequest)
            {
                statusLine += "Bad Request";
            }
            else if (code == StatusCode.InternalServerError)
            {
                statusLine += "Internal Server Error";
            }
            else if (code == StatusCode.NotFound)
            {
                statusLine += "Not found";
            }
            else if (code == StatusCode.OK)
            {
                statusLine += "OK";
            }
            else
            {
                statusLine += "Redirect";
            }
            return statusLine;
        }
    }
}
