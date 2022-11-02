﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BlazorApp1.Models;

namespace BlazorApp1.Data
{
    public partial class CapstoneDBContext : DbContext
    {
        public CapstoneDBContext()
        {
        }

        public CapstoneDBContext(DbContextOptions<CapstoneDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateTime).HasColumnType("smalldatetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.RandomNum).HasMaxLength(6);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(2);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.Property(e => e.Zip)
                    .HasMaxLength(5)
                    .HasColumnName("ZIP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}