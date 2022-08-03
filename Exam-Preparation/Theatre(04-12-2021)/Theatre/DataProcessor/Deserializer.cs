namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Plays");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PlayInputModel[]), xmlRootAttribute);
            using StringReader reader = new StringReader(xmlString);

            PlayInputModel[] dtoModels = (PlayInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();
            List<Play> playes = new List<Play>();
            foreach (PlayInputModel dtoModel in dtoModels)
            {
               TimeSpan currentDtoDuration = TimeSpan.ParseExact(dtoModel.Duration,"c", CultureInfo.InvariantCulture);
                if(!IsValid(dtoModel) || currentDtoDuration.TotalMinutes < 60)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Play newPlay = new Play()
                {
                    Title = dtoModel.Title,
                    Duration = currentDtoDuration,
                    Rating = dtoModel.Rating,
                    Genre = Enum.Parse<Genre>(dtoModel.Genre),
                    Description = dtoModel.Description,
                    Screenwriter = dtoModel.Screenwriter
                };
                playes.Add(newPlay);
                sb.AppendLine(String.Format(SuccessfulImportPlay, newPlay.Title, newPlay.Genre, newPlay.Rating));
            }
            context.Plays.AddRange(playes);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Casts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CastInputModel[]), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            CastInputModel[] dtoCasts = (CastInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();

            List<Cast> casts = new List<Cast>();   

            foreach (CastInputModel dtoCast in dtoCasts)
            {
                if (!IsValid(dtoCast))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast newCast = new Cast()
                {
                    FullName = dtoCast.FullName,
                    IsMainCharacter = dtoCast.IsMainCharacter,
                    PhoneNumber = dtoCast.PhoneNumber,
                    PlayId = dtoCast.PlayId

                };

                casts.Add(newCast);
                sb.AppendLine(String.Format(SuccessfulImportActor, newCast.FullName, newCast.IsMainCharacter ? "main" : "lesser"));
            }
            context.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            TheatreInputModel[] dtoTheatres = JsonConvert.DeserializeObject<TheatreInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Theatre> theatres = new List<Theatre>();   

            foreach (TheatreInputModel dtoTheatre in dtoTheatres)
            {
                if (!IsValid(dtoTheatre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Theatre newTheatre = new Theatre()
                {
                    Name = dtoTheatre.Name,
                    NumberOfHalls = dtoTheatre.NumberOfHalls,
                    Director = dtoTheatre.Director,
                };
                List<Ticket> validTickets = new List<Ticket>();
                foreach (TicketInputModel dtoTicket in dtoTheatre.Tickets)
                {
                    if (!IsValid(dtoTicket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket newTicket = new Ticket()
                    {
                        Price = dtoTicket.Price,
                        RowNumber = dtoTicket.RowNumber,
                        PlayId = dtoTicket.PlayId
                    };
                    validTickets.Add(newTicket);    

                }
                newTheatre.Tickets = validTickets;  
                theatres.Add(newTheatre);
                sb.AppendLine(String.Format(SuccessfulImportTheatre, newTheatre.Name, newTheatre.Tickets.Count));
            }
            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
