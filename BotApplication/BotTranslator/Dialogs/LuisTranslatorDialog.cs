using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace BotTranslator.Dialogs
{
    [Serializable]
    [LuisModel("<YOUR_LUIS_APP_ID>", "YOUR_SUBSCRIPTION_KEY")]
    public class LuisTranslatorDialog : LuisDialog<object>
    {
 
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não consegui entender a seguinte parte: {result.Query}, podemos voltar o assunto de tradução? Eu me sinto menos inseguro falando sobre isso.");
        }

        [LuisIntent("Cumprimentos")]
        public async Task Greetings(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá! Sou um bot pronto para te ajudar a traduzir de Inglês para Português.");
        }

        [LuisIntent("Sobre")]
        public async Task About(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Meu nome é Bot Translator. Eu sou um bot voltado para traduções, e sou o projeto do Maratona Bots da hi-hi-ray. Como sou um bot em ascensão, solicito-lhe muita paciência comigo.");
        }

        [LuisIntent("Cumprimento")]
        public async Task Cumprimento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá, muito prazer, sou um Bot que vai te auxiliar a escolher o melhor hamburguer.");
        }

        [LuisIntent("Cotacao")]
        public async Task Cotacao(IDialogContext context, LuisResult result)
        {
            var moedas = result.Entities?.Select(e => e.Type);
            await context.PostAsync($"Olá, muito prazer, sou um Bot que vai te auxiliar a escolher o melhor hamburguer {string.Join(",", moedas.ToArray())}");
        }
        
    }
}