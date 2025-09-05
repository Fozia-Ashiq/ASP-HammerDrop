using Microsoft.EntityFrameworkCore;

namespace HammerDrop_Auction_app.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Listing> Listings { get; set; }
       public DbSet<Product> Products { get; set; }

        public DbSet<Bid> Bids { get; set; }

        public DbSet<Ad> Ads { get; set; }

        public DbSet<AdImages> AdImagess { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }


        public DbSet<Country> Countries  { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product Relationships

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.State)
                .WithMany()
                .HasForeignKey(p => p.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);


            // Ad Relationships

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey(a => a.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.State)
                .WithMany()
                .HasForeignKey(a => a.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ad>()
                .HasOne(a => a.Subcategory)
                .WithMany()
                .HasForeignKey(a => a.SubcategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // Subcategory → Category

            modelBuilder.Entity<Subcategory>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // City Relationships

            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.country_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<City>()
                .HasOne(c => c.State)
                .WithMany()
                .HasForeignKey(c => c.state_id)
                .OnDelete(DeleteBehavior.Restrict);


            // State → Country

            modelBuilder.Entity<State>()
                .HasOne<Country>()
                .WithMany()
                .HasForeignKey(s => s.country_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Ad)
                .WithMany(a => a.Bids)   //reverse navigation
                .HasForeignKey(b => b.AdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.UserAccount)
                .WithMany()                //one way navigation
                .HasForeignKey(b => b.UserAccountId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

