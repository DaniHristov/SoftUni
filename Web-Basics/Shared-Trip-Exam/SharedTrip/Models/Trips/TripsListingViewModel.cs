using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Models.Trips
{
    public class TripsListingViewModel
    {
        public string Id { get; set; }
        
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string DepartureTime { get; set; }

        public int Seats { get; set; }
    }
}
//                    < th scope = "col" > StartPoint </ th >
 
//                     < th scope = "col" > EndPoint </ th >
  
//                      < th scope = "col" > DepartureTime </ th >
   
//                       < th scope = "col" > Seats </ th >
    
//                        < th scope = "col" > Details </ th >