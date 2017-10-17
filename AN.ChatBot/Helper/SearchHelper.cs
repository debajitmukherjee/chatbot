using AN.ChatBot.Common;
using AN.SC.BLL.Models.Inventory;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Linq;
using System;
using AutoNation.DSF.Datacontracts.Vehicle;
using Microsoft.Bot.Builder.Dialogs;
using AutoNation.DSF.Datacontracts.Store;

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
                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_CONDITION))
                {
                    filter.StockType = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_CONDITION).Entity;
                }

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
                        filter.MaxPrice = Convert.ToInt32(result.Entities.OrderByDescending(e => e.Entity).FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER && e.Entity != yearNumber.ToString()).Entity);
                    }


                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_PRICE_ABOVE) &&
                    result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER))
                {
                    int yearNumber = 0;
                    if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_YEAR))
                    {
                        yearNumber = Convert.ToInt32(result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_YEAR).Entity);
                    }

                    if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER && e.Entity != yearNumber.ToString()))
                    {
                        filter.MinPrice = Convert.ToInt32(result.Entities.OrderBy(e => e.Entity).FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER && e.Entity != yearNumber.ToString()).Entity);
                    }


                }
            }

            return filter;
        }

        public static IList<Attachment> GetCardsAttachments(SimpleSearchFilter filter, bool isKnownUser)
        {
            var response = GetSimpleSearchResult(filter);
            var cards = new List<Attachment>();
            string carDetailsUrl = ConfigurationManager.AppSettings["CarDetailsURL"];
            var stores = response.Stores;

            foreach (var car in response.SearchVehicles)
            {
                string price = GetCarPrice(car, isKnownUser);
                string vin = BotConstants.LABEL_VIN + BotConstants.LABEL_SPACE + car.Vehicle.Vin;
                string condition = BotConstants.LABEL_CONDITION + BotConstants.LABEL_SPACE + car.Vehicle.StockType;
                string inventoryStatus = GetCarInventoryStatus(car);
                var cardAction = GetCardAction(car, isKnownUser);
                var distance = GetDistance(car, stores);

                cards.Add(CardHelper.GetHeroCard(
                       title: car.Name,
                       subtitle: vin + BotConstants.LABEL_NEW_LINE + condition + BotConstants.LABEL_NEW_LINE + inventoryStatus + BotConstants.LABEL_NEW_LINE + distance,
                       text: price,
                       cardImages: new List<CardImage> { new CardImage(url: BotConstants.IMAGE_PROTOCOL + car.ImageURL) },
                       cardActions: new List<CardAction> {
                           cardAction,
                           new CardAction(ActionTypes.OpenUrl,BotConstants.BUTTON_VIEW_CAR, value: carDetailsUrl + car.Vehicle.Vin)
                       }
                    ));
            }


            return cards;
        }

        public static bool IsKnownUser(IDialogContext context)
        {
            string firstName = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_FIRST_NAME, out firstName);

            return !string.IsNullOrEmpty(firstName);
        }

        private static string GetCarPrice(SimpleSearchVehicle car, bool isKnownUser)
        {
            string price = string.Empty;
            if (car.Vehicle.PricingStack != null && car.Vehicle.PricingStack.Items.Count > 0)
            {
                PaymentStackItemType pricingItem;
                if (isKnownUser)
                {
                    pricingItem = car.Vehicle.PricingStack.Items.FirstOrDefault(pi => pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_UNLOCKED || pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_BOTH);
                }
                else
                {
                    pricingItem = car.Vehicle.PricingStack.Items.FirstOrDefault(pi => pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_LOCKED || pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_BOTH);
                }

                price = PopulatePrice(pricingItem);
            }

            return price;
        }

        private static string PopulatePrice(PaymentStackItemType pricingItem)
        {
            string price = string.Empty;
            int priceValue = 0;

            if(AnConstants.PricingStackItems.Any(p => p.Key == pricingItem.Name.ToLowerInvariant()))
            {
                var pricingStackItem = AnConstants.PricingStackItems.FirstOrDefault(p => p.Key == pricingItem.Name.ToLowerInvariant());
                price = pricingStackItem.Value;
                var pricingItemValue = pricingItem.Value;
                priceValue = (int)Convert.ToDouble(pricingItemValue); 
            }
            else
            {
                price = pricingItem.Name;
            }

            if (priceValue > 0)
            {
                price += BotConstants.LABEL_SPACE + BotConstants.LABEL_DOLLAR + string.Format(BotConstants.CURRENCY_FORMAT, priceValue);
            }


            return price;
        }

        private static string GetCarInventoryStatus(SimpleSearchVehicle car)
        {
            string inventoryStatus = string.Empty;

            if (AnConstants.InventoryStatusTypes.Any(i => i.Key == car.Vehicle.InventoryStatus))
            {
                var anInventoryStatus = AnConstants.InventoryStatusTypes.FirstOrDefault(i => i.Key == car.Vehicle.InventoryStatus);
                inventoryStatus = BotConstants.LABEL_INVENTORY_STATUS + BotConstants.LABEL_SPACE + anInventoryStatus.Value;
            }

            return inventoryStatus;
        }

        private static CardAction GetCardAction(SimpleSearchVehicle car, bool isKnownUser)
        {
            if (!isKnownUser && car.UnKnownUserView.ShowGetLowerPrice)
            {
                return new CardAction(ActionTypes.MessageBack, BotConstants.BUTTON_GET_LOWER_PRICE, value: BotConstants.BUTTON_GET_LOWER_PRICE);
            }
            else
            {
                return new CardAction(ActionTypes.MessageBack, BotConstants.BUTTON_CONTACT_STORE, value: BotConstants.BUTTON_CONTACT_STORE);
            }

        }

        private static string GetDistance(SimpleSearchVehicle car, List<StoreDistanceType> stores)
        {
            string distance = string.Empty;
            var store = stores.FirstOrDefault(s => s.HyperionId == car.Vehicle.HyperionId);
            if(store != null)
            {
                string milesFormat = string.Format(BotConstants.MILES_FORMAT, store.Distance);
                distance = milesFormat + BotConstants.LABEL_SPACE + BotConstants.LABEL_MILES_AWAY;
            }

            return distance;
        }


    }
}