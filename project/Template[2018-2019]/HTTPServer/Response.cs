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
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(HTTPVersion version,StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            string StatusLine=GetStatusLine(version,code);
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
           

            // TODO: Create the request string

        }

        private string GetStatusLine(HTTPVersion version, StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            string SCode = ((int)code).ToString();
            statusLine += version + " "+SCode+" ";

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
