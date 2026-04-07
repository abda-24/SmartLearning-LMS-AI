using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.ToTable("Wishlists");
            builder.HasIndex(w => new { w.StudentId, w.CourseId }).IsUnique();

            builder.HasOne(w => w.Student)
                   .WithMany()
                   .HasForeignKey(w => w.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.Course)
                   .WithMany()
                   .HasForeignKey(w => w.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}