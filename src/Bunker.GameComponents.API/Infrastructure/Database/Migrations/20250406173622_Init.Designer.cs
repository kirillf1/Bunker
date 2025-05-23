﻿// <auto-generated />
using System;
using Bunker.GameComponents.API.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bunker.GameComponents.API.Migrations
{
    [DbContext(typeof(GameComponentsContext))]
    [Migration("20250406173622_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("game_components")
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.BunkerComponents.BunkerItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_bunker_items");

                    b.ToTable("bunker_items", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.BunkerComponents.EnvironmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_bunker_environments");

                    b.ToTable("bunker_environments", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.BunkerComponents.RoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_bunker_rooms");

                    b.ToTable("bunker_rooms", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CatastropheComponents.CatastropheEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_catastrophes");

                    b.ToTable("catastrophes", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.AdditionalInformationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_additional_information_entitles");

                    b.ToTable("additional_information_entitles", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.Cards.CardEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CardAction")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("card_action");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_cards");

                    b.ToTable("cards", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.HealthEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_health_entitles");

                    b.ToTable("health_entitles", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.HobbyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_hobbies");

                    b.ToTable("hobbies", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.ItemEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_items");

                    b.ToTable("items", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.PhobiaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_phobias");

                    b.ToTable("phobias", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.ProfessionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_professions");

                    b.ToTable("professions", "game_components");
                });

            modelBuilder.Entity("Bunker.GameComponents.API.Entities.CharacterComponents.TraitEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.HasKey("Id")
                        .HasName("pk_traits");

                    b.ToTable("traits", "game_components");
                });
#pragma warning restore 612, 618
        }
    }
}
