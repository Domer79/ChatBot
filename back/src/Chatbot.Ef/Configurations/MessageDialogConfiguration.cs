using Chatbot.Model.DataModel;
using Chatbot.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Configurations
{
    public class MessageDialogConfiguration: IEntityTypeConfiguration<MessageDialog>
    {
        public void Configure(EntityTypeBuilder<MessageDialog> builder)
        {
            var b = builder;

            b.ToTable("message_dialog");

            b.Property(_ => _.Id)
                .HasColumnName("message_dialog_id");
            
            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            b.Property(_ => _.DateWork)
                .HasColumnName("date_work");

            b.Property(_ => _.DateCompleted)
                .HasColumnName("date_completed");

            b.Property(_ => _.DialogStatus)
                .HasColumnName("dialog_status")
                .HasDefaultValue(DialogStatus.Started);

            b.Property(_ => _.OperatorId)
                .HasColumnName("operator_id");

            b.HasKey(_ => _.Id);

            b.HasOne(_ => _.Operator)
                .WithMany(_ => _.Dialogs)
                .HasForeignKey(_ => _.OperatorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}