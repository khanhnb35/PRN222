using System.Net.Sockets;
using System.Text;

namespace ClientAppDemo
{
    internal class Program
    {
        static void ConnectServer(string server, int port)
        {
            string message, responseData;
            int bytes;

            try
            {
                // Create a TcpClient
                TcpClient client = new TcpClient(server, port);
                Console.Title = "Client Application";
                NetworkStream stream = null;

                while (true)
                {
                    Console.Write("Input message <press Enter to exit>: ");
                    message = Console.ReadLine();
                    if (message == string.Empty)
                    {
                        break;
                    }

                    // Translate the passed message into ASCII and store it as a byte array
                    byte[] data = Encoding.ASCII.GetBytes(message);

                    // Get a client stream for reading and writing
                    stream = client.GetStream();

                    // Send the message to the connected TcpServer
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent: {message}");

                    // Receive the TcpServer response
                    // Use buffer to store the response bytes
                    data = new byte[256];
                    bytes = stream.Read(data, 0, data.Length);
                    responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine($"Received: {responseData}");
                }

                // Shutdown and end connection
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }

        // Main method
        static void Main(string[] args)
        {
            string server = "127.0.0.1";
            int port = 13000;

            // Connect to the server
            ConnectServer(server, port);
        }
    }
}
