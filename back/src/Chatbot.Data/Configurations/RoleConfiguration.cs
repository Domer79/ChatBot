using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class RoleConfiguration: IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var b = builder;

            b.ToTable("role");

            b.Property(_ => _.Id)
                .HasColumnName("role_id");

            b.Property(_ => _.Name)
                .IsRequired()
                .HasMaxLength(200);

            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            b.HasKey(_ => _.Id);
        }
    }
}