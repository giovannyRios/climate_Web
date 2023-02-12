using System;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblWeatherForecastDTO
    {
        public long ID { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string ClimaticDescription { get; set; }

        [Required]

        [Display(Name = "Fecha registro")]
        public System.DateTime DateRegister { get; set; }

        [Required]

        [Display(Name = "Temperatura")]
        public decimal Temperature { get; set; }

        [Required]
        public int IdTypeTemperature { get; set; }

        [Required]
        public int StateRegister { get; set; }

        public Nullable<int> IdClimaticPhenomenon { get; set; }
        public Nullable<int> IdAlert { get; set; }

        [Required]
        public int IdWeatherStation { get; set; }

        public tblAlertDTO tblAlert { get; set; }
        public tblClimaticPhenomenonDTO tblClimaticPhenomenon { get; set; }
        public tblTypeTemperatureDTO tblTypeTemperature { get; set; }
        public virtual tblWeatherStationDTO tblWeatherStation { get; set; }
    }
}