using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var b = builder;
            
            b.ToTable("user");
            
            b.Property(_ => _.Id)
                .HasColumnName("user_id");

            b.Property(_ => _.Login)
                .HasColumnName("login")
                .IsRequired();

            b.Property(_ => _.Email)
                .HasColumnName("email")
                .IsRequired();

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

            b.Property(_ => _.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);
            
            b.HasKey(_ => _.Id);

            b
                .HasMany(_ => _.Roles)
                .WithMany(_ => _.Users)
                .UsingEntity<UserRole>(
                    j => j
                        .HasOne(_ => _.Role)
                        .WithMany(_ => _.RoleUsers)
                        .HasForeignKey(_ => _.RoleId),
                    j => j
                        .HasOne(_ => _.User)
                        .WithMany(_ => _.UserRoles)
                        .HasForeignKey(_ => _.UserId)
                );

            b.HasIndex(_ => _.Login).IsUnique();
            b.HasIndex(_ => _.Email).IsUnique();
        }
    }
}