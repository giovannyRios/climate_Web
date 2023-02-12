using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace climate_MVC.Models.DTO
{
    public class tblUsersDTO
    {
        public int ID { get; set; }

        [Required]
        public string userName { get; set; }

        [Required]
        public string userPassword { get; set; }
    }
}