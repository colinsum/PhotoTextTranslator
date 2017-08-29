using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhotoTextTranslator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakePhoto : ContentPage
    {
        public MediaFile text = null;
        public TakePhoto()
        {            
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            picSentence.Text = "";
            await CrossMedia.Current.Initialize();

            this.text = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                Directory = "sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (this.text != null)
            {
                textPic.Source = ImageSource.FromStream(() =>
                {
                    return this.text.GetStream();
                });
            }
            await GetText(text);
        }

        async Task GetText(MediaFile text)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c42b1041a780481dbb9a828ff64d976b");

            var uri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr?language=unk";

            HttpResponseMessage response;
            
            byte[] byteData = convertToByteArray(text);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    OCRModel model = JsonConvert.DeserializeObject<OCRModel>(responseString);

                    List<regions> regions = model.regions;
                    var sentence = "";

                    foreach (regions r in regions )
                    {
                        List<lines> lines = r.lines;
                        foreach (lines l in lines)
                        {
                            List<words> words = l.words;
                            foreach(words w in words)
                            {
                                sentence += w.text + " ";
                            }
                        }
                    }
                    picSentence.Text = sentence;
                    TranslateManager.createInstance.setSentence(sentence);
                }
            }
        }

        static byte[] convertToByteArray(MediaFile text)
        {
            var stream = text.GetStream();
            BinaryReader binRead = new BinaryReader(stream);
            return binRead.ReadBytes((int)stream.Length);
        }
    }
}