using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<ProjectCreationWizardTemplateCategory> Categories { get; set; }
    public DbSet<ProjectCreationWizardTemplate> Templates { get; set; }
    public DbSet<WorkTypeItem> WorkTypes { get; set; }
    public DbSet<TemplateFeature> TemplateFeatures { get; set; }
    public DbSet<DetailsConfig> DetailsConfigs { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectCreationWizardTemplate>()
            .OwnsOne(t => t.CardConfig);

        modelBuilder.Entity<ProjectCreationWizardTemplate>()
            .HasOne(t => t.DetailsConfig)
            .WithOne()
            .HasForeignKey<ProjectCreationWizardTemplate>(t => t.DetailsConfigId);


        modelBuilder.Entity<WorkTypeItem>()
            .HasOne(w => w.DetailsConfig)
            .WithMany(d => d.WorkTypes)
            .HasForeignKey(w => w.DetailsConfigId);

        modelBuilder.Entity<TemplateFeature>()
            .HasOne(t => t.DetailsConfig)
            .WithMany(d => d.TemplateFeatures)
            .HasForeignKey(t => t.DetailsConfigId);
    }
}
