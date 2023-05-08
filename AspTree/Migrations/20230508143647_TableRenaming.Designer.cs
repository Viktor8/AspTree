﻿// <auto-generated />
using System;
using AspTree.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspTree.Migrations
{
    [DbContext(typeof(DataTreeContext))]
    [Migration("20230508143647_TableRenaming")]
    partial class TableRenaming
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AspTree.Model.DataNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentNodeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentNodeId");

                    b.ToTable("DataNode");
                });

            modelBuilder.Entity("AspTree.Model.DataNode", b =>
                {
                    b.HasOne("AspTree.Model.DataNode", "ParentNode")
                        .WithMany("Children")
                        .HasForeignKey("ParentNodeId");

                    b.Navigation("ParentNode");
                });

            modelBuilder.Entity("AspTree.Model.DataNode", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}