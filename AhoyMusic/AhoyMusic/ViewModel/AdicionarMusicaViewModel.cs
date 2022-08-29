using AhoyMusic.Models;
using AhoyMusic.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace AhoyMusic.ViewModel
{
    public class AdicionarMusicaViewModel : BindableObject
    {
        readonly YoutubeClient youtubeClient;

        readonly WebClient webClient;

        readonly RepositorioDeMusicas repMusicas;


        private Video _objVideo;

        public Video objVideo
        {
            get => _objVideo;
            set
            {
                _objVideo = value;
                OnPropertyChanged();
            }
        }

        private LayoutOptions _mainVerticalLayoutOptions;

        public LayoutOptions mainVerticalLayoutOptions
        {
            get => _mainVerticalLayoutOptions;
            set
            {
                _mainVerticalLayoutOptions = value;
                OnPropertyChanged();
            }
        }

        private string _entryText;

        public string entryText
        {
            get => _entryText;
            set
            {
                _entryText = value;
                OnPropertyChanged();
            }
        }

        private bool _isFrameVisible;

        public bool isFrameVisible
        {
            get => _isFrameVisible;
            set
            {
                _isFrameVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _isRunning;

        public bool isRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _thumbSource;

        public ImageSource thumbSource
        {
            get => _thumbSource;
            set
            {
                _thumbSource = value;
                OnPropertyChanged();
            }
        }


        public ICommand buscarVideo { get; private set; }

        public ICommand baixarMusica { get; private set; }

        public AdicionarMusicaViewModel()
        {
            youtubeClient = new YoutubeClient();
            webClient = new WebClient();
            repMusicas = new RepositorioDeMusicas();

            mainVerticalLayoutOptions = LayoutOptions.CenterAndExpand;
            entryText = String.Empty;
            isFrameVisible = false;
            isRunning = false;

            buscarVideo = new Command(BuscarVideo);
            baixarMusica = new Command(BaixarMusica);
        }

        private async void BuscarVideo()
        {
            isRunning = true;
            await Task.Delay(1);
            try
            {
                objVideo = await youtubeClient.Videos.GetAsync(entryText);
                thumbSource = objVideo.Thumbnails.FirstOrDefault(thumb => thumb.Resolution.Width > 400).Url;
                isRunning = false;
                isFrameVisible = true;
            }
            catch (Exception ex)
            {
                isRunning = false;
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void BaixarMusica()
        {
            RepositorioDeMusicas rep = new RepositorioDeMusicas();
            isRunning = true;
            if (objVideo == null)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Video não encontrado", "OK");
                return;
            }

            var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(objVideo.Id);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            var stream = await youtubeClient.Videos.Streams.GetAsync(streamInfo);
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            var thumbnailUrl = objVideo.Thumbnails.FirstOrDefault(thumb => thumb.Resolution.Width > 400).Url;
            byte[] thumbnail = webClient.DownloadData(new Uri(thumbnailUrl));

            Musica objMusica = new Musica()
            {
                Nome = objVideo.Title,
                Autor = objVideo.Author.ChannelTitle,
                Duracao = objVideo.Duration.Value.TotalSeconds,
                Visualizacoes = objVideo.Engagement.ViewCount,
                Likes = objVideo.Engagement.LikeCount,
                Thumbnail = thumbnail,
                Audio = memoryStream.ToArray()
            };

            rep.Add(objMusica);

            entryText = String.Empty;
            isRunning = false;
            isFrameVisible = false;
            objVideo = null;

            var mainPageViewModel = ((NavigationPage)App.Current.MainPage).RootPage.BindingContext as MusicasViewModel;
            mainPageViewModel.MinhasMusicas = rep.GetAll();

        }




    }
}
