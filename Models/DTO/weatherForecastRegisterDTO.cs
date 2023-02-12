using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace climate_MVC.Models.DTO
{
    public class weatherForecastRegisterDTO
    {
        public long ID { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string ClimaticDescription { get; set; }

        [Required]

        [Display(Name = "Fecha registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateRegister { get; set; }

        [Required]

        [Display(Name = "Temperatura")]

        [RegularExpression(@"^\d+(\.\d{1,2})?$",ErrorMessage = "Debe ingresar un formato valido")]
        [Range(0,99)]
        
        public decimal Temperature { get; set; }

        [Required]
        public int IdTypeTemperature { get; set; }

        [Required]

        [Display(Name = "tipo temperatura")]
        public string tipoTemperatura { get; set; }

        public int StateRegister { get; set; }

        public int? IdClimaticPhenomenon { get; set; }

        [Display(Name = "Fenómeno")]
        public string ClimaticPhenomenon { get; set; }
        public int? IdAlert { get; set; }

        [Display(Name = "Alerta")]
        public string Alert { get; set; }

        public List<tblAlertDTO> listaAlertas { get; set; }
        public List<tblClimaticPhenomenonDTO> listaClimathicPhenomeno { get; set; }
        public List<tblTypeTemperatureDTO> listatipoTemperatura { get; set; }
        public List<tblWeatherStationDTO> listaEstaciones { get; set; }

        [Required]
        public int IdWeatherStation { get; set; }

        [Display(Name = "Estación climatica")]
        public string WeatherStation { get; set; }
    }
}