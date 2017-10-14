using AN.ChatBot.Common;
using AN.ChatBot.Helper;
using AN.ChatBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace AN.ChatBot.Dialogs
{
    [LuisModel(BotConstants.LUIS_APP_ID, BotConstants.LUIS_APP_KEY)]
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        private readonly BuildFormDelegate<LeadForm> SubmitLeadForm;
        private readonly BuildFormDelegate<UserActivityType> SubmitActivityType;

        public LUISDialog(BuildFormDelegate<LeadForm> submitLeadForm, BuildFormDelegate<UserActivityType> submitActivityType)
        {
            SubmitLeadForm = submitLeadForm;
            SubmitActivityType = submitActivityType;
        }
        

        [LuisIntent(BotConstants.INTENT_NONE)]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_NONE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GREET_WELCOME)]
        public async Task GreetWelcome(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GREET_WELCOME);
            var userActivityType = new FormDialog<UserActivityType>(new UserActivityType(), SubmitActivityType, FormOptions.PromptInStart);
            context.Call(userActivityType, CallbackActivity);
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
            var postCode = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_POST_CODE, out postCode);
            var filter = SearchHelper.GetSearchFilter(result, postCode);

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = SearchHelper.GetCardsAttachments(filter);

            await context.PostAsync(reply);
            
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GET_LOWER_PRICE)]
        public async Task GetLowerPrice(IDialogContext context, LuisResult result)
        {
            var leadForm = new FormDialog<LeadForm>(new LeadForm(), SubmitLeadForm, FormOptions.PromptInStart);
            context.Call(leadForm, Callback);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }

        private async Task CallbackActivity(IDialogContext context, IAwaitable<object> result)
        {
            var postCode = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_POST_CODE, out postCode);
            var filter = SearchHelper.GetSearchFilter(null, postCode);

            //Call search
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = SearchHelper.GetCardsAttachments(filter);

            await context.PostAsync(reply);
            
            context.Wait(MessageReceived);
        }

    }
}