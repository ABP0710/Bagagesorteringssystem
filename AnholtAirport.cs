using System.Collections.Concurrent;

namespace Bagagesorteringssystem
{
    public class AnholtAirport
    {
        //Buffers
        //the first is used for when the baggage is received at the check-in
        public static ConcurrentQueue<Luggage?> storage = new ConcurrentQueue<Luggage?>();

        //the second and third is used for the sorting class, to send the luggage to the terminals
        public static ConcurrentQueue<Luggage?> DepotOne = new ConcurrentQueue<Luggage?>();
        public static ConcurrentQueue<Luggage?> DepotTwo = new ConcurrentQueue<Luggage?>();

        //the forth is also used in the sorting class, but is used if there is luggage with an unknown destination
        public static ConcurrentQueue<Luggage?> lostLuggage = new ConcurrentQueue<Luggage?>();


        static void Main(string[] args)
        {
            //makes 3 Check-ins
            for (int j = 0; j < 3; j++)
            {
                CheckIn checkIn = new CheckIn(j + 1);
            }

            //makes 2 terminals
            for (int i = 0; i < 2; i++)
            {
                Terminal terminal = new Terminal(i + 1);
            }

            //makes the sorting machine
            Sorting sortingMachine = new Sorting();

            //output to user
            do
            {
                Console.Clear();
                Console.WriteLine("--------anholt lufthavn bagage system--------");
                Console.WriteLine();
                Console.WriteLine("Bagage til sortering: " + storage.Count);
                Console.WriteLine();
                Console.WriteLine("Terminal 1");
                Console.WriteLine("Bagage i Terminal: " + DepotOne.Count);
                Console.WriteLine();
                Console.WriteLine("Terminal 2");
                Console.WriteLine("Bagage i terminal: " + DepotTwo.Count);
                Console.WriteLine();
                Console.WriteLine("Bagage fejl: " + lostLuggage.Count);
                Console.WriteLine();
                Console.WriteLine("Stop programmet: tryk esc");

                Thread.Sleep(500);

            } while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));

            //join the threads
            CheckIn.CheckInJoin();
            Sorting.SortingJoin();
            Terminal.TerminalJoin();
        }
    }
}