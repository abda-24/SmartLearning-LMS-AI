using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Bio)
                .HasMaxLength(1000);

            builder.Property(i => i.Specialty)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Rating)
                .HasColumnType("decimal(3,2)");

            builder.HasOne<ApplicationUser>(i => i.User)
                .WithOne(u => u.InstructorProfile)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.Courses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}