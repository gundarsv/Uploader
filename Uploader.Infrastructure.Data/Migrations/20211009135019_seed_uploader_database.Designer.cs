﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Uploader.Infrastructure.Data.Contexts;

namespace Uploader.Infrastructure.Data.Migrations
{
    [DbContext(typeof(UploaderContext))]
    [Migration("20211009135019_seed_uploader_database")]
    partial class seed_uploader_database
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Uploader.Infrastructure.Data.Entities.UploaderFileExtension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileExtension")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UploaderFileExtensions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "4c6b6cbb-50a7-42c4-bb81-b5e2ec0647ee",
                            FileExtension = "jpg"
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "8a9b8fcd-2ce0-4777-9a9b-22816a481296",
                            FileExtension = "png"
                        });
                });

            modelBuilder.Entity("Uploader.Infrastructure.Data.Entities.UploaderSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MaxFileSize")
                        .HasColumnType("bigint");

                    b.Property<int>("MaxHeight")
                        .HasColumnType("int");

                    b.Property<int>("MaxWidth")
                        .HasColumnType("int");

                    b.Property<int>("MinHeight")
                        .HasColumnType("int");

                    b.Property<int>("MinWidth")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UploaderSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "89662660-274c-48e4-bcae-b49f2ad37792",
                            MaxFileSize = 50L,
                            MaxHeight = 2000,
                            MaxWidth = 2000,
                            MinHeight = 200,
                            MinWidth = 200
                        });
                });

            modelBuilder.Entity("UploaderFileExtensionUploaderSettings", b =>
                {
                    b.Property<int>("AllowedFileExtensionsId")
                        .HasColumnType("int");

                    b.Property<int>("UploaderSettingsId")
                        .HasColumnType("int");

                    b.HasKey("AllowedFileExtensionsId", "UploaderSettingsId");

                    b.HasIndex("UploaderSettingsId");

                    b.ToTable("UploaderFileExtensionUploaderSettings");
                });

            modelBuilder.Entity("UploaderFileExtensionUploaderSettings", b =>
                {
                    b.HasOne("Uploader.Infrastructure.Data.Entities.UploaderFileExtension", null)
                        .WithMany()
                        .HasForeignKey("AllowedFileExtensionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Uploader.Infrastructure.Data.Entities.UploaderSettings", null)
                        .WithMany()
                        .HasForeignKey("UploaderSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}