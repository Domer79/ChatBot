using Chatbot.Model.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatbot.Ef.Statistics.Consfigurations
{
    public class StatConfiguration: IEntityTypeConfiguration<Stat>
    {
        public void Configure(EntityTypeBuilder<Stat> builder)
        {
            builder.ToTable("Stat");

            builder.Property(_ => _.Id)
                .HasColumnName("stat_id");

            builder.Property(_ => _.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(_ => _.QuestionId)
                .HasColumnName("question_id");

            builder.Property(_ => _.Time)
                .HasColumnName("time")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(_ => _.Type)
                .HasColumnName("stat_type")
                .IsRequired();

            builder.HasKey(_ => _.Id);

            builder.HasIndex(_ => _.Time).IsUnique();
            builder.HasIndex(_ => _.UserId);
            builder.HasIndex(_ => _.Type);
            builder.HasIndex(_ => _.QuestionId);
        }
    }
}