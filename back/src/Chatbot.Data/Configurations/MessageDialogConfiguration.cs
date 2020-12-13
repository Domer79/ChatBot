using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class MessageDialogConfiguration: IEntityTypeConfiguration<MessageDialog>
    {
        public void Configure(EntityTypeBuilder<MessageDialog> builder)
        {
            var b = builder;

            b.ToTable("message_dialog");

            b.Property(_ => _.Id)
                .HasColumnName("message_dialog_id");

            b.Property(_ => _.ClientId)
                .HasColumnName("client_id")
                .IsRequired();

            b.Property(_ => _.OperatorId)
                .HasColumnName("operator_id");
            
            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            b.Property(_ => _.DateWork)
                .HasColumnName("date_work");

            b.Property(_ => _.DateCompleted)
                .HasColumnName("date_completed");

            b.HasKey(_ => _.Id);
            b.HasOne(_ => _.Client)
                .WithMany(_ => _.ClientDialogs)
                .HasForeignKey(_ => _.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
            
            b.HasOne(_ => _.Operator)
                .WithMany(_ => _.OperatorDialogs)
                .HasForeignKey(_ => _.OperatorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}