using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblTerritoryDTO
    {
        public int ID { get; set; }
        [Required]
        public string NameTerritory { get; set; }
        
        [Required]
        public int Id_Country { get; set; }

        public tblCountryDTO tblCountry { get; set; }
        public ICollection<tblWeatherStationDTO> tblWeatherStation { get; set; }
    }
}