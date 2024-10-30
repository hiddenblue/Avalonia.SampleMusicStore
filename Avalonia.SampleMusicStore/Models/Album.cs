using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using iTunesSearch.Library;
using iTunesSearch.Library.Models;

namespace Avalonia.SampleMusicStore.Models;

public class Album
{
    private static iTunesSearchManager s_searchManager = new();
    
    public string Artist { get; set; }
    
    public string Title { get; set; }
    
    public string CoverUrl { get; set; }

    public Album()
    {
        
    }

    public Album(string artist, string title, string coverUrl)
    {
        Artist = artist;
        Title = title;
        CoverUrl = coverUrl;
    }

    public static async Task<IEnumerable<Album>> SearchAsync(string keyword)
    {

        AlbumResult query = await s_searchManager.GetAlbumsAsync(keyword)
            .ConfigureAwait(false);

        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName,
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }
    
    private static HttpClient s_httpClient = new();
    
    private string CachePath => $"./Cache/{Artist}-{Title}";

    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if(File.Exists(CachePath + ".bmp" ))
        {
           return File.OpenRead(CachePath + ".bmp"); 
        }
        byte[] data = await s_httpClient.GetByteArrayAsync(CoverUrl);
        return new MemoryStream(data);
    }

    public async Task SaveAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        using (var fs = File.OpenWrite(CachePath))
        {
            await SaveToStreamAsync(this, fs);
        }
            
    }

    public Stream SaveBitmapStream()
    {
        return File.OpenWrite(CachePath + ".bmp");
    }

    private static async Task SaveToStreamAsync(Album album, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, album).ConfigureAwait(false);
    }

    public static async Task<Album> LoadFromStreamAsync(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }

    public static async Task<IEnumerable<Album>> LoadFromCacheAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }
        
        var results = new List<Album>();
        

        foreach (var file in Directory.EnumerateFiles("./Cache"))
        {
            Console.WriteLine(file);
            
            // 这里是用 判断扩展名非空的跳过了bmp分支
            if(!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;
            
            await using var fs = File.OpenRead(file);
            
            results.Add(await Album.LoadFromStreamAsync(fs).ConfigureAwait(false));
        }
        
        return results;
    }
}
