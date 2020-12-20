using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Configurations
{
    public class OperatorLogConfiguration: IEntityTypeConfiguration<OperatorLog>
    {
        public void Configure(EntityTypeBuilder<OperatorLog> builder)
        {
            var b = builder;

            b.Property(_ => _.Id)
                .HasColumnName("operator_log_id");

            b.Property(_ => _.OperatorId)
                .HasColumnName("operator_id")
                .IsRequired();

            b.Property(_ => _.Action)
                .HasColumnName("action")
                .IsRequired();
            
            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            b.HasKey(_ => _.Id);
        }
    }
}