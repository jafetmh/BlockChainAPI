﻿// <auto-generated />
using System;
using BlockChain_DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlockChain_DB.Migrations
{
    [DbContext(typeof(BlockChainContext))]
    partial class BlockChainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlockChain_DB.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Attempts")
                        .HasColumnType("int");

                    b.Property<int>("ChainID")
                        .HasColumnType("int");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Milliseconds")
                        .HasColumnType("int");

                    b.Property<DateTime>("MiningDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Previous_Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChainID");

                    b.ToTable("blocks", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.Chain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("chains", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.Document", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("BlockID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Doc_encode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BlockID");

                    b.ToTable("documents", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.MemPool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("mempools", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.MemPoolDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Doc_encode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MemPoolID")
                        .HasColumnType("int");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MemPoolID");

                    b.ToTable("mempool_documents", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.SystemConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("system_configs", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("BlockChain_DB.Block", b =>
                {
                    b.HasOne("BlockChain_DB.Chain", "Chain")
                        .WithMany("Blocks")
                        .HasForeignKey("ChainID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chain");
                });

            modelBuilder.Entity("BlockChain_DB.Chain", b =>
                {
                    b.HasOne("BlockChain_DB.User", "User")
                        .WithOne("Chain")
                        .HasForeignKey("BlockChain_DB.Chain", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlockChain_DB.Document", b =>
                {
                    b.HasOne("BlockChain_DB.Block", "Block")
                        .WithMany("Documents")
                        .HasForeignKey("BlockID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Block");
                });

            modelBuilder.Entity("BlockChain_DB.MemPool", b =>
                {
                    b.HasOne("BlockChain_DB.User", "User")
                        .WithOne("MemPool")
                        .HasForeignKey("BlockChain_DB.MemPool", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlockChain_DB.MemPoolDocument", b =>
                {
                    b.HasOne("BlockChain_DB.MemPool", "MemPool")
                        .WithMany("Documents")
                        .HasForeignKey("MemPoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MemPool");
                });

            modelBuilder.Entity("BlockChain_DB.Block", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("BlockChain_DB.Chain", b =>
                {
                    b.Navigation("Blocks");
                });

            modelBuilder.Entity("BlockChain_DB.MemPool", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("BlockChain_DB.User", b =>
                {
                    b.Navigation("Chain");

                    b.Navigation("MemPool");
                });
#pragma warning restore 612, 618
        }
    }
}
