
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RameneToi.Models;


namespace RameneToi.Data
{
    public class RameneToiWebAPIContext : DbContext
    {
        public RameneToiWebAPIContext(DbContextOptions<RameneToiWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<RameneToi.Models.Adresse> Adresses { get; set; } = default!;
        public DbSet<RameneToi.Models.Composant> Composants { get; set; } = default!;
        public DbSet<RameneToi.Models.ConfigurationPc> ConfigurationPcs { get; set; } = default!;
        public DbSet<RameneToi.Models.Commande> Commandes { get; set; } = default!;
        public DbSet<RameneToi.Models.Utilisateurs> Utilisateurs { get; set; } = default!;


    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         

            // Utilisateur 1–n ConfigurationPc 
            modelBuilder.Entity<ConfigurationPc>()
                .HasOne(cp => cp.Utilisateur)
                .WithMany(u => u.Configurations)
                .HasForeignKey(cp => cp.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Utilisateur 1–n Commande
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Commandes)
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict); // ou NoAction

            // ConfigurationPc 1–1 Commande : pas de cascade
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.ConfigurationPc)
                .WithOne(cp => cp.Commande)
                .HasForeignKey<Commande>(c => c.ConfigurationPcId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Many–many ConfigurationPc <-> Composant via table "est_composé_de"
            modelBuilder.Entity<ConfigurationPc>()
                .HasMany(cp => cp.Composants)
                .WithMany(c => c.Configurations)
                .UsingEntity(j =>
                    j.ToTable("est_composé_de") 
                );

            modelBuilder.Entity<Utilisateurs>()
        .HasOne(u => u.Adresse)
        .WithMany(a => a.Utilisateur)
        .HasForeignKey(u => u.AdresseId) // propriété FK sur Utilisateurs
        .HasConstraintName("FK_Adresses_Utilisateurs");
        }
    }
    }
