using AuthenticationProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationProject.Data.Maps.UserDataContext;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ToTable(name: "AspNetUser");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("AspNetUserId");
    }
}
