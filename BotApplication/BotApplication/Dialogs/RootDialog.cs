using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotTranslatorUnicorn.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await context.PostAsync("Wellcome to hi-hi-ray bot!");

            var message = activity.CreateReply();
            var heroCard = new HeroCard();
            heroCard.Title = "Planet";
            heroCard.Subtitle = "Universe";
            heroCard.Images = new List<CardImage> {  new CardImage("https://avatars0.githubusercontent.com/u/9919?s=280&v=4", "logo github") };
            message.Attachments.Add(heroCard.ToAttachment());

            await context.PostAsync(message);


            context.Wait(MessageReceivedAsync);
        }
    }
}