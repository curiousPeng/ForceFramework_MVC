using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.SystemRole
{
    public class SystemRoleTreeMenu
    {
        public string text { get; set; }
        public int id { get; set; }
        public string icon { get; set; }
        public Dictionary<string,bool> state { get; set; }
        public List<SystemRoleTreeMenu> children { get; set; }
    }

    public class RoleAuthMenu
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Icon { get; set; }
    }

    public class AuthSaveMenu
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public string Menus { get; set; }
    }
}
