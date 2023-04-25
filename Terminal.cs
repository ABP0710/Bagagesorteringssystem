using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagagesorteringssystem
{
    internal class Terminal
    {
        //the thread
        public static Thread t;

        //int used for the terminal number
        public int terminalNumber { get; set; }

        //constructor
        public Terminal(int TerminalNumber)
        {
            terminalNumber = TerminalNumber;

            t = new Thread(() => FilPlane());
            t.Start();
        }

        /// <summary>
        /// method to join the terminal thread
        /// </summary>
        public static void TerminalJoin()
        {
            t.Join();
        }

        /// <summary>
        /// method used for filling the plane by calling other methods in the switch
        /// </summary>
        /// <returns></returns>
        private Luggage FilPlane()
        {
            while (true)
            {
                switch (terminalNumber + 1)
                {
                    case 1:
                        Terminal1();
                        break;
                    case 2:
                        Terminal2();
                        break;

                    default:
                        break;
                }
            }
        }

        private void Terminal1()
        {
            while (true)
            {
                //lock on the DepotOne queue
                lock (AnholtAirport.DepotOne)
                //enter the lock
                {
                    try
                    {
                        //the terminal is waiting for a plane, the plane is a counter that trikker when it hits 30                     
                        while (AnholtAirport.DepotOne.Count <= 30)
                        {
                            //release the lock on the resource and block the thread untill the lock is reacquired
                            Monitor.Wait(AnholtAirport.DepotOne);

                            //Debug putput
                            Debug.WriteLine("Terminal 1: venter på fly");

                            //sleep, for make the output more visible
                            Thread.Sleep(100);
                        }
                    }
                    finally
                    {
                        //Debug putput
                        Debug.WriteLine("Terminal 1: Fly ankommet, læsser bagage!");

                        //removing all item in the queue
                        AnholtAirport.DepotOne.Clear();
                        
                        //sleep, for make the output more visible
                        Thread.Sleep(100);
                    }
                    //exit the lock
                }
            }
        }

        private void Terminal2()
        {
            while (true)
            {
                //lock on the DepotTwo queue
                lock (AnholtAirport.DepotTwo)
                //enter the lock
                {
                    try
                    {
                        //the terminal is waiting for a plane, the plane is a counter that trikker when it hits 30 
                        while (AnholtAirport.DepotTwo.Count < 30)
                        {
                            //Debug putput
                            Debug.WriteLine("Terminal 2: venter på fly");

                            //sleep, for make the output more visible
                            Thread.Sleep(100);

                            //release the lock on the resource and block the thread untill the lock is reacquired
                            Monitor.Wait(AnholtAirport.DepotTwo);
                        }
                    }
                    finally
                    {
                        //Debug putput
                        Debug.WriteLine("Terminal 2: Fly ankommet. læsser bagage!");

                        //removing all item in the queue
                        AnholtAirport.DepotTwo.Clear();
                     
                        //sleep, for make the output more visible
                        Thread.Sleep(100);
                    }
                    //exit the lock
                }
            }
        }
    }
}
