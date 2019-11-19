﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.Menu
{
    public class SaveMenuModel
    {
        [Required]
        public string ParentCode { get; set; }
        [Required]
        public string ControllName { get; set; }
        [Required]
        public string Icon { get; set; }
        [Required]
        public int Sort { get; set; }
        [Required]
        public string Remark { get; set; }
        [Required]
        public int ControllType { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string ControllUrl { get; set; }
    }
}
