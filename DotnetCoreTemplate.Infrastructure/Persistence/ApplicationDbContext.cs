using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Domain.Entities;
using DotnetCoreTemplate.Infrastructure.Identity;
using DotnetCoreTemplate.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<TodoItem> TodoItems { get; set; }

	public DbSet<RefreshToken> RefreshTokens { get; set; }

	public DbSet<User> UsersView { get; set; }

	public DbSet<Role> RolesView { get; set; }

	public DbSet<UserRole> UserRolesView { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

		modelBuilder.SeedRoles(SecurityRole.Administrator, SecurityRole.Individual);
	}
}