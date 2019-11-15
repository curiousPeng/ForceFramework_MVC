using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Model.ViewModel.SubHeader
{
    public class SubHeader
    {
        public string Title { get; set; }
        public string PageTitle { get; set; }
        public string SubPageTitle { get; set; }
        public Dictionary<string, string> LinkText { get; set; }
    }
}
