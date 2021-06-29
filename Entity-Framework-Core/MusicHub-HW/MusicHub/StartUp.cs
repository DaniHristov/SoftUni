namespace MusicHub
{
    using System;

    using Data;
    using Initializer;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using MusicHub.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context = 
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));

        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var album = context.Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(x => new
            {
                name = x.Name,
                releaseDate = x.ReleaseDate,
                producerName = x.Producer.Name,
                albumSongs = x.Songs.Select(x => new
                {
                    songName = x.Name,
                    songPrice = x.Price,
                    writerName = x.Writer.Name

                })
                  .OrderByDescending(x => x.songName).ThenBy(x => x.writerName).ToList(),
                  albumPrice = x.Price

            }).OrderByDescending(x=>x.albumPrice).ToList();

            var sb = new StringBuilder();
            var counter = 0;
            foreach (var alb in album)
            {
                sb.AppendLine($"-AlbumName: {alb.name}");
                sb.AppendLine($"-ReleaseDate: {alb.releaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {alb.producerName}");
                sb.AppendLine($"-Songs:");
                foreach (var song in alb.albumSongs)
                {
                    counter++;
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {song.songName}");
                    sb.AppendLine($"---Price: {song.songPrice:F2}");
                    sb.AppendLine($"---Writer: {song.writerName}");
                }
                counter = 0;
                sb.AppendLine($"-AlbumPrice: {alb.albumPrice:F2}");
               


            }

            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(x => x.Duration.TotalSeconds > duration)
                .Select(x => new
                      {
                          songName = x.Name,
                          performer = x.SongPerformers.Select(x => x.Performer.FirstName + " " + x.Performer.LastName).FirstOrDefault(),
                          writerName = x.Writer.Name,
                          albumProducerName = x.Album.Producer.Name,
                          duration = x.Duration.ToString("c"),
                      })
                .OrderBy(x => x.songName)
            .ThenBy(x => x.writerName)
            .ThenBy(x => x.performer)
            .ToList();

            var sb = new StringBuilder();
            var counter = 0;
            foreach (var song in songs) 
            {
                counter++;
                sb.AppendLine($"-Song #{counter}")
                    .AppendLine($"---SongName: {song.songName}")
                    .AppendLine($"---Writer: {song.writerName}")
                    .AppendLine($"---Performer: {song.performer}")
                    .AppendLine($"---AlbumProducer: {song.albumProducerName}")
                    .AppendLine($"---Duration: {song.duration:c}");
                
            }



            return sb.ToString().Trim();
        }
    }
}
