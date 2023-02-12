using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblCountryDTO
    {
        public int ID { get; set; }

        [Required]
        public string NameCountry { get; set; }

        public ICollection<tblTerritoryDTO> tblTerritory { get; set; }
    }
}