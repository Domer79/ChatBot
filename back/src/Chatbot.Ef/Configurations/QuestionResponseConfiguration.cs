using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Configurations
{
    public class QuestionResponseConfiguration: IEntityTypeConfiguration<QuestionResponse>
    {
        public void Configure(EntityTypeBuilder<QuestionResponse> builder)
        {
            var b = builder;

            b.Property(_ => _.Id)
                .HasColumnName("question_id");

            b.Property(_ => _.Number)
                .HasColumnName("number")
                .IsRequired()
                .ValueGeneratedOnAdd();

            b.Property(_ => _.Question)
                .HasColumnName("question")
                .HasMaxLength(800);

            b.Property(_ => _.Response)
                .HasColumnName("response");

            b.Property(_ => _.ParentId)
                .HasColumnName("parent_id")
                .IsRequired(false);

            b.Property(_ => _.DateCreated)
                .HasColumnName("date_created")
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property(_ => _.SortOrder)
                .HasColumnName("sort_order");

            b.ToTable("question_response");
            b.HasKey(_ => _.Id);
            b.HasIndex(_ => _.Question);
            b.HasIndex(_ => _.Number).IsUnique();
            b.HasIndex(_ => _.SortOrder);
                
            b.HasOne(_ => _.Parent)
                .WithMany(_ => _.Children)
                .HasForeignKey(_ => _.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}