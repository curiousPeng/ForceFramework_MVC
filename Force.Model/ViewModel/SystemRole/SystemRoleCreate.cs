using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.SystemRole
{
    public class SystemRoleCreate
    {
        [Required]
        public string RoleName { get; set; }
        public string Remark { get; set; }
    }
}
