using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Force.Model.ViewModel.Menu
{
    public class MenuViewModel
    {
        public List<MenuViewModel> Children { get; set; }
        public int MenuCode { get; set; }
        public string MenuIcon { get; set; }
        public string MenuUrl { get; set; }
        public string MenuName { get; set; }
        public int Sort { get; set; }
        public string MenuType { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }
}
