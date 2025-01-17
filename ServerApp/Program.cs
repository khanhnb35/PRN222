using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerApp
{
    internal class Program
    {
        static void ProcessMessage(object param)
        {
            try
            {
                TcpClient client = param as TcpClient;

                // Lấy thông tin endpoint của client
                string clientEndpoint = client.Client.RemoteEndPoint.ToString();
                Console.WriteLine($"Client connected from {clientEndpoint}");

                // Tạo buffer để đọc dữ liệu
                Byte[] buffer = new Byte[256];
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    // Đọc dữ liệu từ client
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // Client đã ngắt kết nối

                    // Chuyển đổi dữ liệu thành chuỗi
                    string data = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Console.WriteLine($"Received from {clientEndpoint}: {data}");

                    // Chuyển đổi dữ liệu thành chữ hoa
                    string response = data.ToUpper();
                    byte[] responseData = Encoding.ASCII.GetBytes(response);

                    // Gửi phản hồi lại client
                    stream.Write(responseData, 0, responseData.Length);
                    Console.WriteLine($"Sent to {clientEndpoint}: {response}");
                }

                // Đóng kết nối khi client ngắt
                Console.WriteLine($"Client disconnected: {clientEndpoint}");
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ProcessMessage: {ex.Message}");
            }
        }

        static void ExecuteServer(string host, int port)
        {
            TcpListener server = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse(host);
                server = new TcpListener(localAddr, port);
                server.Start();

                Console.Title = "Server Application";
                Console.WriteLine($"Server started at {host}:{port}");
                Console.WriteLine("Waiting for client connections...");

                while (true)
                {
                    // Chấp nhận kết nối từ client
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"New client connected: {client.Client.RemoteEndPoint}");

                    // Tạo một thread mới để xử lý client
                    Thread clientThread = new Thread(ProcessMessage);
                    clientThread.Start(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExecuteServer: {ex.Message}");
            }
            finally
            {
                server?.Stop();
                Console.WriteLine("Server stopped.");
            }
        }

        static void Main(string[] args)
        {
            string host = "127.0.0.1"; // Địa chỉ IP của server
            int port = 8080; // Port của server
            ExecuteServer(host, port);
        }
    }
}
