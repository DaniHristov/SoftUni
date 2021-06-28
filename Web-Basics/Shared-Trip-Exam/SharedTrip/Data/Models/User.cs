using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Data.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(20)]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<UserTrip> UserTrips { get; set; } = new List<UserTrip>();

    }
}
//•	Has an Id – a string, Primary Key
//•	Has a Username – a string with min length 5 and max length 20 (required)
//•	Has an Email - a string (required)
//•	Has a Password – a string with min length 6 and max length 20  - hashed in the database (required)
//•	Has UserTrips collection – a UserTrip type
