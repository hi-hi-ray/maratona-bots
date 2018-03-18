using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Threading.Tasks;

namespace BotTranslatorUnicorn.Dialogs
{
    [Serializable]
    [LuisModel("<YOUR_LUIS_APP_ID>", "YOUR_SUBSCRIPTION_KEY")]
    public class LuisTranslatorDialog : LuisDialog<object>
    {

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

            await context.PostAsync($"Olá {greetingsTimeZone} ! Sou um Bot Translator Unicorn pronto para te ajudar a traduzir qualquer coisa para português, e também consigo reconhecer uma foto, em outras palavras, você pode me mandar uma foto e eu vou tentar descrever-la.");
            context.Done<string>(null);

        }

        [LuisIntent("About")]
        public async Task About(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Meu nome é Bot Translator Unicorn. Eu sou um bot voltado para traduções para português, e também consigo reconhecer uma foto, em outras palavras, você pode me mandar uma foto e eu vou tentar descrever-la, na real eu só consigo descrever-la em inglês, pois é minha lingua nativa, mas irei traduzir-la sem você solicitar. Sou o projeto do Maratona Bots da hi-hi-ray. Como sou um bot em ascensão, solicito-lhe muita paciência comigo. mais infos sobre minha criadora: hi-hi-ray.github.io ");
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Eita! Muita calma nessa hora! O que eu posso fazer é ** TRADUZIR ** algo e ** DESCREVER ** uma imagem para ti, como a minha lingua nativa é o inglês, traduzir-la sem você solicitar");
        }

        [LuisIntent("Translation")]
        public async Task Translation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Meu nome é Bot Translator. Eu sou um bot voltado para traduções, e sou o projeto do Maratona Bots da hi-hi-ray. Como sou um bot em ascensão, solicito-lhe muita paciência comigo.");
        }

        [LuisIntent("DescriptionImage")]
        public async Task DescriptionImage(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Meu nome é Bot Translator. Eu sou um bot voltado para traduções, e sou o projeto do Maratona Bots da hi-hi-ray. Como sou um bot em ascensão, solicito-lhe muita paciência comigo.");
        }
    }
}