using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.SystemUser
{
    public class SystemUserCreate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        public string Account { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        public string Pwd { get; set; }
        public short IsUse { get; set; }
    }
}
