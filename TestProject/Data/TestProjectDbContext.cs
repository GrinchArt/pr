using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestProject.Data.Configuration;
using TestProject.Models;

    public class TestProjectDbContext : DbContext
    {
        public TestProjectDbContext (DbContextOptions<TestProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestProject.Models.TestModel> TestModel { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TestModelConfiguration());
    }
}
