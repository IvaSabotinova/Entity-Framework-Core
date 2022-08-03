using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }
        public int Id { get; set; }

        [Required]
        public string CountryName { get; set; }    
        public int ArmySize { get; set; }

        public ICollection<CountryGun> CountriesGuns { get; set; }

    }

}
