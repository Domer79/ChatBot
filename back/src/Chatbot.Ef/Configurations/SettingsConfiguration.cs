using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Configurations
{
    public class SettingsConfiguration: IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            var b = builder;

            b.ToTable("settings");
            b.Property(_ => _.Id)
                .HasColumnName("settings_id");
            
            b.Property(_ => _.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            b.Property(_ => _.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            b.Property(_ => _.Value)
                .HasColumnName("value");

            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            b.HasKey(_ => _.Id);
            b.HasIndex(_ => _.Name).IsUnique();
            b.HasIndex(_ => _.Description);
        }
    }
}