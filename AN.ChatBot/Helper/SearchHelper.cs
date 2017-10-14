using AN.ChatBot.Common;
using AN.SC.BLL.Models.Inventory;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Linq;
using System;

namespace AN.ChatBot.Helper
{
    public static class SearchHelper
    {
        public static SimpleSearchResponse GetSimpleSearchResult(SimpleSearchFilter filter)
        {
            var serializer = new JavaScriptSerializer();
            var url = ConfigurationManager.AppSettings["SearchURL"] + serializer.Serialize(filter);
            var webRequestHelper = new HttpRequestHelper(url);
            var responseponse = webRequestHelper.GetResponse();

            return serializer.Deserialize<SimpleSearchResponse>(responseponse);
        }

        public static SimpleSearchFilter GetSearchFilter(LuisResult result, string postalCode)
        {
            var filter = new SimpleSearchFilter
            {
                MinMileage = 0,
                MaxMileage = 650000,
                PageNo = 1,
                PageSize = 12,
                MinPrice = 0,
                MaxPrice = 65000,
                Radius = -1,
                SortBy = "distance",
                SortDirection = 0

            };

            if (!string.IsNullOrEmpty(postalCode))
            {
                filter.PostalCode = postalCode;
            }

            if (result != null && result.Entities != null && result.Entities.Count > 0)
            {
                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_MAKE))
                {
                    filter.Make = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_MAKE).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_MODEL))
                {
                    filter.Model = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_MODEL).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_COLOR))
                {
                    filter.ExteriorColor = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_COLOR).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_YEAR))
                {
                    filter.Year = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_YEAR).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_PRICE_UNDER) &&
                    result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER))
                {
                    int yearNumber = 0;
                    if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_YEAR))
                    {
                        yearNumber = Convert.ToInt32(result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_YEAR).Entity);
                    }

                    if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER && e.Entity != yearNumber.ToString()))
                    {
                        filter.MaxPrice = Convert.ToInt32(result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER && e.Entity != yearNumber.ToString()).Entity);
                    }


                }
            }

            return filter;
        }

        public static IList<Attachment> GetCardsAttachments(SimpleSearchFilter filter)
        {
            var response = GetSimpleSearchResult(filter);
            var cards = new List<Attachment>();

            foreach (var car in response.SearchVehicles)
            {
                cards.Add(CardHelper.GetHeroCard(
                       title: car.Name,
                       subtitle: BotConstants.LABEL_VIN + car.Vehicle.Vin,
                       text: "MSRP:" + "\n" + "Autonation Price:",
                       cardImages: new List<CardImage> { new CardImage(url: BotConstants.IMAGE_PROTOCOL + car.ImageURL) },
                       cardActions: new List<CardAction> { new CardAction(ActionTypes.MessageBack, BotConstants.BUTTON_GET_LOWER_PRICE, value: BotConstants.BUTTON_GET_LOWER_PRICE) }
                    ));
            }


            return cards;
        }


    }
}