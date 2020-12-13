﻿using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class PermissionConfiguration: IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            var b = builder;

            b.ToTable("permission");

            b.Property(_ => _.Id)
                .HasColumnName("id");

            b.Property(_ => _.Politic)
                .HasColumnName("politic")
                .IsRequired();
            
            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            b.HasKey(_ => _.Id);
            b.HasIndex(_ => _.Politic)
                .HasDatabaseName("UQ_permission_politic");
        }
    }
}