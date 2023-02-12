using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using climate_MVC.Models.DTO;
using climate_MVC.Classes;


namespace climate_MVC.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<ActionResult> Index()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (Session["token"] == null)
            {
                FormsAuthentication.SignOut();
            }

            ApiControl apiControl = new ApiControl();
            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getAllWeatherForecast);
            List<weatherForecastRegisterDTO> listaSalida = new List<weatherForecastRegisterDTO>();


            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        List<tblWeatherForecastDTO> tblWeatherForecast = JsonConvert.DeserializeObject<List<tblWeatherForecastDTO>>(json);

                        foreach (var item in tblWeatherForecast)
                        {
                            weatherForecastRegisterDTO weatherForecastRegister = new weatherForecastRegisterDTO();
                            if (item.IdAlert != null)
                            {
                                weatherForecastRegister.IdAlert = item.IdAlert;
                                tblAlertDTO alerta = await retornarAlerta(apiControl, item.IdAlert);
                                weatherForecastRegister.Alert = alerta.NameAlert;
                            }
                            else
                            {
                                weatherForecastRegister.IdAlert = null;
                                weatherForecastRegister.Alert = "N/A";
                            }

                            weatherForecastRegister.ClimaticDescription = item.ClimaticDescription;
                            weatherForecastRegister.DateRegister = item.DateRegister;
                            weatherForecastRegister.ID = item.ID;
                            weatherForecastRegister.StateRegister = item.StateRegister;
                            weatherForecastRegister.Temperature = item.Temperature;
                            weatherForecastRegister.IdTypeTemperature = item.IdTypeTemperature;
                            tblTypeTemperatureDTO tblTypeTemperature = await retornarTipoTemperatura(apiControl, item.IdTypeTemperature);
                            weatherForecastRegister.tipoTemperatura = tblTypeTemperature.NameTypeTemp;
                            if (item.IdClimaticPhenomenon != null)
                            {
                                weatherForecastRegister.IdClimaticPhenomenon = item.IdClimaticPhenomenon;
                                tblClimaticPhenomenonDTO fenomeno = await retornarFenomenoClimatico(apiControl, item.IdClimaticPhenomenon);
                                weatherForecastRegister.ClimaticPhenomenon = fenomeno.DescriptionClimaticPhenomenon;
                            }
                            else
                            {
                                weatherForecastRegister.IdClimaticPhenomenon = null;
                                weatherForecastRegister.ClimaticPhenomenon = "N/A";
                            }

                            weatherForecastRegister.IdWeatherStation = item.IdWeatherStation;
                            tblWeatherStationDTO tblWeatherStation = await retornarEstacion(apiControl, item.IdWeatherStation);
                            weatherForecastRegister.WeatherStation = tblWeatherStation.NameStation;
                            listaSalida.Add(weatherForecastRegister);
                        }

                        return View(listaSalida);

                    case System.Net.HttpStatusCode.InternalServerError:
                        ModelState.AddModelError("", "Ocurrio un error al consultar");
                        return View();
                    case System.Net.HttpStatusCode.NoContent:
                        ModelState.AddModelError("", "No existen datos registrados");
                        return View();
                    case System.Net.HttpStatusCode.NotFound:
                        return View();
                    default:
                        ModelState.AddModelError("", "Contacte al administrador");
                        return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "No existen datos registrados");
                return View();
            }
        }

        [Authorize]
        public async Task<ActionResult> Borrar(long ID)
        {
            ApiControl apiControl = new ApiControl();
            var json = JsonConvert.SerializeObject(new { ID });
            HttpResponseMessage response = await apiControl.PostRequest(new StringContent(json, Encoding.UTF8, "application/json"), Session["token"].ToString(), serviceUrl.DeleteWeatherForecast + ID);
            switch (response.StatusCode)
            {
                default:
                    return RedirectToAction("Index", "Home");
            }
            
        }

        [Authorize]
        public async Task<ActionResult> GuardarInsercion(weatherForecastRegisterDTO objInsercion)
        {
            ApiControl apiControl = new ApiControl();

            WeatherForecastInsert objectoInsercion = new WeatherForecastInsert();
            objectoInsercion.ID = objInsercion.ID;
            objectoInsercion.ClimaticDescription = objInsercion.ClimaticDescription;
            objectoInsercion.DateRegister = Convert.ToDateTime(objInsercion.DateRegister);
            if (objInsercion.IdAlert == null || objInsercion.IdAlert == 0)
            {
                objectoInsercion.IdAlert = null;
            }
            else
            {
                objectoInsercion.IdAlert = objInsercion.IdAlert;
            }

            if (objInsercion.IdClimaticPhenomenon == null || objInsercion.IdClimaticPhenomenon == 0)
            {
                objectoInsercion.IdClimaticPhenomenon = null;
            }
            else
            {
                objectoInsercion.IdClimaticPhenomenon = objInsercion.IdClimaticPhenomenon;
            }

            if (objInsercion.IdTypeTemperature == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar el tipo de temperatura");
                return View(objInsercion);
            }
            else
            {
                objectoInsercion.IdTypeTemperature = objInsercion.IdTypeTemperature;
            }

            if (objInsercion.IdWeatherStation == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar el tipo de la estación climatica");
                return View(objInsercion);
            }
            else
            {
                objectoInsercion.IdWeatherStation = objInsercion.IdWeatherStation;
            }

            objectoInsercion.StateRegister = 1;
            objectoInsercion.Temperature = objInsercion.Temperature;


            var json = JsonConvert.SerializeObject(objectoInsercion);
            HttpResponseMessage response = await apiControl.PostRequest(new StringContent(json, Encoding.UTF8, "application/json"), Session["token"].ToString(), serviceUrl.InsertWeatherForecast);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return RedirectToAction("Index", "Home");
                default:
                    ModelState.AddModelError("", "Error en la Inserción, valide los datos ingresados");
                    return View(objInsercion);
            }
        }

        [Authorize]
        public async Task<ActionResult> Insertar()
        {
            ApiControl apiControl = new ApiControl();
            weatherForecastRegisterDTO weatherForecastRegister = new weatherForecastRegisterDTO();
            List<tblAlertDTO> alertaLista = await retornarListAlerta(apiControl);
            weatherForecastRegister.listaAlertas = alertaLista;
            weatherForecastRegister.listatipoTemperatura = await retornarListaTipoTemperatura(apiControl);
            List<tblClimaticPhenomenonDTO> listaFenomeno = await retornarListaFenomenoClimatico(apiControl);
            weatherForecastRegister.listaClimathicPhenomeno = listaFenomeno;
            weatherForecastRegister.listaEstaciones = await retornarListaEstacion(apiControl);
            return View(weatherForecastRegister);
        }

        [Authorize]
        public async Task<ActionResult> Editar(int? ID)
        {
            weatherForecastRegisterDTO salida;
            ApiControl apiControl = new ApiControl();
            weatherForecastRegisterDTO weatherForecastRegister = new weatherForecastRegisterDTO();
            tblWeatherForecastDTO item = await retornarDatoClimatico(apiControl, ID);
            if (item.IdAlert != null)
            {
                weatherForecastRegister.IdAlert = item.IdAlert;
                tblAlertDTO alerta = await retornarAlerta(apiControl, item.IdAlert);
                List<tblAlertDTO> alertaLista = await retornarListAlerta(apiControl);
                weatherForecastRegister.Alert = alerta.NameAlert;
                weatherForecastRegister.listaAlertas = alertaLista;
            }
            else
            {
                List<tblAlertDTO> alertaLista = await retornarListAlerta(apiControl);
                weatherForecastRegister.listaAlertas = alertaLista;
                weatherForecastRegister.IdAlert = null;
                weatherForecastRegister.Alert = "N/A";
            }

            weatherForecastRegister.ClimaticDescription = item.ClimaticDescription;
            weatherForecastRegister.DateRegister = item.DateRegister;
            weatherForecastRegister.ID = item.ID;
            weatherForecastRegister.StateRegister = item.StateRegister;
            weatherForecastRegister.Temperature = item.Temperature;
            weatherForecastRegister.IdTypeTemperature = item.IdTypeTemperature;
            tblTypeTemperatureDTO tblTypeTemperature = await retornarTipoTemperatura(apiControl, item.IdTypeTemperature);
            weatherForecastRegister.tipoTemperatura = tblTypeTemperature.NameTypeTemp;
            weatherForecastRegister.listatipoTemperatura = await retornarListaTipoTemperatura(apiControl);

            if (item.IdClimaticPhenomenon != null)
            {

                weatherForecastRegister.IdClimaticPhenomenon = item.IdClimaticPhenomenon;
                tblClimaticPhenomenonDTO fenomeno = await retornarFenomenoClimatico(apiControl, item.IdClimaticPhenomenon);
                weatherForecastRegister.ClimaticPhenomenon = fenomeno.DescriptionClimaticPhenomenon;
                List<tblClimaticPhenomenonDTO> listaFenomeno = await retornarListaFenomenoClimatico(apiControl);
                weatherForecastRegister.listaClimathicPhenomeno = listaFenomeno;
            }
            else
            {
                weatherForecastRegister.IdClimaticPhenomenon = null;
                weatherForecastRegister.ClimaticPhenomenon = "N/A";
                List<tblClimaticPhenomenonDTO> listaFenomeno = await retornarListaFenomenoClimatico(apiControl);
                weatherForecastRegister.listaClimathicPhenomeno = listaFenomeno;
            }

            weatherForecastRegister.IdWeatherStation = item.IdWeatherStation;
            tblWeatherStationDTO tblWeatherStation = await retornarEstacion(apiControl, item.IdWeatherStation);
            weatherForecastRegister.WeatherStation = tblWeatherStation.NameStation;
            weatherForecastRegister.listaEstaciones = await retornarListaEstacion(apiControl);
            return View(weatherForecastRegister);
        }

        [Authorize]
        private async Task<tblWeatherForecastDTO> retornarDatoClimatico(ApiControl apiControl, int? IdDato)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getWeatherForecast + IdDato.ToString());
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        tblWeatherForecastDTO datoClimatico = JsonConvert.DeserializeObject<tblWeatherForecastDTO>(json);
                        return datoClimatico;
                    default:
                        return new tblWeatherForecastDTO();
                }
            }
            else
            {
                return new tblWeatherForecastDTO();
            }
        }


        [Authorize]
        private async Task<tblWeatherStationDTO> retornarEstacion(ApiControl apiControl, int IdStation)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getStation + IdStation.ToString());
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        tblWeatherStationDTO alerta = JsonConvert.DeserializeObject<tblWeatherStationDTO>(json);
                        return alerta;
                    default:
                        return new tblWeatherStationDTO();
                }
            }
            else
            {
                return new tblWeatherStationDTO();
            }
        }

        [Authorize]
        private async Task<List<tblWeatherStationDTO>> retornarListaEstacion(ApiControl apiControl)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getAllStation);
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        List<tblWeatherStationDTO> alerta = JsonConvert.DeserializeObject<List<tblWeatherStationDTO>>(json);
                        return alerta;
                    default:
                        return new List<tblWeatherStationDTO>();
                }
            }
            else
            {
                return new List<tblWeatherStationDTO>();
            }
        }

        [Authorize]
        private async Task<tblAlertDTO> retornarAlerta(ApiControl apiControl, int? idlerta)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getAlert + idlerta.ToString());
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        tblAlertDTO alerta = JsonConvert.DeserializeObject<tblAlertDTO>(json);
                        return alerta;
                    default:
                        return new tblAlertDTO();
                }
            }
            else
            {
                return new tblAlertDTO();
            }
        }

        [Authorize]
        private async Task<List<tblAlertDTO>> retornarListAlerta(ApiControl apiControl)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getAllAlert);
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        List<tblAlertDTO> alerta = JsonConvert.DeserializeObject<List<tblAlertDTO>>(json);
                        return alerta;
                    default:
                        return new List<tblAlertDTO>();
                }
            }
            else
            {
                return new List<tblAlertDTO>();
            }
        }

        [Authorize]
        private async Task<tblClimaticPhenomenonDTO> retornarFenomenoClimatico(ApiControl apiControl, int? idFenomenoClimatica)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getClimaticPhenomen + idFenomenoClimatica.ToString());
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        tblClimaticPhenomenonDTO fenomeno = JsonConvert.DeserializeObject<tblClimaticPhenomenonDTO>(json);
                        return fenomeno;
                    default:
                        return new tblClimaticPhenomenonDTO();
                }
            }
            else
            {
                return new tblClimaticPhenomenonDTO();
            }
        }

        [Authorize]
        private async Task<List<tblClimaticPhenomenonDTO>> retornarListaFenomenoClimatico(ApiControl apiControl)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getAllClimaticPhenomen);
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        List<tblClimaticPhenomenonDTO> fenomeno = JsonConvert.DeserializeObject<List<tblClimaticPhenomenonDTO>>(json);
                        return fenomeno;
                    default:
                        return new List<tblClimaticPhenomenonDTO>();
                }
            }
            else
            {
                return new List<tblClimaticPhenomenonDTO>();
            }
        }

        [Authorize]
        private async Task<tblTypeTemperatureDTO> retornarTipoTemperatura(ApiControl apiControl, int idTipoTemperatura)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getTypeTemperature + idTipoTemperatura.ToString());
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        tblTypeTemperatureDTO alerta = JsonConvert.DeserializeObject<tblTypeTemperatureDTO>(json);
                        return alerta;
                    default:
                        return new tblTypeTemperatureDTO();
                }
            }
            else
            {
                return new tblTypeTemperatureDTO();
            }
        }

        [Authorize]
        private async Task<List<tblTypeTemperatureDTO>> retornarListaTipoTemperatura(ApiControl apiControl)
        {

            HttpResponseMessage responseMessage = await apiControl.GetRequest(Session["token"].ToString(), serviceUrl.getAllTypeTemperature);
            if (responseMessage != null)
            {
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        List<tblTypeTemperatureDTO> alerta = JsonConvert.DeserializeObject<List<tblTypeTemperatureDTO>>(json);
                        return alerta;
                    default:
                        return new List<tblTypeTemperatureDTO>();
                }
            }
            else
            {
                return new List<tblTypeTemperatureDTO>();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> GuardarEdicion(weatherForecastRegisterDTO objInsercion)
        {
            
            ApiControl apiControl = new ApiControl();

            WeatherForecastInsert objectoInsercion = new WeatherForecastInsert();
            objectoInsercion.ID = objInsercion.ID;
            objectoInsercion.ClimaticDescription = objInsercion.ClimaticDescription;
            objectoInsercion.DateRegister = Convert.ToDateTime(objInsercion.DateRegister);
            if (objInsercion.IdAlert == null || objInsercion.IdAlert == 0)
            {
                objectoInsercion.IdAlert = null;
            }
            else
            {
                objectoInsercion.IdAlert = objInsercion.IdAlert;
            }

            if (objInsercion.IdClimaticPhenomenon == null || objInsercion.IdClimaticPhenomenon == 0)
            {
                objectoInsercion.IdClimaticPhenomenon = null;
            }
            else
            {
                objectoInsercion.IdClimaticPhenomenon = objInsercion.IdClimaticPhenomenon;
            }

            if (objInsercion.IdTypeTemperature == 0)
            {
                ModelState.AddModelError("","Debe seleccionar el tipo de temperatura");
                return View(objInsercion);
            }
            else
            {
                objectoInsercion.IdTypeTemperature = objInsercion.IdTypeTemperature;
            }

            if (objInsercion.IdWeatherStation == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar el tipo de la estación climatica");
                return View(objInsercion);
            }
            else
            {
                objectoInsercion.IdWeatherStation = objInsercion.IdWeatherStation;
            }

            objectoInsercion.StateRegister = objInsercion.StateRegister;
            objectoInsercion.Temperature = objInsercion.Temperature;


            var json = JsonConvert.SerializeObject(objectoInsercion);
            HttpResponseMessage response = await apiControl.PostRequest(new StringContent(json,Encoding.UTF8, "application/json"),Session["token"].ToString(),serviceUrl.UpdateWeatherForecast);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return RedirectToAction("Index","Home");
                default:
                    ModelState.AddModelError("","Error en la actualización, valide los datos ingresados");
                    return View(objInsercion);
            }
        }



        [AllowAnonymous]
        [OutputCache(CacheProfile = "noCache")]
        public ActionResult Login(string URL)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Home");
            }
            ViewBag.ReturnUrl = URL;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(tblUsersInsert objeto, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(objeto);
            }
            else
            {
                ApiControl apiControl = new ApiControl();
                HttpResponseMessage responseMessage = await apiControl.ApiLogin(objeto);

                if (responseMessage != null)
                {
                    switch (responseMessage.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var jsonUser = await responseMessage.Content.ReadAsStringAsync();
                            tblUsersDTO obj = JsonConvert.DeserializeObject<tblUsersDTO>(jsonUser);
                            FormsAuthentication.SetAuthCookie(obj.userName, false);
                            HttpHeaders headers = responseMessage.Headers;
                            IEnumerable<string> values;
                            if (headers.TryGetValues("token", out values))
                            {
                                Session["token"] = values.First();
                            }

                            return RedirectToLocal(ReturnUrl);

                        case System.Net.HttpStatusCode.NoContent:
                            ModelState.AddModelError("", "Verifique usuario o contraseña");
                            return View();
                        case System.Net.HttpStatusCode.Unauthorized:
                            ModelState.AddModelError("", "Verifique usuario o contraseña");
                            return View();
                        case System.Net.HttpStatusCode.InternalServerError:
                            ModelState.AddModelError("", "Error interno, contacte al administrador");
                            return View();
                        case System.Net.HttpStatusCode.NotFound:
                            ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                            return View();
                        default:
                            ModelState.AddModelError("", "Error interno, contacte al administrador");
                            return View();

                    }
                }

                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View();
            }
        }

        [Authorize]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}