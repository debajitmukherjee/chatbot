using Microsoft.Bot.Connector;
using System.Collections.Generic;


namespace AN.ChatBot.Helper
{
    public static class CardHelper
    {
        public static Attachment GetThumbnailCard(string title, string subtitle, string text, List<CardImage> cardImages, List<CardAction> cardActions)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = cardImages,
                Buttons = cardActions,
            };

            return heroCard.ToAttachment();
        }

        public static Attachment GetHeroCard(string title, string subtitle, string text, List<CardImage> cardImages, List<CardAction> cardActions)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = cardImages,
                Buttons = cardActions,
            };

            return heroCard.ToAttachment();
        }

    }
}