using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace AhoyMusic.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdicionarMusicaPage : ContentPage
    {
        //private Video video;
        public AdicionarMusicaPage()
        {
            InitializeComponent();
        }

        //private async void btnBuscar_Clicked(object sender, EventArgs e)
        //{
        //    var youtube = new YoutubeClient();
        //    try
        //    {
        //        video = await youtube.Videos.GetAsync(entryLink.Text);

        //        lblNome.Text = video.Title;
        //        lblAutor.Text = video.Author.ChannelTitle;
        //        imgThumb.Source = video.Thumbnails.FirstOrDefault(thumb => thumb.Resolution.Width > 400).Url;
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Erro", ex.Message, "OK");
        //    }
        //}

        //private async void btnBaixar_Clicked(object sender, EventArgs e)
        //{
        //    var youtube = new YoutubeClient();
        //    var webClient = new WebClient();
        //    RepositorioDeMusicas rep = new RepositorioDeMusicas();

        //    if (video == null)
        //    {
        //        await DisplayAlert("Erro", "Video não encontrado", "OK");
        //        return;
        //    }

        //    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        //    var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        //    var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
        //    var memoryStream = new MemoryStream();
        //    await stream.CopyToAsync(memoryStream);

        //    var thumbnailUrl = video.Thumbnails.FirstOrDefault(thumb => thumb.Resolution.Width > 400).Url;
        //    byte[] thumbnail = webClient.DownloadData(new Uri(thumbnailUrl));

        //    Musica objMusica = new Musica()
        //    {
        //        Nome = video.Title,
        //        Autor = video.Author.ChannelTitle,
        //        Duracao = video.Duration.Value.TotalSeconds,
        //        Visualizacoes = video.Engagement.ViewCount,
        //        Likes = video.Engagement.LikeCount,
        //        Thumbnail = thumbnail,
        //        Audio = memoryStream.ToArray()
        //    };

        //    rep.Add(objMusica);

        //    video = null;
        //    lblNome.Text = String.Empty;
        //    lblAutor.Text = String.Empty;
        //    imgThumb.Source = String.Empty;
        //}
    }
}