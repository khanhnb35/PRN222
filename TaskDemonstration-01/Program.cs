namespace TaskDemonstration_01
{
    internal class Program
    {
        static void PrintNumber(string message)
        {
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"{message}:{i}");
                Thread.Sleep(1000);
            }
        }

        static void Main()
        {
            Thread.CurrentThread.Name = "Main";

            // Create a task by using a lambda expression
            Task task01 = new Task(() => PrintNumber("Task 01"));
            task01.Start();

            // Create a task by using a delegate and run the task
            Task task02 = Task.Run(delegate
            {
                PrintNumber("Task 02");
            });

            // Create a task by using an Action delegate
            Task task03 = new Task(new Action(() =>
            {
                PrintNumber("Task 03");
            }));
            task03.Start();

            Console.WriteLine($"Thread '{Thread.CurrentThread.Name}'");
            Console.ReadKey();
        }
    }
}
