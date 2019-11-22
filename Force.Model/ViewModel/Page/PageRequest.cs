using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Model.ViewModel.Page
{
    public class PageRequest
    {
        public int draw { get; set; }
        public int length { get; set; }
        public int start { get; set; }
    }
}
