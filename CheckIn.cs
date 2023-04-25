using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagagesorteringssystem
{

    public class CheckIn
    {
        //var used to set the id number of the used check-in counter
        public int counterId { get; set; }

        //unset int used to set an id on the luggage
        public int idNumberForLuggage;

        //thread
        public static Thread c;

        public CheckIn(int CounterId)
        {
            this.counterId = CounterId;

            //makes a new thread and starts
            c = new Thread(ReceiveLuggageFromPassenger);
            c.Start();
        }

        /// <summary>
        /// method to join the checkIn thread
        /// </summary>
        public static void CheckInJoin()
        {
            c.Join();
        }

        private void ReceiveLuggageFromPassenger()
        {

            Random rdm = new Random();

            while (true)
            {
                //lock on the storage queue
                lock (AnholtAirport.storage)
                //enter the resource
                {
                    try
                    {
                        //if more than 25 luggages in the queue, it wil wait, to prevent a "bottelnek"
                        if (AnholtAirport.storage.Count > 25)
                        {
                            Monitor.Wait(AnholtAirport.storage);
                        }
                        else
                        {
                            //the forloop get a random number 1 or 2, to simulate the number of luggage pieces 
                            for (int i = 0; i < rdm.Next(1, 3); i++)
                            {
                                //increments the idNumberForLuggage by 1 to get get a new id for the bagages
                                idNumberForLuggage++;

                                //makes a piece of luggage, with the destination, counter Id, and luggage Id
                                Luggage luggage = new Luggage(rdm.Next(1, 3), counterId, idNumberForLuggage);

                                //moves the luggage/obj to the storage/queue
                                AnholtAirport.storage.Enqueue(luggage);
                            }
                        }


                        //output to the debug
                        Debug.WriteLine("Check in: Bagage på vej til sortering!");

                        //the sleep is used as an simulation of the bagage being moved
                        Thread.Sleep(500);

                        //informs other threads that changes has been made to the resource
                        Monitor.PulseAll(AnholtAirport.storage);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Problemer med " + e);
                    }
                    //exit the resource
                }
            }
        }
    }
}
