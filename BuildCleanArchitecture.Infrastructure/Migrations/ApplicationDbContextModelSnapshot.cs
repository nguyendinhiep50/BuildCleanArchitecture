﻿// <auto-generated />
using System;
using BuildCleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BuildCleanArchitecture.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BuildCleanArchitecture.Domain.Entities.AuthorBook", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("CreatedBy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedSpanTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid>("UId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("UpdatedBy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedSpanTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("BuildCleanArchitecture.Domain.Entities.Book", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorBookId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CatalogBookId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("CreatedBy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedSpanTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid>("UId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("UpdatedBy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedSpanTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorBookId");

                    b.HasIndex("CatalogBookId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BuildCleanArchitecture.Domain.Entities.CatalogBook", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("CreatedBy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedSpanTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<Guid>("UId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("UpdatedBy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedSpanTime")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CatalogBooks");
                });

            modelBuilder.Entity("BuildCleanArchitecture.Infrastructure.Identity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationRole");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cac43a6e-f7bb-4448-baaf-1add431ccbbf"),
                            Name = "Employee",
                            NormalizedName = "EMPLOYEE"
                        },
                        new
                        {
                            Id = new Guid("cbc43a8e-f7bb-4445-baaf-1add431ffbbf"),
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        });
                });

            modelBuilder.Entity("BuildCleanArchitecture.Infrastructure.Identity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "692d4ddb-f3b0-43fb-aeaf-f9786371c820",
                            CreatedDate = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "admin@localhost.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "System",
                            NormalizedEmail = "ADMIN@LOCALHOST.COM",
                            NormalizedUserName = "ADMIN@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEOnuaqKTFq9fqg1poQJfDEX7O6ctDL9XpqlyH5VYW/39TunqwOo2oJXgGkDJVtXCMQ==",
                            PhoneNumberConfirmed = false,
                            Status = true,
                            TwoFactorEnabled = false,
                            UserName = "admin@localhost.com"
                        },
                        new
                        {
                            Id = new Guid("9e224968-33e4-4652-b7b7-8574d048cdb9"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "9d6ad73a-db4f-4123-bed7-45594704fc26",
                            CreatedDate = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Email = "user@localhost.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            Name = "User",
                            NormalizedEmail = "USER@LOCALHOST.COM",
                            NormalizedUserName = "USER@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEKD9XNQjLjz4UWLuPEX73bBnqOOIvLf8Bl8NXi4ZVQGSwc+4BnjUOuZtvAA6/AYpmw==",
                            PhoneNumberConfirmed = false,
                            Status = true,
                            TwoFactorEnabled = false,
                            UserName = "user@localhost.com"
                        });
                });

            modelBuilder.Entity("BuildCleanArchitecture.Infrastructure.Identity.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationUserRole");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("cbc43a8e-f7bb-4445-baaf-1add431ffbbf"),
                            UserId = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9")
                        },
                        new
                        {
                            RoleId = new Guid("cac43a6e-f7bb-4448-baaf-1add431ccbbf"),
                            UserId = new Guid("9e224968-33e4-4652-b7b7-8574d048cdb9")
                        });
                });

            modelBuilder.Entity("BuildCleanArchitecture.Domain.Entities.Book", b =>
                {
                    b.HasOne("BuildCleanArchitecture.Domain.Entities.AuthorBook", "AuthorBook")
                        .WithMany()
                        .HasForeignKey("AuthorBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BuildCleanArchitecture.Domain.Entities.CatalogBook", "CatalogBook")
                        .WithMany()
                        .HasForeignKey("CatalogBookId");

                    b.Navigation("AuthorBook");

                    b.Navigation("CatalogBook");
                });

            modelBuilder.Entity("BuildCleanArchitecture.Infrastructure.Identity.ApplicationUserRole", b =>
                {
                    b.HasOne("BuildCleanArchitecture.Infrastructure.Identity.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BuildCleanArchitecture.Infrastructure.Identity.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BuildCleanArchitecture.Infrastructure.Identity.ApplicationRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BuildCleanArchitecture.Infrastructure.Identity.ApplicationUser", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
