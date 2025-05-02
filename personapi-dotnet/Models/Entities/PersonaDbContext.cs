using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Models.Entities 
{
    public partial class PersonaDbContext : DbContext
    {
        // Constructor(s) remain the same
        public PersonaDbContext() { }

        public PersonaDbContext(DbContextOptions<PersonaDbContext> options) : base(options) { }

        // DbSets remain the same
        public virtual DbSet<Estudio> Estudios { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Profesion> Profesions { get; set; } // Note: Consider renaming to Profesiones for consistency
        public virtual DbSet<Telefono> Telefonos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estudio>(entity =>
            {
                // Existing key and table config
                entity.HasKey(e => new { e.IdProf, e.CcPer }).HasName("PK__estudios__FB3F71A6223C4A76");
                entity.ToTable("estudios");

                // Existing property configs
                entity.Property(e => e.IdProf).HasColumnName("id_prof");
                entity.Property(e => e.CcPer).HasColumnName("cc_per");
                entity.Property(e => e.Fecha).HasColumnName("fecha"); // Consider using DateOnly mapping if needed
                entity.Property(e => e.Univer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("univer");

                // --- Configure RELATIONSHIPS for Estudio ---

                // Estudio -> Persona: CASCADE DELETE
                entity.HasOne(d => d.CcPerNavigation).WithMany(p => p.Estudios)
                    .HasForeignKey(d => d.CcPer)
                    .OnDelete(DeleteBehavior.Cascade) // <<< MODIFIED
                    .HasConstraintName("FK__estudios__cc_per__3F466844"); // Keep existing constraint name if possible

                // Estudio -> Profesion: CASCADE DELETE
                entity.HasOne(d => d.IdProfNavigation).WithMany(p => p.Estudios)
                    .HasForeignKey(d => d.IdProf)
                    .OnDelete(DeleteBehavior.Cascade) // <<< MODIFIED
                    .HasConstraintName("FK__estudios__id_pro__3E52440B"); // Keep existing constraint name if possible
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                // Existing key and table config
                entity.HasKey(e => e.Cc).HasName("PK__persona__3213E83FC333E091");
                entity.ToTable("persona");

                // Existing property configs
                entity.Property(e => e.Cc)
                    .ValueGeneratedNever()
                    .HasColumnName("cc");
                entity.Property(e => e.Apellido)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("apellido");
                entity.Property(e => e.Edad).HasColumnName("edad");
                entity.Property(e => e.Genero)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("genero");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Profesion>(entity =>
            {
                // Existing key and table config
                entity.HasKey(e => e.Id).HasName("PK__profesio__3213E83F50854D8C");
                entity.ToTable("profesion");

                // Existing property configs
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Des)
                    .IsUnicode(false)
                    .HasColumnName("des");
                entity.Property(e => e.Nom)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                // Existing key and table config
                entity.HasKey(e => e.Num).HasName("PK__telefono__DF908D65F944ADC4");
                entity.ToTable("telefono");

                // Existing property configs
                entity.Property(e => e.Num)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("num");
                entity.Property(e => e.Duenio).HasColumnName("duenio");
                entity.Property(e => e.Oper)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("oper");

                // --- Configure RELATIONSHIP for Telefono ---

                // Telefono -> Persona: CASCADE DELETE
                entity.HasOne(d => d.DuenioNavigation).WithMany(p => p.Telefonos)
                    .HasForeignKey(d => d.Duenio)
                    .OnDelete(DeleteBehavior.Cascade) // <<< MODIFIED
                    .HasConstraintName("FK__telefono__duenio__3B75D760"); // Keep existing constraint name if possible
            });

            // Call partial method if it exists (often used with scaffolding)
            OnModelCreatingPartial(modelBuilder);
        }

        // Add this partial method declaration if it doesn't exist
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}