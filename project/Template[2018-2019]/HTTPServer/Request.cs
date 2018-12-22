using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        public HTTPVersion httpVersion;
        string requestString;
        public string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
            
            headerLines = new Dictionary<string, string>();
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        
        public bool ParseRequest()
        {
            try
            {
                //TODO: parse the receivedRequest using the \r\n delimeter   
                string[] Lines = Regex.Split(requestString,"\r\n");
                // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
                if(Lines.Count() <3)
                {
                    return false;
                }
                //Parse Request line
                if (!ParseRequestLine(Lines[0])) return false;
                // Load header lines into HeaderLines dictionary
                if (!LoadHeaderLines(Lines)) return false;
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
            //throw new NotImplementedException();
            return true;
        }

        private bool ParseRequestLine(string RequestLine)
        {
            //throw new NotImplementedException();
            string[] words = RequestLine.Split(' ');
            if (words[0].ToLower() == "get")
            {
                method = RequestMethod.GET;
            }
            else if (words[0].ToLower() == "head")
            {
                method = RequestMethod.HEAD;
            }
            else
            {
                method = RequestMethod.POST;
            }
            relativeURI = words[1];
            if (!ValidateIsURI(relativeURI)) return false;
            if (words[2].ToLower() == "http/1.0")
            {
                httpVersion = HTTPVersion.HTTP10;
            }
            else if (words[2].ToLower() == "http/1.1")
            {
                httpVersion = HTTPVersion.HTTP11;
            }
            else
            {
                httpVersion = HTTPVersion.HTTP09;
            }
            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines(string[] Lines)
        {
            int i = 1;
            for (i = 1; i < Lines.Count(); i++)
            {
                if (!Lines[i].Contains(':'))
                {
                    break;
                }
                else
                {
                    string[] Heads = Lines[i].Split(new char[] { ':' }, 2); //2 substrings
                    headerLines[Heads[0]] = Heads[1];
                }
            }
            // Validate blank line exists
            if (i == Lines.Count())
            {
                return false;
            }
            //Load Content Lines
            i++;
            contentLines = new string[Lines.Count()-i];
            int j = 0;
            for (; i < Lines.Count(); i++)
            {
                contentLines[j] = Lines[i];
                j++;
            }   
            return true;
        }

        private bool ValidateBlankLine()
        {
            throw new NotImplementedException();
        }

    }
}
