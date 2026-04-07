using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.OwnsMany(u => u.RefreshTokens, action =>
            {
                action.ToTable("RefreshTokens");
                action.WithOwner().HasForeignKey("UserId");
                action.HasKey("UserId", "Token");
                action.Property(t => t.Token)
                      .HasMaxLength(200)
                      .IsRequired();
            });
        }
    }
}
