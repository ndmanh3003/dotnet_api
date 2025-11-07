using Microsoft.EntityFrameworkCore;
using dotnet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dotnet.Enums.Course;

namespace dotnet.Repositories;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Major> Majors { get; set; }

    private static readonly (Type EntityType, object[] Data)[] _seedData =
    [
        (typeof(Major), new object[]
            {
                new Major { Id = 1, Code = "CNPM", Name = "Công nghệ phần mềm", IsActive = true },
                new Major { Id = 2, Code = "HTTT", Name = "Hệ thống thông tin", IsActive = true },
                new Major { Id = 3, Code = "MMT", Name = "Mạng máy tính", IsActive = true },
            }),

        (typeof(Course), new object[]
            {
                new Course
                {
                    Id = 1,
                    Code = "CS101",
                    Name = "Nhập môn lập trình",
                    Credits = 3,
                    Type = CourseType.Compulsory,
                    TheoryHours = 30,
                    PracticeHours = 15,
                    ExerciseHours = 15
                },
                new Course
                {
                    Id = 2,
                    Code = "CS201",
                    Name = "Cấu trúc dữ liệu và giải thuật",
                    Credits = 3,
                    Type = CourseType.Compulsory,
                    TheoryHours = 30,
                    PracticeHours = 15,
                    ExerciseHours = 15
                },
                new Course
                {
                    Id = 3,
                    Code = "CS301",
                    Name = "Kỹ năng mềm",
                    Credits = 4,
                    Type = CourseType.Optional,
                    TheoryHours = 30,
                    PracticeHours = 30,
                    ExerciseHours = 15
                },
            }),
    ];

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

            hasDataMethod.Invoke(entityBuilder, new object[] { typedArray });
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

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Major)
            .WithMany()
            .HasForeignKey(s => s.MajorCode)
            .HasPrincipalKey(m => m.Code);

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
