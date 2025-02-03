using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


public class Teacher
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TeacherId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [MaxLength(10)]
    public string Gender { get; set; }

    [MaxLength(100)]
    public string Subject { get; set; }

    public virtual ICollection<Pupil> Pupils { get; set; } = new HashSet<Pupil>();
}

public class Pupil
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PupilId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [MaxLength(10)]
    public string Gender { get; set; }

    [MaxLength(20)]
    public string Class { get; set; }

    public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
}


public class SchoolContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Pupil> Pupils { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Teacher>()
            .HasMany(t => t.Pupils)
            .WithMany(p => p.Teachers)
            .UsingEntity<Dictionary<string, object>>(
                "TeacherPupil",
                j => j.HasOne<Pupil>().WithMany().HasForeignKey("PupilId"),
                j => j.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId"),
                j =>
                {
                    j.ToTable("TeacherPupil");

                    j.Property("TeacherId").HasColumnName("teacherid");
                    j.Property("PupilId").HasColumnName("pupilid");
                });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}


public class SchoolRepository
{
    public Teacher[] GetAllTeachersByStudent(string studentName)
    {
        using var context = new SchoolContext();
        return context.Teachers

            .Where(t => t.Pupils.Any(p =>
                p.FirstName.Equals(studentName, StringComparison.OrdinalIgnoreCase)))
            .ToArray();
    }
}

public class Program
{
    public static void Main()
    {

        using (var context = new SchoolContext())
        {
            context.Database.EnsureCreated();
        }


        var repository = new SchoolRepository();
        var giorgisTeachers = repository.GetAllTeachersByStudent("გიორგი");


        foreach (var teacher in giorgisTeachers)
        {
            Console.WriteLine($"{teacher.FirstName} {teacher.LastName} teaches {teacher.Subject}");
        }
    }
}