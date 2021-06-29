namespace Cinema.Data.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ProjectionId { get; set; }
        public Projection Projection { get; set; }
    }
}
//•	Id – integer, Primary Key
//•	Price – decimal (non-negative, minimum value: 0.01) (required)
//•	CustomerId – integer, Foreign key(required)
//•	Customer – the Ticket’s Customer 
//•	ProjectionId – integer, Foreign key (required)
//•	Projection – the Ticket’s Projection
