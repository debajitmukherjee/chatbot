using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;


namespace AN.ChatBot.Models
{
    [Serializable]
    public class Car
    {
        public string Image { get; set; }

        public string AutoNationPrice { get; set; }

        public string Msrp { get; set; }

        public string Name { get; set; }

        public string Vin { get; set; }

        public static List<Car> GetCars()
        {
            List<Car> cars = new List<Car>();
            cars.Add(new Car
            {
                Image = "http://az336639.vo.msecnd.net/actualcdn/5111b51fb17457cf811b617cdfefeac5_392x294_V2.jpg",
                Name = "2017 Ford Super Duty F-350 SRW Lariat 4WD Crew Cab 6.75' Box",
                Vin = "1FT8W3BT9HED76074",
                Msrp = "$67,855",
                AutoNationPrice = "$61,855"
            });

            cars.Add(new Car
            {
                Image = "http://az336639.vo.msecnd.net/actualcdn/b934d0f67c6b5e2cb7e1c3c3d877731b_392x294_V2.jpg",
                Name = "2017 Ford Fusion SE FWD",
                Vin = "3FA6P0H91HR363935",
                Msrp = "$34,115",
                AutoNationPrice = "$30,115"
            });

            cars.Add(new Car
            {
                Image = "http://az336639.vo.msecnd.net/actualcdn/e92aadacdec256398effc5f7aa5d7d11_392x294_V2.jpg",
                Name = "2017 Audi A6 2.0 TFSI Premium FWD",
                Vin = "WAUC8AFC1HN083477",
                Msrp = "$54,335",
                AutoNationPrice = "$50,335"
            });

            cars.Add(new Car
            {
                Image = "http://az336639.vo.msecnd.net/actualcdn/dd5f9a200d1d5e8586eccafba8ec0e7c_392x294_V2.jpg",
                Name = "2017 Jeep Wrangler Unlimited Chief Edition 4x4 *Ltd Avail*",
                Vin = "1C4BJWEG7HL655524",
                Msrp = "$43,220",
                AutoNationPrice = "$35,220"
            });

            cars.Add(new Car
            {
                Image = "http://az336639.vo.msecnd.net/actualcdn/2d68dccb599f5d37b6517ab89b2f7519_392x294_V2.jpg",
                Name = "2017 Honda Civic Sedan LX CVT",
                Vin = "19XFC2F50HE053736",
                Msrp = "$20,415",
                AutoNationPrice = "$18,415"
            });


            return cars;
        }

        public static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage> { cardImage },
                Buttons = new List<CardAction> { cardAction },
            };

            return heroCard.ToAttachment();
        }

        public static IList<Attachment> GetCardsAttachments()
        {
            var cards = new List<Attachment>();

            var cars = GetCars();

            foreach (var car in cars)
            {
                cards.Add(GetThumbnailCard(
                   title: car.Name,
                   subtitle: car.Vin,
                   text: car.Msrp,
                   cardImage: new CardImage(url: car.Image),
                   cardAction: new CardAction(ActionTypes.MessageBack, "Get Lower Price", value: "Get Lower Price")
                    ));
            }


            return cards;
        }
    }
}