using BotTranslatorUnicorn.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace BotTranslatorUnicorn.Services
{
    public class TranslationService
    {

        public class Language
        {
            private readonly string _translateApiKey = ConfigurationManager.AppSettings["TranslateApiKey"];
            private readonly string _translateUri = ConfigurationManager.AppSettings["TranslateUri"];

            public async Task<string> TranslateText(string text)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _translateApiKey);

                var uri = _translateUri + "?to=pt-br" +
                          "&text=" + System.Net.WebUtility.UrlEncode(text);

                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();
                var content = XElement.Parse(result).Value;

                return $"O que você solicitou tradução: ** { text } **. \n A tradução para Português: **{ content }**";
            }

            public async Task<string> TranslateDescription(string text)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _translateApiKey);

                var uri = _translateUri + "?to=pt-br" +
                          "&text=" + System.Net.WebUtility.UrlEncode(text);

                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();
                var content = XElement.Parse(result).Value;

                return $"{ content }";
            }
        }

        public class ImageVision
        {
            private readonly string _computerVisionApiKey = ConfigurationManager.AppSettings["ComputerVisionApiKey"];
            private readonly string _computerVisionUri = ConfigurationManager.AppSettings["ComputerVisionUri"];

            public async Task<string> DescriptionImage(Uri query)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _computerVisionApiKey);

                HttpResponseMessage response = null;

                var byteData = Encoding.UTF8.GetBytes("{ 'url': '" + query + "' }");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync($"{_computerVisionUri}", content).ConfigureAwait(false);
                }

                var responseString = await response.Content.ReadAsStringAsync();

                var analyze = JsonConvert.DeserializeObject<DescriptionAnalyzeImage>(responseString);

                var captionDescription = analyze.description.captions.FirstOrDefault()?.text;
                var isAdultContent = analyze.adult.isAdultContent;
                var isRacyContent = analyze.adult.isRacyContent;
                var hasAdultContent = "";
                var hasRacyContent = "";


                if (isAdultContent)
                {
                    hasAdultContent = "have";
                }
                else
                {
                    hasAdultContent = "does not have";
                }

                if (isRacyContent)
                {
                    hasRacyContent = "have";
                }
                else
                {
                    hasRacyContent = "does not have";
                }

                if (string.IsNullOrWhiteSpace(captionDescription))
                {
                    return $"Sorry I can't describe this photo. I even don't know if this image ** {hasAdultContent} ** Adult Content and  ** {hasRacyContent} ** Racy Content... I guess i'm blind, Try another photo.";

                }

                return $"I see in this image: ** {captionDescription} **, This image ** {hasAdultContent} ** Adult Content and  **{hasRacyContent}** Racy Content.";
            }
        }

    }
}