﻿// <auto-generated />
using System;
using CondigiBack.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CondigiBack.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240813024644_contract_participant_update")]
    partial class contract_participant_update
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CondigiBack.Models.AIRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("ai_requests");
                });

            modelBuilder.Entity("CondigiBack.Models.Canton", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("cantons");
                });

            modelBuilder.Entity("CondigiBack.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("companies");
                });

            modelBuilder.Entity("CondigiBack.Models.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<Guid>("ContractTypeId")
                        .HasColumnType("uuid")
                        .HasColumnName("contract_type_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("created_by");

                    b.Property<string>("EncryptionKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("encryption_key");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<int?>("NumClauses")
                        .HasColumnType("integer")
                        .HasColumnName("num_clauses");

                    b.Property<decimal?>("PaymentAmount")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int?>("PaymentFrequency")
                        .HasColumnType("integer")
                        .HasColumnName("payment_frequency");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("updated_by");

                    b.HasKey("Id");

                    b.HasIndex("ContractTypeId");

                    b.ToTable("contracts");
                });

            modelBuilder.Entity("CondigiBack.Models.ContractParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("CompanyId")
                        .HasColumnType("uuid")
                        .HasColumnName("company_id");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid")
                        .HasColumnName("contract_id");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<bool>("Signed")
                        .HasColumnType("boolean")
                        .HasColumnName("signed");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean")
                        .HasColumnName("status");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContractId");

                    b.HasIndex("UserId");

                    b.ToTable("contract_participants");
                });

            modelBuilder.Entity("CondigiBack.Models.ContractType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("contract_types");
                });

            modelBuilder.Entity("CondigiBack.Models.Parish", b =>
                {
                    b.Property<int>("IdParish")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdParish"));

                    b.Property<int>("CantonId")
                        .HasColumnType("integer");

                    b.Property<string>("NameParish")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdParish");

                    b.HasIndex("CantonId");

                    b.ToTable("parishes");
                });

            modelBuilder.Entity("CondigiBack.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Identification")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParishId")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ParishId");

                    b.ToTable("persons");
                });

            modelBuilder.Entity("CondigiBack.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("provinces");
                });

            modelBuilder.Entity("CondigiBack.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("CondigiBack.Models.UserCompanies", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoleInCompany")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("user_companies");
                });

            modelBuilder.Entity("CondigiBack.Models.AIRequest", b =>
                {
                    b.HasOne("CondigiBack.Models.Contract", "Contract")
                        .WithMany("AiRequests")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("CondigiBack.Models.Canton", b =>
                {
                    b.HasOne("CondigiBack.Models.Province", "Province")
                        .WithMany("Cantons")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("CondigiBack.Models.Contract", b =>
                {
                    b.HasOne("CondigiBack.Models.ContractType", "ContractType")
                        .WithMany("Contracts")
                        .HasForeignKey("ContractTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractType");
                });

            modelBuilder.Entity("CondigiBack.Models.ContractParticipant", b =>
                {
                    b.HasOne("CondigiBack.Models.Company", "Company")
                        .WithMany("ContractParticipants")
                        .HasForeignKey("CompanyId");

                    b.HasOne("CondigiBack.Models.Contract", "Contract")
                        .WithMany("ContractParticipants")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CondigiBack.Models.User", "User")
                        .WithMany("ContractParticipants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Contract");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CondigiBack.Models.Parish", b =>
                {
                    b.HasOne("CondigiBack.Models.Canton", "Canton")
                        .WithMany("Parishes")
                        .HasForeignKey("CantonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Canton");
                });

            modelBuilder.Entity("CondigiBack.Models.Person", b =>
                {
                    b.HasOne("CondigiBack.Models.Parish", "Parish")
                        .WithMany("Persons")
                        .HasForeignKey("ParishId");

                    b.Navigation("Parish");
                });

            modelBuilder.Entity("CondigiBack.Models.User", b =>
                {
                    b.HasOne("CondigiBack.Models.Person", "Person")
                        .WithOne("User")
                        .HasForeignKey("CondigiBack.Models.User", "PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CondigiBack.Models.UserCompanies", b =>
                {
                    b.HasOne("CondigiBack.Models.Company", "Company")
                        .WithMany("UserCompanies")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CondigiBack.Models.User", "User")
                        .WithMany("UserCompanies")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CondigiBack.Models.Canton", b =>
                {
                    b.Navigation("Parishes");
                });

            modelBuilder.Entity("CondigiBack.Models.Company", b =>
                {
                    b.Navigation("ContractParticipants");

                    b.Navigation("UserCompanies");
                });

            modelBuilder.Entity("CondigiBack.Models.Contract", b =>
                {
                    b.Navigation("AiRequests");

                    b.Navigation("ContractParticipants");
                });

            modelBuilder.Entity("CondigiBack.Models.ContractType", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("CondigiBack.Models.Parish", b =>
                {
                    b.Navigation("Persons");
                });

            modelBuilder.Entity("CondigiBack.Models.Person", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("CondigiBack.Models.Province", b =>
                {
                    b.Navigation("Cantons");
                });

            modelBuilder.Entity("CondigiBack.Models.User", b =>
                {
                    b.Navigation("ContractParticipants");

                    b.Navigation("UserCompanies");
                });
#pragma warning restore 612, 618
        }
    }
}
