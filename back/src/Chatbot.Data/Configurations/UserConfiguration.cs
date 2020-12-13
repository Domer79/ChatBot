using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var b = builder;
            
            b.ToTable("user");
            
            b.Property(_ => _.Id)
                .HasColumnName("user_id");

            b.Property(_ => _.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(500);

            b.Property(_ => _.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(500);

            b.Property(_ => _.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(500);

            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            b.Property(_ => _.Password)
                .HasColumnName("password")
                .IsRequired();
            
            b.HasKey(_ => _.Id);
        }
    }
}