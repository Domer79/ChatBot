﻿// <auto-generated />
using System;
using Chatbot.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chatbot.Ef.Migrations
{
    [DbContext(typeof(ChatbotContext))]
    [Migration("20210125173507_7")]
    partial class _7
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Chatbot.Model.DataModel.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("message_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<Guid>("MessageDialogId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("message_dialog_id");

                    b.Property<int>("Owner")
                        .HasColumnType("int")
                        .HasColumnName("owner");

                    b.Property<Guid?>("Sender")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("sender");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<DateTime>("Time")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("time")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("MessageDialogId");

                    b.ToTable("message");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.MessageDialog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("message_dialog_id");

                    b.Property<Guid?>("ClientId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("client_id");

                    b.Property<DateTime?>("DateCompleted")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_completed");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime?>("DateWork")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_work");

                    b.Property<int>("DialogStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1)
                        .HasColumnName("dialog_status");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("number")
                        .UseIdentityColumn();

                    b.Property<Guid?>("OperatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("operator_id");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("OperatorId");

                    b.ToTable("message_dialog");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.OperatorLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("operator_log_id");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("action");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid>("OperatorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("operator_id");

                    b.HasKey("Id");

                    b.ToTable("operator_logs");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("Politic")
                        .HasColumnType("int")
                        .HasColumnName("politic");

                    b.HasKey("Id");

                    b.HasIndex("Politic")
                        .IsUnique()
                        .HasDatabaseName("UQ_permission_politic");

                    b.ToTable("permission");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.QuestionResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("question_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("number")
                        .UseIdentityColumn();

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("parent_id");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("question");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("response");

                    b.HasKey("Id");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.HasIndex("Question")
                        .IsUnique();

                    b.ToTable("question_response");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("role");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.RolePermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_id");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("permission_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("role_permission");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Settings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("settings_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("settings");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Token", b =>
                {
                    b.Property<string>("TokenId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("token_id");

                    b.Property<TimeSpan>("AutoExpired")
                        .HasColumnType("time")
                        .HasColumnName("auto_expired");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime>("DateExpired")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_expired");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("TokenId");

                    b.HasIndex("UserId");

                    b.ToTable("token");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<DateTime?>("DateBlocked")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_blocked");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsOperator")
                        .HasColumnType("bit")
                        .HasColumnName("is_operator");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("last_name");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("login");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("middle_name");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("number")
                        .UseIdentityColumn();

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("IsOperator");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("Number")
                        .IsUnique();

                    b.ToTable("user");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("role_id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasColumnName("date_created")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_role");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Message", b =>
                {
                    b.HasOne("Chatbot.Model.DataModel.MessageDialog", "Dialog")
                        .WithMany("Messages")
                        .HasForeignKey("MessageDialogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dialog");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.MessageDialog", b =>
                {
                    b.HasOne("Chatbot.Model.DataModel.User", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("Chatbot.Model.DataModel.User", "Operator")
                        .WithMany("Dialogs")
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Client");

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.QuestionResponse", b =>
                {
                    b.HasOne("Chatbot.Model.DataModel.QuestionResponse", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.RolePermission", b =>
                {
                    b.HasOne("Chatbot.Model.DataModel.Permission", "Permission")
                        .WithMany("PermissionRoles")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chatbot.Model.DataModel.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Token", b =>
                {
                    b.HasOne("Chatbot.Model.DataModel.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.UserRole", b =>
                {
                    b.HasOne("Chatbot.Model.DataModel.Role", "Role")
                        .WithMany("RoleUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Chatbot.Model.DataModel.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.MessageDialog", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Permission", b =>
                {
                    b.Navigation("PermissionRoles");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.QuestionResponse", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("RoleUsers");
                });

            modelBuilder.Entity("Chatbot.Model.DataModel.User", b =>
                {
                    b.Navigation("Dialogs");

                    b.Navigation("Tokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
