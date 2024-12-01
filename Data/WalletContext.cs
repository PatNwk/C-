using Microsoft.EntityFrameworkCore;
using MonWebApp.Models;

public class WalletContext : DbContext
{
    // Déclaration des DbSet pour les entités
    public DbSet<Asset> Assets { get; set; }
    public DbSet<UserAsset> UserAssets { get; set; }
    public DbSet<User> Users { get; set; }

    // Constructeur avec DbContextOptions
    public WalletContext(DbContextOptions<WalletContext> options)
        : base(options)
    {
    }

    // Configuration des entités et relations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de UserAsset
        modelBuilder.Entity<UserAsset>(entity =>
        {
            // Définir UserAssetId comme clé primaire et auto-incrémentée
            entity.HasKey(ua => ua.UserAssetId);
            entity.Property(ua => ua.UserAssetId)
                .ValueGeneratedOnAdd(); // Spécifie que la base génère la valeur

            // Relation entre UserAsset et Asset
            entity.HasOne(ua => ua.Asset)
                .WithMany(a => a.UserAssets)
                .HasForeignKey(ua => ua.AssetId)
                .OnDelete(DeleteBehavior.Cascade); // Supprime les UserAssets liés si Asset est supprimé

            // Relation entre UserAsset et User
            entity.HasOne(ua => ua.User)
                .WithMany(u => u.UserAssets)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Supprime les UserAssets liés si User est supprimé

            // Configuration des colonnes numériques
            entity.Property(ua => ua.Quantity)
                .HasColumnType("decimal(18,2)"); // Quantité décimale avec précision
            entity.Property(ua => ua.TotalPrice)
                .HasColumnType("decimal(18,2)");
        });

        // Configuration de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId); // Clé primaire pour User
            entity.Property(u => u.UserId)
                .ValueGeneratedOnAdd(); // Auto-incrément pour UserId

            // Configuration pour WalletAddress
            entity.Property(u => u.WalletAddress)
                .HasMaxLength(100); // Longueur maximale pour WalletAddress
        });

        // Configuration de Asset
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(a => a.AssetId); // Clé primaire pour Asset
            entity.Property(a => a.AssetId)
                .ValueGeneratedOnAdd(); // Auto-incrément pour AssetId

            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50); // Nom de l'Asset obligatoire avec une longueur maximale
        });
    }
}
