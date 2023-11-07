﻿// <auto-generated />
using System;
using ClinicaVeterinaria.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicaVeterinaria.Migrations
{
    [DbContext(typeof(Conn))]
    [Migration("20231107194835_Teste6")]
    partial class Teste6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate.Agendamento", b =>
                {
                    b.Property<int>("idAgendamento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idAgendamento"));

                    b.Property<DateOnly>("data")
                        .HasColumnType("date");

                    b.Property<int?>("especie")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("horario")
                        .HasColumnType("time without time zone");

                    b.Property<int?>("idCliente")
                        .HasColumnType("integer");

                    b.Property<int>("nif")
                        .HasColumnType("integer");

                    b.Property<string>("nomeAnimal")
                        .HasColumnType("text");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.HasKey("idAgendamento");

                    b.HasIndex("idCliente");

                    b.HasIndex("nif");

                    b.ToTable("agendamentos");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Domain.Models.ClienteAggregate.Cliente", b =>
                {
                    b.Property<int>("idCliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idCliente"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("senha")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("idCliente");

                    b.ToTable("clientes");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario", b =>
                {
                    b.Property<int>("nif")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("nif"));

                    b.Property<int>("cargo")
                        .HasColumnType("integer");

                    b.Property<string>("nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("senha")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("nif");

                    b.ToTable("funcionarios");
                });

            modelBuilder.Entity("ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate.Agendamento", b =>
                {
                    b.HasOne("ClinicaVeterinaria.Domain.Models.ClienteAggregate.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("idCliente");

                    b.HasOne("ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario", "funcionario")
                        .WithMany()
                        .HasForeignKey("nif")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("funcionario");
                });
#pragma warning restore 612, 618
        }
    }
}
