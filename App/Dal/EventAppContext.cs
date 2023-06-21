using EventApp.App.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventApp.App.Dal;

public class EventAppContext : DbContext
{
	public EventAppContext(DbContextOptions<EventAppContext> options) : base(options)
	{

	}

	public DbSet<Dal.Entities.Event> Events { get; set; }
	public DbSet<Dal.Entities.Place> Places { get; set; }
	public DbSet<Dal.Entities.Notification> Notifications { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseLazyLoadingProxies();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// add relationship between Event and Place based on PlaceId 
		modelBuilder.Entity<Event>()
			.HasOne(e => e.Place)
			.WithMany()
			.IsRequired(false)
			.HasForeignKey(e => e.PlaceId);
	}

}
