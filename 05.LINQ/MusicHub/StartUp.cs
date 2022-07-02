namespace MusicHub
{
    using Data;
    using Initializer;
    using MusicHub.Data.Models;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            //Console.WriteLine(ExportAlbumsInfo(context, 9)); //T02. Albums Info

            Console.WriteLine(ExportSongsAboveDuration(context, 4)); //T03. Songs Above Duration

        }

        //T02. Albums Info
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {

            Producer producer = context.Producers.Find(producerId);
            var albumsOfProducer = producer.Albums.Select(x => new
            {
                AlbumName = x.Name,
                ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                ProducerName = x.Producer.Name,
                AlbumPrice = x.Price,
                Songs = x.Songs.Select(x => new
                {
                    SongName = x.Name,
                    Price = x.Price,
                    Writer = x.Writer.Name,
                })
                .OrderByDescending(x => x.SongName).ThenBy(x => x.Writer).ToList()
            })
           .OrderByDescending(x => x.AlbumPrice).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albumsOfProducer)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}" + Environment.NewLine +
                    $"-ReleaseDate: {album.ReleaseDate}" + Environment.NewLine +
                    $"-ProducerName: {album.ProducerName}" + Environment.NewLine +
                    "-Songs:");
                int countSongs = 1;
                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{countSongs++}" + Environment.NewLine +
                        $"---SongName: {song.SongName}" + Environment.NewLine +
                        $"---Price: {song.Price:f2}" + Environment.NewLine +
                        $"---Writer: {song.Writer}");
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //T03. Songs Above Duration
        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs.ToList().Where(x => x.Duration.TotalSeconds > duration).Select(x => new
            {
                SongName = x.Name,
                Writer = x.Writer.Name,
                Performer = x.SongPerformers.Select(x => x.Performer.FirstName + " " + x.Performer.LastName).FirstOrDefault(),
                AlbumProducer = x.Album.Producer.Name,
                Duration = x.Duration
            });

            StringBuilder sb = new StringBuilder();

            int songsCount = 1;
            foreach (var song in songs.OrderBy(x => x.SongName).ThenBy(x => x.Writer).ThenBy(x => x.Performer))
            {
                sb.AppendLine($"-Song #{songsCount++}" + Environment.NewLine +
                    $"---SongName: {song.SongName}" + Environment.NewLine +
                    $"---Writer: {song.Writer}" + Environment.NewLine +
                    $"---Performer: {song.Performer}" + Environment.NewLine +
                    $"---AlbumProducer: {song.AlbumProducer}" + Environment.NewLine +
                    $"---Duration: {song.Duration:c}");
                //$"---Duration: {song.Duration.ToString("c")}");

            }
            return sb.ToString().TrimEnd();
        }
    }
}
