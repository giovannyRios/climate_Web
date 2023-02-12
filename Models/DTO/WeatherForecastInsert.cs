using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace climate_MVC.Models.DTO
{
    public class WeatherForecastInsert
    {
        public long ID { get; set; }
        public string ClimaticDescription { get; set; }
        public System.DateTime DateRegister { get; set; }
        public decimal Temperature { get; set; }
        public int IdTypeTemperature { get; set; }
        public int StateRegister { get; set; }
        public Nullable<int> IdClimaticPhenomenon { get; set; }
        public Nullable<int> IdAlert { get; set; }
        public int IdWeatherStation { get; set; }
    }
}