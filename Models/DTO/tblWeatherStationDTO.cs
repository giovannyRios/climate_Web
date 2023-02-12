using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblWeatherStationDTO
    {
        public int ID { get; set; }
        public int NumStation { get; set; }
        public string NameStation { get; set; }
        public int Id_Territory { get; set; }

        public tblTerritoryDTO tblTerritory { get; set; }

        public ICollection<tblWeatherForecastDTO> tblWeatherForecast { get; set; }
    }
}