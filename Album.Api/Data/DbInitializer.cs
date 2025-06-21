using Microsoft.EntityFrameworkCore;
using Album.Api.Models; 

namespace Album.Api.Data;

public static class DbInitializer
{
    public static void Initialize(AlbumContext context)
    {
        if (context.Database.IsRelational())
        {
            context.Database.Migrate();
        }

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

        context.Albums.AddRange(albums);
        context.SaveChanges();

        // Koppel albumnamen aan hun ID's
        var albumsByName = albums.ToDictionary(a => a.Name);

        // Voeg 5 echte tracks toe per album
        var tracks = new List<Track>
        {
            //new() { Title = "Wanna Be Startin' Somethin'", Artist = "Michael Jackson", Duration = 362, AlbumId = albumsByName["Thriller"].Id },
            //new() { Title = "Baby Be Mine", Artist = "Michael Jackson", Duration = 257, AlbumId = albumsByName["Thriller"].Id },
            //new() { Title = "The Girl Is Mine", Artist = "Michael Jackson", Duration = 225, AlbumId = albumsByName["Thriller"].Id },
            //new() { Title = "Thriller", Artist = "Michael Jackson", Duration = 358, AlbumId = albumsByName["Thriller"].Id },
            //new() { Title = "Beat It", Artist = "Michael Jackson", Duration = 258, AlbumId = albumsByName["Thriller"].Id },

            //new() { Title = "Hells Bells", Artist = "AC/DC", Duration = 312, AlbumId = albumsByName["Back in Black"].Id },
            //new() { Title = "Shoot to Thrill", Artist = "AC/DC", Duration = 317, AlbumId = albumsByName["Back in Black"].Id },
            //new() { Title = "Back in Black", Artist = "AC/DC", Duration = 255, AlbumId = albumsByName["Back in Black"].Id },
            //new() { Title = "You Shook Me All Night Long", Artist = "AC/DC", Duration = 210, AlbumId = albumsByName["Back in Black"].Id },
            //new() { Title = "Rock and Roll Ain't Noise Pollution", Artist = "AC/DC", Duration = 251, AlbumId = albumsByName["Back in Black"].Id },

            //new() { Title = "Speak to Me", Artist = "Pink Floyd", Duration = 90, AlbumId = albumsByName["The Dark Side of the Moon"].Id },
            //new() { Title = "Breathe", Artist = "Pink Floyd", Duration = 163, AlbumId = albumsByName["The Dark Side of the Moon"].Id },
            //new() { Title = "Time", Artist = "Pink Floyd", Duration = 412, AlbumId = albumsByName["The Dark Side of the Moon"].Id },
            //new() { Title = "Money", Artist = "Pink Floyd", Duration = 382, AlbumId = albumsByName["The Dark Side of the Moon"].Id },
            //new() { Title = "Us and Them", Artist = "Pink Floyd", Duration = 462, AlbumId = albumsByName["The Dark Side of the Moon"].Id },

            //new() { Title = "I Will Always Love You", Artist = "Whitney Houston", Duration = 273, AlbumId = albumsByName["The Bodyguard"].Id },
            //new() { Title = "I Have Nothing", Artist = "Whitney Houston", Duration = 272, AlbumId = albumsByName["The Bodyguard"].Id },
            //new() { Title = "I'm Every Woman", Artist = "Whitney Houston", Duration = 270, AlbumId = albumsByName["The Bodyguard"].Id },
            //new() { Title = "Run to You", Artist = "Whitney Houston", Duration = 254, AlbumId = albumsByName["The Bodyguard"].Id },
            //new() { Title = "Queen of the Night", Artist = "Whitney Houston", Duration = 234, AlbumId = albumsByName["The Bodyguard"].Id },

            //new() { Title = "Dreams", Artist = "Fleetwood Mac", Duration = 257, AlbumId = albumsByName["Rumours"].Id },
            //new() { Title = "Don't Stop", Artist = "Fleetwood Mac", Duration = 219, AlbumId = albumsByName["Rumours"].Id },
            //new() { Title = "Go Your Own Way", Artist = "Fleetwood Mac", Duration = 223, AlbumId = albumsByName["Rumours"].Id },
            //new() { Title = "The Chain", Artist = "Fleetwood Mac", Duration = 273, AlbumId = albumsByName["Rumours"].Id },
            //new() { Title = "You Make Loving Fun", Artist = "Fleetwood Mac", Duration = 225, AlbumId = albumsByName["Rumours"].Id },

            //new() { Title = "Stayin' Alive", Artist = "Bee Gees", Duration = 285, AlbumId = albumsByName["Saturday Night Fever"].Id },
            //new() { Title = "Night Fever", Artist = "Bee Gees", Duration = 202, AlbumId = albumsByName["Saturday Night Fever"].Id },
            //new() { Title = "More Than a Woman", Artist = "Bee Gees", Duration = 208, AlbumId = albumsByName["Saturday Night Fever"].Id },
            //new() { Title = "Jive Talkin'", Artist = "Bee Gees", Duration = 222, AlbumId = albumsByName["Saturday Night Fever"].Id },
            //new() { Title = "If I Can't Have You", Artist = "Bee Gees", Duration = 190, AlbumId = albumsByName["Saturday Night Fever"].Id },

            //new() { Title = "Hotel California", Artist = "Eagles", Duration = 391, AlbumId = albumsByName["Hotel California"].Id },
            //new() { Title = "New Kid in Town", Artist = "Eagles", Duration = 302, AlbumId = albumsByName["Hotel California"].Id },
            //new() { Title = "Life in the Fast Lane", Artist = "Eagles", Duration = 285, AlbumId = albumsByName["Hotel California"].Id },
            //new() { Title = "Wasted Time", Artist = "Eagles", Duration = 295, AlbumId = albumsByName["Hotel California"].Id },
            //new() { Title = "Victim of Love", Artist = "Eagles", Duration = 236, AlbumId = albumsByName["Hotel California"].Id },

            //new() { Title = "Rolling in the Deep", Artist = "Adele", Duration = 228, AlbumId = albumsByName["21"].Id },
            //new() { Title = "Rumour Has It", Artist = "Adele", Duration = 223, AlbumId = albumsByName["21"].Id },
            //new() { Title = "Turning Tables", Artist = "Adele", Duration = 251, AlbumId = albumsByName["21"].Id },
            //new() { Title = "Set Fire to the Rain", Artist = "Adele", Duration = 242, AlbumId = albumsByName["21"].Id },
            //new() { Title = "Someone Like You", Artist = "Adele", Duration = 285, AlbumId = albumsByName["21"].Id },

            //new() { Title = "Come Together", Artist = "The Beatles", Duration = 259, AlbumId = albumsByName["Abbey Road"].Id },
            //new() { Title = "Something", Artist = "The Beatles", Duration = 182, AlbumId = albumsByName["Abbey Road"].Id },
            //new() { Title = "Maxwell's Silver Hammer", Artist = "The Beatles", Duration = 207, AlbumId = albumsByName["Abbey Road"].Id },
            //new() { Title = "Oh! Darling", Artist = "The Beatles", Duration = 208, AlbumId = albumsByName["Abbey Road"].Id },
            //new() { Title = "Here Comes the Sun", Artist = "The Beatles", Duration = 185, AlbumId = albumsByName["Abbey Road"].Id },

            //new() { Title = "Born in the U.S.A.", Artist = "Bruce Springsteen", Duration = 279, AlbumId = albumsByName["Born in the U.S.A."].Id },
            //new() { Title = "Cover Me", Artist = "Bruce Springsteen", Duration = 231, AlbumId = albumsByName["Born in the U.S.A."].Id },
            //new() { Title = "Darlington County", Artist = "Bruce Springsteen", Duration = 260, AlbumId = albumsByName["Born in the U.S.A."].Id },
            //new() { Title = "I'm on Fire", Artist = "Bruce Springsteen", Duration = 160, AlbumId = albumsByName["Born in the U.S.A."].Id },
            new() { Title = "Glory Days", Artist = "Bruce Springsteen", Duration = 290, AlbumId = albumsByName["Born in the U.S.A."].Id }
        };
        context.Tracks.AddRange(tracks);
        context.SaveChanges();


    }
}
