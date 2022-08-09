namespace VaporStore
{
	using AutoMapper;
    using System;
    using System.Globalization;
    using System.Linq;
    using VaporStore.Data;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;
    using VaporStore.DataProcessor.Dto.Import;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{
            
			//Input
			CreateMap<CardInputModel, Card>()
				   .ForMember(d => d.Type, mo => mo.MapFrom(s => Enum.Parse<CardType>(s.Type)));
			CreateMap<UserInputModel, User>()
				.ForMember(d=>d.Cards, mo=>mo.MapFrom(s=>s.Cards));

            //Output

            CreateMap<Game, GameOutputModel>()
                .ForMember(d => d.Name, mo => mo.MapFrom(s => s.Name))
                .ForMember(d => d.DeveloperName, mo => mo.MapFrom(s => s.Developer.Name))
                .ForMember(d => d.Tags, mo => mo.MapFrom(s => string.Join(", ", s.GameTags.Select(gt=>gt.Tag.Name))))
               .ForMember(d => d.PurchasesCount, mo => mo.MapFrom(s => s.Purchases.Count)); 
            
            CreateMap<Genre, GenreOutputModel>()
                .ForMember(d => d.Games, mo => mo.MapFrom(s => s.Games
                     .OrderByDescending(g=>g.Purchases.Count)
                     .ThenBy(g=>g.Id)
                     .Where(g => g.Purchases.Any())))
                .ForMember(d => d.TotalPurchaseCount, mo => mo.MapFrom(s => s.Games
                      .Where(g => g.Purchases.Any()).Sum(g => g.Purchases.Count)));



            CreateMap<Game, GameOfPurchaseOutputModel>()
                .ForMember(d => d.Genre, mo => mo.MapFrom(s => s.Genre.Name))
                .ForMember(d => d.Price, mo => mo.MapFrom(s => s.Price));

            CreateMap<Purchase, PurchaseOutputModel>()
                .ForMember(d => d.CardNumber, mo => mo.MapFrom(s => s.Card.Number))
                .ForMember(d => d.Cvc, mo => mo.MapFrom(s => s.Card.Cvc))
                .ForMember(d => d.Date, mo => mo.MapFrom(s => s.Date.ToString("yyyy-MM-dd HH:mm", 
                     CultureInfo.InvariantCulture)))
                .ForMember(d => d.Game, mo => mo.MapFrom(s => s.Game));

         
        }
    }
}