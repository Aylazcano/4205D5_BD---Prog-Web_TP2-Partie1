using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using _4204D5_labo10.Models;

namespace _4204D5_labo10.Data
{
    public partial class Lab10Context : DbContext
    {
        public Lab10Context()
        {
        }

        public Lab10Context(DbContextOptions<Lab10Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Changelog> Changelogs { get; set; } = null!;
        public virtual DbSet<Chanson> Chansons { get; set; } = null!;
        public virtual DbSet<Chanteur> Chanteurs { get; set; } = null!;
        public virtual DbSet<VwChanteurNbChanson> VwChanteurNbChansons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Lab10");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Changelog>(entity =>
            {
                entity.Property(e => e.InstalledOn).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Chanson>(entity =>
            {
                entity.HasOne(d => d.Chanteur)
                    .WithMany(p => p.Chansons)
                    .HasForeignKey(d => d.ChanteurId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Chanson_ChanteurID");
            });

            modelBuilder.Entity<VwChanteurNbChanson>(entity =>
            {
                entity.ToView("VW_ChanteurNbChansons", "Musique");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
