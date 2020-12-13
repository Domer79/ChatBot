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
            
            b
                .HasMany(_ => _.Permissions)
                .WithMany(_ => _.Roles)
                .UsingEntity<RolePermission>(
                    j => j
                        .HasOne(_ => _.Permission)
                        .WithMany(_ => _.PermissionRoles)
                        .HasForeignKey(_ => _.PermissionId),
                    j => j
                        .HasOne(_ => _.Role)
                        .WithMany(_ => _.RolePermissions)
                        .HasForeignKey(_ => _.RoleId)
                    );
        }
    }
}