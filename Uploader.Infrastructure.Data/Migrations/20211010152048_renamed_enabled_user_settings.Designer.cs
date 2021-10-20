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
    [Migration("20211010152048_renamed_enabled_user_settings")]
    partial class renamed_enabled_user_settings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Uploader.Infrastructure.Data.Entities.EnabledUploaderSettings", b =>
                {
                    b.Property<int>("EnabledSettingsId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EnabledSettingsId");

                    b.ToTable("EnabledUploaderSettings");
                });

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

            modelBuilder.Entity("Uploader.Infrastructure.Data.Entities.EnabledUploaderSettings", b =>
                {
                    b.HasOne("Uploader.Infrastructure.Data.Entities.UploaderSettings", "EnabledSettings")
                        .WithOne("EnabledUserSettings")
                        .HasForeignKey("Uploader.Infrastructure.Data.Entities.EnabledUploaderSettings", "EnabledSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EnabledSettings");
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

            modelBuilder.Entity("Uploader.Infrastructure.Data.Entities.UploaderSettings", b =>
                {
                    b.Navigation("EnabledUserSettings");
                });
#pragma warning restore 612, 618
        }
    }
}
