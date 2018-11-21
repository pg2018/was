﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAssistedSurvey.Service.Models;

namespace WebAssistedSurvey.Service.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20181119102327_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("WebAssistedSurvey.Service.Models.WebEvent", b =>
                {
                    b.Property<int>("WebEventID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("EndDateTime");

                    b.Property<bool>("IsMultidays");

                    b.Property<DateTime>("StartDateTime");

                    b.Property<string>("Summery");

                    b.Property<string>("Title");

                    b.HasKey("WebEventID");

                    b.ToTable("WebEvents");
                });

            modelBuilder.Entity("WebAssistedSurvey.Service.Models.WebSurvey", b =>
                {
                    b.Property<int>("WebSurveyID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BadGuy");

                    b.Property<string>("ContactEmail");

                    b.Property<string>("ContactName");

                    b.Property<DateTime>("Created");

                    b.Property<string>("Feedback");

                    b.Property<string>("GoodGuy");

                    b.Property<string>("Source");

                    b.Property<bool>("WantNewsletter");

                    b.Property<int>("WebEventID");

                    b.HasKey("WebSurveyID");

                    b.HasIndex("WebEventID");

                    b.ToTable("WebSurveys");
                });

            modelBuilder.Entity("WebAssistedSurvey.Service.Models.WebSurvey", b =>
                {
                    b.HasOne("WebAssistedSurvey.Service.Models.WebEvent")
                        .WithMany("WebSurveys")
                        .HasForeignKey("WebEventID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}