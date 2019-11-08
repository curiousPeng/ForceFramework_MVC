using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.Login
{
    public class SignInModel
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
