using AN.ChatBot.Common;
using Microsoft.Bot.Builder.FormFlow;
using System;
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
                    .Field(nameof(Email))
                    .Field(nameof(PhoneNo))
                    .Field(nameof(PrefferedContactOption))
                    .Confirm(BotConstants.CONFIRM_LEAD_FORM)
                    .OnCompletion(LeadFormSubmitted)
                    .Message("Thank you, I have submitted your message.")
                    .Build();
        }

        private static Task LeadFormSubmitted(IDialogContext context, LeadForm state)
        {
            throw new NotImplementedException();
        }

        [Prompt(BotConstants.PROMPT_FIRST_NAME)]
        public string FirstName;

        [Prompt(BotConstants.PROMPT_LAST_NAME)]
        public string LastName;

        [Prompt(BotConstants.PROMPT_EMAIL)]
        [Pattern(BotConstants.REGX_EMAIL)]
        public string Email;

        [Prompt(BotConstants.PROMPT_PHONE_NO)]
        public string PhoneNo;

        [Template(TemplateUsage.EnumSelectOne, BotConstants.PROMPT_CONTACT_OPTION_1, BotConstants.PROMPT_CONTACT_OPTION_2)]
        public ContactOption? PrefferedContactOption;
    }
}