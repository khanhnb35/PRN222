using System;
using System.Net.Sockets;
using System.Text;
namespace ClientApp
{


    class Program
    {
        static void ConnectServer(string server, int port)
        {
            try
            {
                // Create a TcpClient
                TcpClient client = new TcpClient(server, port);
                Console.Title = "Client Application";
                NetworkStream stream = client.GetStream();

                Console.WriteLine($"Connected to server at {server}:{port}");
                Console.WriteLine($"Local Endpoint: {client.Client.LocalEndPoint}");
                Console.WriteLine();

                while (true)
                {
                    Console.Write("Enter a message (or press Enter to exit): ");
                    string message = Console.ReadLine();

                    if (string.IsNullOrEmpty(message))
                    {
                        break;
                    }

                    // Translate the message into a byte array.
                    byte[] data = Encoding.ASCII.GetBytes(message);

                    // Send the message to the server.
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"Sent: {message}");

                    // Receive the response from the server.
                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string responseData = Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine($"Received: {responseData}");
                }

                // Close the client connection.
                client.Close();
                Console.WriteLine("Disconnected from the server.");
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        static void Main(string[] args)
        {
            string server = "127.0.0.1"; // Server IP
            int port = 8080; // Server Port
            ConnectServer(server, port);
        }
    }
}
