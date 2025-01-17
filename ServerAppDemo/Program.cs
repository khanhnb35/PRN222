using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerAppDemo
{
    internal class Program
    {
        static void ProcessMessage(object parm)
        {
            string data;
            int count;

            try
            {
                TcpClient client = parm as TcpClient;

                // Buffer for reading data
                byte[] bytes = new byte[256];

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                // Loop to receive all the data sent by the client
                while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to an ASCII string
                    data = Encoding.ASCII.GetString(bytes, 0, count);
                    Console.WriteLine($"Received: {data} at {DateTime.Now:t}");

                    // Process the data sent by the client
                    data = $"Data processed: {data.ToUpper()}";
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    // Send back a response
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine($"Sent: {data}");
                }

                // Shutdown and end connection
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine("Waiting message...");
            }
        }

        // Method to execute the server
        static void ExecuteServer(string host, int port)
        {
            int Count = 0;
            TcpListener server = null;

            try
            {
                Console.Title = "Server Application";
                IPAddress localAddr = IPAddress.Parse(host);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests
                server.Start();
                Console.WriteLine(new string('*', 40));
                Console.WriteLine("Waiting for a connection... ");

                // Enter the listening loop
                while (true)
                {
                    // Perform a blocking call to accept requests
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"Number of clients connected: {++Count}");
                    Console.WriteLine(new string('*', 40));

                    // Create a thread to receive and send messages
                    Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    thread.Start(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                server?.Stop();
                Console.WriteLine("Server stopped. Press any key to exit!");
                Console.Read();
            }
        }

        // Main method
        public static void Main()
        {
            string host = "127.0.0.1";
            int port = 13000;

            // Start the server
            ExecuteServer(host, port);
        }
    }
}
