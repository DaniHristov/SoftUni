namespace SoftJail.Data
{
	using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
	{
		public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

        public DbSet<Cell> Cells { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }
        public DbSet<Prisoner> Prisoners { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<OfficerPrisoner>().HasKey(x => new
			{
				x.PrisonerId,
				x.OfficerId
			});

			builder.Entity<OfficerPrisoner>()
				.HasOne(bc => bc.Prisoner)
				.WithMany(b => b.PrisonerOfficers)
				.HasForeignKey(bc => bc.PrisonerId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Entity<OfficerPrisoner>()
				.HasOne(bc => bc.Officer)
				.WithMany(c => c.OfficerPrisoners)
				.HasForeignKey(bc => bc.OfficerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}