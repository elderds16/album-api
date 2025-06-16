using Microsoft.EntityFrameworkCore;

namespace Album.Api.Data;

public static class DbInitializer
{
    public static void Initialize(AlbumContext context)
    {
        context.Database.Migrate();

        if (context.Albums.Any())
        {
            return;
        }

        var albums = new Models.Album[]
        {
            new () { Id = Guid.NewGuid(), Name = "Thriller", Artist = "Michael Jackson", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/5/55/Michael_Jackson_-_Thriller.png"},
            new () { Id = Guid.NewGuid(), Name = "Back in Black", Artist = "AC/DC", ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/be/Acdc_backinblack_cover.jpg" },
            new () { Id = Guid.NewGuid(), Name = "The Dark Side of the Moon", Artist = "Pink Floyd", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/3b/Dark_Side_of_the_Moon.png" },
            new () { Id = Guid.NewGuid(), Name = "The Bodyguard", Artist = "Whitney Houston", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/0/03/Whitney_Houston_-_The_Bodyguard.png" },
            new () { Id = Guid.NewGuid(), Name = "Rumours", Artist = "Fleetwood Mac", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/f/fb/FMacRumours.PNG" },
            new () { Id = Guid.NewGuid(), Name = "Saturday Night Fever", Artist = "Bee Gees", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/0/0c/TheBeeGeesSaturdayNightFeveralbumcover.jpg" },
            new () { Id = Guid.NewGuid(), Name = "Hotel California", Artist = "Eagles", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/49/Hotelcalifornia.jpg" },
            new () { Id = Guid.NewGuid(), Name = "21", Artist = "Adele", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/1/1b/Adele_-_21.png" },
            new () { Id = Guid.NewGuid(), Name = "Abbey Road", Artist = "The Beatles", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/4/42/Beatles_-_Abbey_Road.jpg" },
            new () { Id = Guid.NewGuid(), Name = "Born in the U.S.A.", Artist = "Bruce Springsteen", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/3/31/BruceBorn1984.JPG" }
        };

        foreach (var album in albums)
        {
            context.Albums.Add(album);
        }

        context.SaveChanges();
    }
}

