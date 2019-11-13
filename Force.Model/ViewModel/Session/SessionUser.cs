using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Model.ViewModel.Session
{
    public class SessionUser
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string HeadImg { get; set; }
        public string Email { get; set; }
        public string UId { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public List<int> AuthMenu { get; set; }
    }
}
