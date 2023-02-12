using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace climate_MVC.Classes
{
    public class serviceUrl
    {
        public static string loginUrl = "api/LoginUser";
        public static string getAlert = "api/GetAlert?ID=";
        public static string getAllAlert = "api/GetAllAlert";
        public static string getTypeTemperature = "api/GetTypeTemperature?ID=";
        public static string getAllTypeTemperature = "api/GetAllTypeTemperature";
        public static string getClimaticPhenomen = "api/GetClimaticPhenomenon?ID=";
        public static string getAllClimaticPhenomen  = "api/GetAllClimaticPhenomenon";
        public static string getStation = "api/GetWeatherStation?ID=";
        public static string getAllStation = "api/GetAllWeatherStation";
        public static string getAllWeatherForecast = "api/GetAllweatherForecast";
        public static string getWeatherForecast = "api/GetweatherForecast?ID=";
        public static string UpdateWeatherForecast = "api/UpdateWeatherForecast";
        public static string DeleteWeatherForecast = "api/DeleteWeatherForecast?ID=";
        public static string InsertWeatherForecast = "api/RegisterWeatherForecast";
    }
}