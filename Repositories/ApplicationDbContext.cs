using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet.Repositories;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }

    private static readonly (Type EntityType, object[] Data)[] _seedData = [];

    private static void ApplySeeds(ModelBuilder modelBuilder, (Type EntityType, object[] Data)[] seeds)
    {
        if (seeds == null || seeds.Length == 0) return;

        foreach (var (entityType, data) in seeds)
        {
            var entityMethod = typeof(ModelBuilder)
                .GetMethods()
                .First(m => m.Name == nameof(ModelBuilder.Entity) && m.IsGenericMethod && m.GetParameters().Length == 0)
                .MakeGenericMethod(entityType);

            var entityBuilder = entityMethod.Invoke(modelBuilder, null)!;

            var typedArray = Array.CreateInstance(entityType, data.Length);
            Array.Copy(data, typedArray, data.Length);

            var hasDataMethod = entityBuilder
                .GetType()
                .GetMethods()
                .First(m => m.Name == nameof(EntityTypeBuilder<object>.HasData) && m.GetParameters().Length == 1);

            hasDataMethod.Invoke(entityBuilder, [typedArray]);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;
            var method = typeof(ApplicationDbContext)
                .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.MakeGenericMethod(entityType.ClrType);

            method?.Invoke(null, [modelBuilder]);
        }

        modelBuilder.Entity<Todo>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType.IsEnum)
                {
                    property.SetColumnType("varchar(50)");
                    property.SetMaxLength(50);
                    var converterType = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                    var converter = Activator.CreateInstance(converterType);
                    property.SetValueConverter((ValueConverter)converter!);
                }
            }
        }

        ApplySeeds(modelBuilder, _seedData);
    }

    public override int SaveChanges()
    {
        Stamp();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        Stamp();
        return base.SaveChangesAsync(ct);
    }

    private void Stamp()
    {
        var now = DateTime.UtcNow;
        foreach (var e in ChangeTracker.Entries<BaseEntity>())
        {
            if (e.State == EntityState.Added)
            {
                e.Entity.CreatedAt = now;
                e.Entity.UpdatedAt = now;
            }
            else if (e.State == EntityState.Modified)
            {
                e.Entity.UpdatedAt = now;
            }
        }
    }

    private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : BaseEntity
    {
        builder.Entity<T>().HasQueryFilter(x => x.DeletedAt == null);
    }
}
