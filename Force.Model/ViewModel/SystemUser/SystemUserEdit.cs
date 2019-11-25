using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.SystemUser
{
    public class SystemUserEdit
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Pwd { get; set; }
        public short IsUse { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
