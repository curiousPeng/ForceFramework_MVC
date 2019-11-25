using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Model.ViewModel.SystemUser
{
    public class SystemUserRes
    {
        public int UseCode { set; get; }
        public string Name { set; get; }
        public string Account { set; get; }
        public string Phone { set; get; }
        public int Status { set; get; }
        public string Email { set; get; }
        public string HeadImg { set; get; }
        public DateTime CreatedTime { set; get; }
    }
}
