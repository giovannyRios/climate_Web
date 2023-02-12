using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblTypeTemperatureDTO
    {
        public int ID { get; set; }

        [Required]
        public string NameTypeTemp { get; set; }

        public ICollection<tblWeatherForecastDTO> tblWeatherForecast { get; set; }
    }
}