using AN.ChatBot.Common;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Text.RegularExpressions;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;

namespace AN.ChatBot.Models
{

    public enum ContactOption
    {
        Email,
        Mobile
    }

    [Serializable]
    public class LeadForm
    {
        public static IForm<LeadForm> BuildForm()
        {
            return new FormBuilder<LeadForm>().Message(BotConstants.MESSAGE_LEAD_FORM)
                    .Field(nameof(FirstName))
                    .Field(nameof(LastName))
                    .Field(nameof(Email), IsValidEmail)
                    .Field(nameof(PhoneNo), IsValidPhoneNo)
                    .Field(nameof(PrefferedContactOption))
                    .OnCompletion(LeadFormSubmitted)
                    .Message("Thank you, I have submitted your message.")
                    .Build();
        }

        private static Task LeadFormSubmitted(IDialogContext context, LeadForm state)
        {
            throw new NotImplementedException();
        }

        private static bool IsValidPhoneNo(LeadForm state)
        {
            return true;
        }

        private static bool IsValidEmail(LeadForm state)
        {
            bool isEmail = Regex.IsMatch(state.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            return isEmail;
        }

        [Prompt(BotConstants.PROMPT_FIRST_NAME)]
        public string FirstName;

        [Prompt(BotConstants.PROMPT_LAST_NAME)]
        public string LastName;

        [Prompt(BotConstants.PROMPT_EMAIL)]
        public string Email;

        [Prompt(BotConstants.PROMPT_PHONE_NO)]
        public string PhoneNo;

        [Template(TemplateUsage.EnumSelectOne, BotConstants.PROMPT_CONTACT_OPTION_1, BotConstants.PROMPT_CONTACT_OPTION_2)]
        public ContactOption? PrefferedContactOption;
    }
}