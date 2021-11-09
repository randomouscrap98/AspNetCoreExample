using AspExample.Models;
using Microsoft.EntityFrameworkCore;

namespace AspExample
{
   public class MyDbContext : DbContext
   {
      public DbSet<Toybox> Toyboxes { get; set; }
      public DbSet<StuffedAnimal> StuffedAnimals { get; set; }

      public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
      {
         //IF the database structure (tables) do not exist to handle this DbContext, create them now. NOTE: if the tables
         //already exist but are old and have outdated columns etc, they will not be updated with this command! You will
         //need to manually update your database after the fact if you change your models!
         this.Database.EnsureCreated();
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         //We store the color as a string! The database engine doesn't "understand" a color. We DON'T need to
         //do this for enums, because they're converted to either an int or a string depending on the configuration.
         modelBuilder.Entity<Toybox>()
             .Property(b => b.Color)
             .HasConversion(
                 c => System.Drawing.ColorTranslator.ToHtml(c),
                 c => System.Drawing.ColorTranslator.FromHtml(c));
      }
   }
}