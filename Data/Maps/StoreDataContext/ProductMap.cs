using AuthenticationProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationProject.Data.Maps.StoreDataContext;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(1024).HasColumnType("varchar(1024)").IsRequired();
        builder.Property(x => x.Price).HasColumnType("money").IsRequired();
        builder.HasOne(x => x.Category).WithMany(x => x.Products);
    }
}
