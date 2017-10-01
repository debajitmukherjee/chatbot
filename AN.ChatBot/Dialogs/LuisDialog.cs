using AN.ChatBot.Common;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AN.ChatBot.Dialogs
{
    [LuisModel(BotConstants.LUIS_APP_ID, BotConstants.LUIS_APP_KEY)]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {

        [LuisIntent(BotConstants.INTENT_NONE)]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_NONE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GREET_WELCOME)]
        public async Task GreetWelcome(IDialogContext context, LuisResult result)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            var actions = new List<CardAction>();
            actions.Add(new CardAction { Type = ActionTypes.ImBack, Title = BotConstants.BUTTON_BUY, Text = BotConstants.BUTTON_BUY });
            actions.Add(new CardAction { Type = ActionTypes.ImBack, Title = BotConstants.BUTTON_SELL, Text = BotConstants.BUTTON_SELL });

            reply.Attachments.Add(
                new HeroCard
                {
                    Title = BotConstants.DIALOG_GREET_HELP,
                    //Subtitle = BotConstants.DIALOG_GREET_HELP,
                    Buttons = actions
                }.ToAttachment()
            );
            await context.PostAsync(BotConstants.DIALOG_GREET_WELCOME);
            await context.PostAsync(reply);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GREET_FAREWELL)]
        public async Task GreetFarewell(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GREET_FAREWELL);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_SEARCH_CAR)]
        public async Task SearchCar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Here is the list of cars.");
            context.Wait(MessageReceived);
        }


    }
}