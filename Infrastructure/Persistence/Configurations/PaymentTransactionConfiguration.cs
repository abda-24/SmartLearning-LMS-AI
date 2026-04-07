using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Status)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasOne(p => p.Student)
                   .WithMany() 
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Course)
                   .WithMany()
                   .HasForeignKey(p => p.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}