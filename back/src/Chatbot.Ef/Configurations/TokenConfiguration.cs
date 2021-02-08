using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Configurations
{
    public class TokenConfiguration: IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            var b = builder;

            b.Property(_ => _.TokenId)
                .HasColumnName("token_id")
                .HasMaxLength(450);
            
            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property(_ => _.DateExpired)
                .HasColumnName("date_expired");

            b.Property(_ => _.AutoExpired)
                .HasColumnName("auto_expired");

            b.Property(_ => _.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            b.ToTable("token");
            b.HasKey(_ => _.TokenId);
            b.HasOne(_ => _.User)
                .WithMany(_ => _.Tokens)
                .HasForeignKey(_ => _.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}