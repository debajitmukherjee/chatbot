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
    public class AutoNationLuisDialog : LuisDialog<object>
    {
        private readonly BuildFormDelegate<LeadForm> SubmitLeadForm;
        private readonly BuildFormDelegate<UserActivityType> SubmitActivityType;

        public AutoNationLuisDialog(BuildFormDelegate<LeadForm> submitLeadForm, BuildFormDelegate<UserActivityType> submitActivityType)
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
            string welcomeMessage = BotConstants.DIALOG_GREET_WELCOME;
            var firstName = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_FIRST_NAME, out firstName);

            if(!string.IsNullOrEmpty(firstName))
            {
                welcomeMessage = welcomeMessage.Replace(BotConstants.DIALOG_FIRST_NAME_TOKEN, firstName);
            }
            else
            {
                welcomeMessage = welcomeMessage.Replace(BotConstants.DIALOG_FIRST_NAME_TOKEN, "");
            }

            await context.PostAsync(welcomeMessage);
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
            bool isKnownUser = SearchHelper.IsKnownUser(context);

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = SearchHelper.GetCardsAttachments(filter, isKnownUser);

            if(reply.Attachments.Count > 0)
            {
                await context.PostAsync(reply);
            }
            else // no results found
            {
                await context.PostAsync(BotConstants.DIALOG_SEARCH_NO_RESULTS);
            }
            
            
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GET_LOWER_PRICE)]
        public async Task GetLowerPrice(IDialogContext context, LuisResult result)
        {
            var leadForm = new LeadForm();

            //Check if value already exists in the context or not
            var firstName = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_FIRST_NAME, out firstName);

            if(!string.IsNullOrEmpty(firstName))
            {
                var lastName = string.Empty;
                var email = string.Empty;
                var phoneNo = string.Empty;
                var prefrredContactMethod = string.Empty;

                context.UserData.TryGetValue(BotConstants.USER_DATA_LAST_NAME, out lastName);
                context.UserData.TryGetValue(BotConstants.USER_DATA_EMAIL, out email);
                context.UserData.TryGetValue(BotConstants.USER_DATA_PHONE, out phoneNo);
                context.UserData.TryGetValue(BotConstants.USER_DATA_PREFERED_CONTACT_METHOD, out prefrredContactMethod);

                leadForm.FirstName = firstName;
                leadForm.LastName = lastName;
                leadForm.Email = email;
                leadForm.PhoneNo = phoneNo;
                leadForm.PrefferedContactOption = ContactOption.Email.ToString() == prefrredContactMethod ? ContactOption.Email : ContactOption.Mobile;

            }

            var leadFormDialog = new FormDialog<LeadForm>(leadForm, SubmitLeadForm, FormOptions.PromptInStart);
            context.Call(leadFormDialog, Callback);
        }

        #region Generic Intents

        [LuisIntent(BotConstants.INTENT_GENERIC_AGE)]
        public async Task GenericAge(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_AGE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_APPEARANCE)]
        public async Task GenericAppearance(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_APPEARANCE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_AWAKE)]
        public async Task GenericAwake(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_AWAKE);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_HELP)]
        public async Task GenericHelp(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_HELP);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_HOBBY)]
        public async Task GenericHobby(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_HOBBY);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_JOKE)]
        public async Task GenericJoke(IDialogContext context, LuisResult result)
        {
            var rnd = new Random();
            int r = rnd.Next(AnConstants.Jokes.Count);
            await context.PostAsync(AnConstants.Jokes[r]);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_LANGUAGE)]
        public async Task GenericLanguage(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_LANGUAGE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_LOCATION)]
        public async Task GenericLocation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_LOCATION);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_LOVE)]
        public async Task GenericLove(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_LOVE);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_LOVEQUESTION)]
        public async Task GenericLoveQuestion(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_LOVEQUESTION);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_NAME)]
        public async Task GenericName(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_NAME);
            context.Wait(MessageReceived);
        }


        [LuisIntent(BotConstants.INTENT_GENERIC_REALITY)]
        public async Task GenericReality(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_REALITY);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_RUDE)]
        public async Task GenericRude(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_RUDE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_SORRY)]
        public async Task GenericSorry(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_SORRY);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_STATE)]
        public async Task GenericState(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_STATE);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_THANKS)]
        public async Task GenericThanks(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_THANKS);
            context.Wait(MessageReceived);
        }

        [LuisIntent(BotConstants.INTENT_GENERIC_TIME)]
        public async Task GenericTime(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(BotConstants.DIALOG_GENERIC_TIME);
            context.Wait(MessageReceived);
        }

        #endregion Generic intents


        private async Task CallbackActivity(IDialogContext context, IAwaitable<object> result)
        {
            var postCode = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_POST_CODE, out postCode);
            var filter = SearchHelper.GetSearchFilter(null, postCode);
            bool isKnownUser = SearchHelper.IsKnownUser(context);

            //Call search
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = SearchHelper.GetCardsAttachments(filter, isKnownUser);

            await context.PostAsync(reply);
            
            context.Wait(MessageReceived);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }


    }
}