using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagagesorteringssystem
{
    //the luggage class with 3 int vars with get and set
    public class Luggage
    {
        //feilds
        private int destination;
        private int luggageId;
        private int checktedInBy;

        //Properties 
        public int Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        public int LuggageId
        {
            get { return luggageId; }
            set { luggageId = value; }
        }
        public int ChecktedInBy
        {
            get { return checktedInBy; }
            set { checktedInBy = value; }
        }

        public Luggage(int destination, int checktedInBy, int luggageId)
        {
            this.destination = destination;
            this.checktedInBy = checktedInBy;
            this.luggageId = luggageId;
        }
    }
}
