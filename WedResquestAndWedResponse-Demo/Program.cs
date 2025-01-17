using System;
using System.IO;
using System.Net;
namespace WedResquestAndWedResponse_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("http://www.contoso.com/default.html");
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("Status: " + response.StatusDescription);
            Console.WriteLine(new string('*', 50));
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            Console.WriteLine(new string('*', 50));
            reader.Close();
            dataStream.Close();
            response.Close();
        }
    }
}
