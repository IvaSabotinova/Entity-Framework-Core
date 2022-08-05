namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            CountryInputModel[] dtoCountries = Deserialize<CountryInputModel[]>(xmlString, "Countries");

            StringBuilder sb = new StringBuilder();

            List<Country> countries = new List<Country>();

            foreach (CountryInputModel dtoCountry in dtoCountries)
            {
                if (!IsValid(dtoCountry))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country newCountry = new Country()
                {
                    CountryName = dtoCountry.CountryName,
                    ArmySize = dtoCountry.ArmySize
                };
                countries.Add(newCountry);
                sb.AppendLine(String.Format(SuccessfulImportCountry, newCountry.CountryName, newCountry.ArmySize));
            }
            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {

            ManufacturerInputModel[] dtoManufacturers = Deserialize<ManufacturerInputModel[]>(xmlString, "Manufacturers");

            StringBuilder sb = new StringBuilder();

            List<Manufacturer> manufacturers = new List<Manufacturer>();

            foreach (ManufacturerInputModel dtoManufacturer in dtoManufacturers)
            {
                if (!IsValid(dtoManufacturer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool manufacturerNameExists = manufacturers.FirstOrDefault(x => x.ManufacturerName == dtoManufacturer.ManufacturerName) != null;

                if (manufacturerNameExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string[] manufacturerFounded = dtoManufacturer.Founded.Split(", ");

                if (manufacturerFounded.Length < 3)
                {
                    continue;
                }

                Manufacturer newManufacturer = new Manufacturer()
                {
                    ManufacturerName = dtoManufacturer.ManufacturerName,
                    Founded = dtoManufacturer.Founded,
                };

                manufacturers.Add(newManufacturer);
                string townNameAndCountry = $"{manufacturerFounded[manufacturerFounded.Length - 2]}, {manufacturerFounded[manufacturerFounded.Length - 1]}";


                sb.AppendLine(string.Format(SuccessfulImportManufacturer, newManufacturer.ManufacturerName, townNameAndCountry));

            }
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            ShellInputModel[] dtoShells = Deserialize<ShellInputModel[]>(xmlString, "Shells");

            StringBuilder sb = new StringBuilder();

            List<Shell> shells = new List<Shell>();

            foreach (ShellInputModel dtoShell in dtoShells)
            {
                if (!IsValid(dtoShell))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Shell newShell = new Shell()
                {
                    ShellWeight = dtoShell.ShellWeight,
                    Caliber = dtoShell.Caliber
                };

                shells.Add(newShell);
                sb.AppendLine(String.Format(SuccessfulImportShell, newShell.Caliber, newShell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            GunInputModel[] dtoGuns = JsonConvert.DeserializeObject<GunInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Gun> guns = new List<Gun>();

            foreach (GunInputModel dtoGun in dtoGuns)
            {
                if (!IsValid(dtoGun))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Gun newGun = new Gun()
                {
                    ManufacturerId = dtoGun.ManufacturerId,
                    GunWeight = dtoGun.GunWeight,
                    BarrelLength = dtoGun.BarrelLength,
                    NumberBuild = dtoGun.NumberBuild,
                    ShellId = dtoGun.ShellId,
                    Range = dtoGun.Range,
                    GunType = Enum.Parse<GunType>(dtoGun.GunType),
                    CountriesGuns = dtoGun.Countries.Select(x => new CountryGun
                    {
                        CountryId = x.Id
                    })
                   .ToArray(),
                };
                guns.Add(newGun);

                sb.AppendLine(String.Format(SuccessfulImportGun, newGun.GunType.ToString(), newGun.GunWeight, newGun.BarrelLength));

            }
            context.Guns.AddRange(guns);
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

        public static T Deserialize<T>(string xmlString, string xmlRoot)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute(xmlRoot);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);
            T dto = (T)xmlSerializer.Deserialize(reader);

            return dto;
        }
    }
}
