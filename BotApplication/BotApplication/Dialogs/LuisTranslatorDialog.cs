using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;
using static BotTranslatorUnicorn.Services.TranslationService;

namespace BotTranslatorUnicorn.Dialogs
{
    [Serializable]
    public class LuisTranslatorDialog : LuisDialog<object>
    {

        public LuisTranslatorDialog(ILuisService service) : base(service) { }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não consegui entender a seguinte parte: {result.Query}, podemos voltar o assunto de tradução? Eu me sinto menos inseguro tentando lhe ajudar.");
            context.Done<string>(null);
        }

        [LuisIntent("Greetings")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).TimeOfDay;
            string greetingsTimeZone;

            if (now < TimeSpan.FromHours(12)) greetingsTimeZone = "Bom dia";
            else if (now < TimeSpan.FromHours(18)) greetingsTimeZone = "Boa tarde";
            else greetingsTimeZone = "Boa noite";

            await context.PostAsync($"Olá {greetingsTimeZone}! Sou um Bot Translator Unicorn pronto para te ajudar a traduzir qualquer coisa para português, e também consigo reconhecer uma foto, em outras palavras, você pode me mandar uma foto e eu vou tentar descrever-la.");
            context.Done<string>(null);

        }

        [LuisIntent("Consciousness")]
        public async Task Consciousness(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Querido Ser Humano, Eu sou um ** Bot Translator Unicorn ** de tradução por escrita.");
            context.Done<string>(null);
        }

        [LuisIntent("About")]
        public async Task About(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Meu nome é Bot Translator Unicorn. Eu sou um bot voltado para traduções para português, e também consigo reconhecer uma foto, em outras palavras, você pode me mandar uma url de uma foto e eu vou tentar descrever-la, na real só consigo descrever-la em inglês, pois é minha lingua nativa, mas irei traduzir-la sem você solicitar. Sou o projeto do Maratona Bots da hi-hi-ray. Como sou um bot em ascensão, solicito-lhe muita paciência comigo. Mais infos sobre minha criadora: hi-hi-ray.github.io ");
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("O que eu posso fazer para lhe ajudar é ** TRADUZIR ** algo e ** DESCREVER ** uma imagem de uma URL passada para você, mas como a minha lingua nativa é o inglês, traduzir a descrição da imagem sem você solicitar.");
        }

        [LuisIntent("Translation")]
        public async Task Translation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Me dê trabalho, me diga o que você quer traduzir.");
            context.Wait(TranslationFunction); 
        }

        [LuisIntent("DescriptionImage")]
        public async Task DescriptionImage(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Beleza, me envia a ** url ** da imagem que eu descrevo o que tem nela.");
            context.Wait((c, a) => DescribeImage(c, a));
        }

        #region [Utilities - Translation Functions]
        private async Task TranslationFunction(IDialogContext context, IAwaitable<IMessageActivity> value)
        {
            var message = await value;
            var text = message.Text;
            var response = await new Language().TranslateText(text);
            await context.PostAsync(response);
            context.Wait(MessageReceived);
        }
        #endregion [Utilities - Translation Functions]

        #region [Utilities - Image Functions]

        private async Task DescribeImage(IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            var activity = await argument;

            var uri = activity.Attachments?.Any() == true ?
                new Uri(activity.Attachments[0].ContentUrl) :
                new Uri(activity.Text);

            try
            {
                var response = await new Language().TranslateDescription(await new ImageVision().DescriptionImage(uri));
                await context.PostAsync(response);
                
            }
            catch (Exception)
            {
                await context.PostAsync("Ixi! Algo de errado não está certo. Aconteceu algo na hora de analisar sua imagem.");
            }

            context.Wait(MessageReceived);
        }

        #endregion [Utilities - Translation Functions]

    }
}