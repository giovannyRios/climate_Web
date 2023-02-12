using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblUsersInsert
    {
        [Required]
        [Display(Name = "Usuario")]
        public string userName { get; set; }
        [Required]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string userPassword { get; set; }
    }
}