using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Data.Configurations
{
    public class MessageConfiguration: IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("message");

            builder.Property(_ => _.Id)
                .HasColumnName("message_id");

            builder.Property(_ => _.Content)
                .HasColumnName("content")
                .IsRequired();

            builder.Property(_ => _.Type)
                .HasColumnName("type")
                .IsRequired();

            builder.Property(_ => _.Owner)
                .HasColumnName("owner")
                .IsRequired();

            builder.Property(_ => _.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.Property(_ => _.Time)
                .HasColumnName("time")
                .IsRequired();

            builder.Property(_ => _.MessageDialogId)
                .HasColumnName("message_dialog_id")
                .IsRequired();

            builder.HasKey(_ => _.Id);
            builder.HasOne(_ => _.Dialog)
                .WithMany(_ => _.Messages)
                .HasForeignKey(_ => _.MessageDialogId);
        }
    }
}