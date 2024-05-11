using Microsoft.EntityFrameworkCore;

namespace lab78.Pages;


public class Genre
{
    public Genre(
        int id,
        string genre_name
    )
    {
        this.id = id;
        this.genre_name = genre_name;
    }
    public int id { get; set; }
    public string genre_name { get; set; }
}
public class Publisher
{
    public Publisher(
        int id,
        string publisher_name
    )
    {
        this.id = id;
        this.publisher_name = publisher_name;
    }
    public int id { get; set; }
    public string publisher_name { get; set; }
}
public class GamePublisher
{
    public GamePublisher(
        int id,
        int game_id,
        int publisher_id
    )
    {
        this.id = id;
        this.game_id = game_id;
        this.publisher_id = publisher_id;
    }
    public int id { get; set; }
    public int game_id { get; set; }
    public int publisher_id { get; set; }
}
public class GamePlatform
{
    public GamePlatform(
        int id,
        int game_publisher_id,
        int platform_id,
        int release_year
    )
    {
        this.id = id;
        this.game_publisher_id = game_publisher_id;
        this.platform_id = platform_id;
        this.release_year = release_year;
    }
    public int id { get; set; }
    public int game_publisher_id { get; set; }
    public int platform_id { get; set; }
    public int release_year { get; set; }
}

/// <summary>
///  Схемы - https://www.databasestar.com/sample-database-video-games/
/// </summary>
public class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=kostya;Username=kostya;Password=;");
    }


    public DbSet<Publisher> publisher { get; set; }
    public DbSet<GamePublisher> game_publisher { get; set; }
    public DbSet<GamePlatform> game_platform { get; set; }
    public DbSet<Game> game { get; set; }
    public DbSet<Genre> genre { get; set; }

}