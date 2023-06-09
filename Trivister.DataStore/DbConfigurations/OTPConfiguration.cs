using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class OTPConfiguration: IEntityTypeConfiguration<OTPStore>
{
    public void Configure(EntityTypeBuilder<OTPStore> builder)
    {
        builder.Property(x => x.OTP).HasColumnType("nvarchar(10)").IsRequired();
        builder.Property(x => x.Email).HasColumnType("nvarchar(400)").IsRequired();
        builder.Property(x => x.PhoneNumber).HasColumnType("nvarchar(400)").IsRequired(false);
    }
}