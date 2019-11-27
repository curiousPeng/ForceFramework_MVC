using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.SystemRole
{
   public class SystemRoleEdit
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public string Remark { get; set; }
    }
}
