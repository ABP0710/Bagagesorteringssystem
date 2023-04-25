using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagagesorteringssystem
{
    public class Sorting
    {
        //the thread
        public static Thread s;

        //constructor
        public Sorting()
        {
            s = new Thread(() => Sorter());
            s.Start();
        }

        /// <summary>
        /// method to join the sorting thread
        /// </summary>
        public static void SortingJoin()
        {
            s.Join();
        }

        /// <summary>
        /// Sends the luggage to the teminal, according to the luggages destination
        /// </summary>
        /// <returns></returns>
        private Luggage Sorter()
        {
            while (true)
            {
                //trys to enter the lock on the storage queue
                if (Monitor.TryEnter(AnholtAirport.storage))
                {
                    try
                    {
                        //if the queue is empty, do this
                        if (AnholtAirport.storage.Count == 0)
                        {
                            //debug output
                            Debug.WriteLine("Sortering: klar til modtagelse");

                            //sleep, for make the output more visible
                            Thread.Sleep(100);

                            //release the lock on the resource and block the thread untill the lock is reacquired
                            Monitor.Wait(AnholtAirport.storage);
                        }

                        //takes a look at the queue, and see if there is an object without removing it
                        if (AnholtAirport.storage.TryPeek(out Luggage? lug))
                        {
                            //if the destination on the obj is 1 call this method
                            if (lug.Destination.Equals(1))
                            {
                                TerminalOneLuggage();
                            }
                            //if the destination on the obj is 2 call this method
                            else if (lug.Destination.Equals(2))
                            {
                                TerminalTwoLuggage();
                            }
                            //if the destination on the obj is neither 1 or 2 there is an issus, and this method needs to be called
                            else
                            {
                                RegistrationIssue();
                            }
                        }
                    }
                    finally
                    {
                        //debug output
                        Debug.WriteLine("Sortering: Bagage afsendt");

                        //sleep, for make the output more visible
                        Thread.Sleep(100);

                        //exit the lock on the storage resource
                        Monitor.Exit(AnholtAirport.storage);
                    }
                }
            }
        }

        /// <summary>
        /// handel luggage for terminal 1
        /// </summary>
        private static void TerminalOneLuggage()
        {
            //enter the lock
            Monitor.Enter(AnholtAirport.DepotOne);

            try
            {
                //if there is an obj in the storage queue, it will be removed 
                AnholtAirport.storage.TryDequeue(out Luggage? luggageForTerminalOne);

                //the removed obj is then enqueued in the DepotOne queue
                AnholtAirport.DepotOne.Enqueue(luggageForTerminalOne);

                //debug output
                Debug.WriteLine("Sortering: Bagage til Terminal 1");

                //sleep, for make the output more visible
                Thread.Sleep(100);

                //informs other threads that changes has been made to the resource
                Monitor.PulseAll(AnholtAirport.DepotOne);
            }
            finally
            {
                //exit the lock
                Monitor.Exit(AnholtAirport.DepotOne);
            }
        }
        /// <summary>
        /// handel luggage for terminal 2
        /// </summary>
        private static void TerminalTwoLuggage()
        {
            //enter the lock
            Monitor.Enter(AnholtAirport.DepotTwo);

            try
            {
                //if there is an obj in the storage queue, it will be removed 
                AnholtAirport.storage.TryDequeue(out Luggage? luggageForTerminalTwo);

                //the removed obj is then enqueued in the DepotTwo queue
                AnholtAirport.DepotTwo.Enqueue(luggageForTerminalTwo);

                //debug output
                Debug.WriteLine("Sortering: Bagage til Terminal 2");

                //sleep, for make the output more visible
                Thread.Sleep(100);

                //informs other threads that changes has been made to the resource
                Monitor.PulseAll(AnholtAirport.DepotTwo);
            }
            finally
            {
                //ecit the lock
                Monitor.Exit(AnholtAirport.DepotTwo);
            }
        }

        /// <summary>
        /// handel the luggage when there is an issue with the destination
        /// </summary>
        private static void RegistrationIssue()
        {
            Monitor.Enter(AnholtAirport.lostLuggage);

            try
            {
                AnholtAirport.storage.TryDequeue(out Luggage? lostAndFound);
                AnholtAirport.lostLuggage.Enqueue(lostAndFound);
                Debug.WriteLine("Sortering: Fejl på registrering, bagage sendt til opbevaring!");
                Thread.Sleep(100);
                Monitor.Pulse(AnholtAirport.lostLuggage);
            }
            finally
            {
                Monitor.Exit(AnholtAirport.lostLuggage);
            }
        }
    }
}
