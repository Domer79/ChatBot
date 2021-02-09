using Chatbot.Ef.Migrations;
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
            
            b.Property(_ => _.Number)
                .HasColumnName("number")
                .IsRequired()
                .ValueGeneratedOnAdd();

            b.Property(_ => _.Login)
                .HasColumnName("login")
                .IsRequired();

            b.Property(_ => _.Email)
                .HasColumnName("email");

            b.Property(_ => _.Phone)
                .HasColumnName("phone");

            b.Property(_ => _.Fio)
                .HasColumnName("fio")
                .HasMaxLength(500);

            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property(_ => _.DateBlocked)
                .HasColumnName("date_blocked");

            b.Property(_ => _.Password)
                .HasColumnName("password")
                .IsRequired();

            b.Property(_ => _.IsActive)
                .HasColumnName("is_active");

            b.Property(_ => _.IsOperator)
                .HasColumnName("is_operator");
            
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
            b.HasIndex(_ => _.Email);
            b.HasIndex(_ => _.Phone);
            b.HasIndex(_ => _.Fio);
            b.HasIndex(_ => _.IsOperator);
            b.HasIndex(_ => _.Number).IsUnique();
        }
    }
}