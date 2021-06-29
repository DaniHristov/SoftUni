using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportJsonMailsModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression("[0-9]+ [A-z ]+str.")]
        public string Address { get; set; }
    }
}