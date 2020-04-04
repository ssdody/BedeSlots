using BedeSlots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BedeSlots.Data.Configurations
{
    class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                        .HasOne(t => t.User)
                        .WithMany(u => u.Transactions)
                        .HasForeignKey(u => u.UserId);

            var transactionTypeConverter = new EnumToStringConverter<TransactionType>();

            builder
                .Property(t => t.Type)
                .HasConversion(transactionTypeConverter);
        }
    }
}
