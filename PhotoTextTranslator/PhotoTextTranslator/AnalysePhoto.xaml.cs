using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace PhotoTextTranslator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalysePhoto : ContentPage
    {
        Dictionary<string, string> language;
        string action;
        public AnalysePhoto()
        {
            InitializeComponent();
        }

        private async void translateButton_Clicked(object sender, EventArgs e)
        {
            var text = TranslateManager.createInstance.getSentence();
            picSentence.Text = text;
            string langCode = language[action].ToString();
            string token = await getTokenAsync();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var textEncode = WebUtility.UrlEncode(text);
            var uri = "https://api.microsofttranslator.com/V2/Http.svc/Translate?text=" + textEncode + "&to=" + langCode;

            var response = await client.GetStringAsync(uri);
            var xDoc = XDocument.Parse(response);
            var translate = xDoc.Root?.FirstNode.ToString();
            translation.Text = translate;
        }

        private async void languages_Clicked(object sender, EventArgs e)
        {
            
            language = makeDictionary();
            string token = await getTokenAsync();
            string[] langNames = language.Keys.ToArray();
            action = await DisplayActionSheet("Select language", "Cancel", "", langNames);
            if (action != "Cancel") { selected.Text = "Language Selected: " + action; }
            else { selected.Text = ""; }
        }

        private Dictionary<string, string> makeDictionary()
        {
            Dictionary<string, string> languages = new Dictionary<string, string>();
            languages.Add("English", "en");
            languages.Add("Arabic", "ar");
            languages.Add("Bulgarian", "bg");
            languages.Add("Chinese Simplified", "zh-CHS");
            languages.Add("Chinese Traditional", "zh-CHT");
            languages.Add("French", "fr");
            languages.Add("German", "de");
            languages.Add("Greek", "el");
            languages.Add("Hebrew", "he");
            languages.Add("Hindi", "hi");
            languages.Add("Hungarian", "hu");
            languages.Add("Indonesian", "id");
            languages.Add("Italian", "it");
            languages.Add("Japanese", "ja");
            languages.Add("Korean", "ko");
            languages.Add("Spanish", "es");
            return languages;
        }

        public async Task<string> getTokenAsync()
        {
            string token;
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");
                request.Content = new StringContent(String.Empty);
                request.Headers.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", "e891c180ea62477ab3d6136b73e94d24");
                var response = await client.SendAsync(request);
                token = await response.Content.ReadAsStringAsync();
            }
            return token;
        }
    }
}