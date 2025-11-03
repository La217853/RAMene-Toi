
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
            modelBuilder.Entity<Utilisateurs>()
                 .HasOne(u => u.Adresse)
                 .WithMany(a => a.utilisateur)
                 .HasForeignKey(u => u.AdresseId);

            modelBuilder.Entity<Utilisateurs>()
                .HasMany(u => u.ConfigurationsPc)
                .WithOne(c => c.Utilisateur)
                .HasForeignKey(c => c.UtilisateurId);

            modelBuilder.Entity<ConfigurationPc>()
                .HasOne(c => c.Commande)
                .WithOne(cmd => cmd.ConfigurationPc)
                .HasForeignKey<Commande>(cmd => cmd.ConfigurationPcId);

            modelBuilder.Entity<Utilisateurs>()
                .HasMany(u => u.Commandes)
                .WithOne(c => c.Utilisateur)
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConfigurationPc>()
                .HasMany(cp => cp.Composants)
                .WithMany(c => c.Configurations)
                .UsingEntity(j =>
                    j.ToTable("est_composé_de") 
                );


        }
    }
}
    