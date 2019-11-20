using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Model.ViewModel.Menu
{
    public class ViewMenu
    {
        public int MenuCode { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string MenuIcon { get; set; }
        public bool IsChecked { get; set; }
        public int ParentCode { get; set; }
        public List<ViewMenu> Children { get; set; }
    }
}
