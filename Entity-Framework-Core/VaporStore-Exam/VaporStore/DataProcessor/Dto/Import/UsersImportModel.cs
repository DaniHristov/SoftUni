using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UsersImportModel
    {
        [RegularExpression("[A-Z][a-z]{2,} [A-Z][a-z]{2,}")]
        [Required]
        public string FullName { get; set; }

        [StringLength(20,MinimumLength =3)]
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Range(3,103)]
        public int Age { get; set; }
        public CardInputModel[] Cards { get; set; }
    }

    public class CardInputModel
    {
        [Required]
        [RegularExpression(@"[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}")]
        public string Number { get; set; }

        [RegularExpression(@"[0-9]{3}")]
        [Required]
        public string CVC { get; set; }
        [Required]  
        public CardType? Type { get; set; }
    }

}
//User
//•	Id – integer, Primary Key
//•	Username – text with length [3, 20] (required)
//•	FullName – text, which has two words, consisting of Latin letters. Both start with an upper letter and are followed by lower letters. The two words are separated by a single space (ex. "John Smith") (required)
//•	Email – text(required)
//•	Age – integer in the range[3, 103] (required)
//•	Cards – collection of type Card
