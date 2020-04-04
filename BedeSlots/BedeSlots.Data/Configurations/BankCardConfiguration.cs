using BedeSlots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BedeSlots.Data.Configurations
{
    internal class BankCardConfiguration : IEntityTypeConfiguration<BankCard>
    {
        public void Configure(EntityTypeBuilder<BankCard> builder)
        {
            builder
               .HasOne(c => c.User)
               .WithMany(u => u.Cards)
               .HasForeignKey(u => u.UserId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
