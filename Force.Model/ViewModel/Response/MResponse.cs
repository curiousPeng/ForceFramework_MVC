using System;
using System.Collections.Generic;
using System.Text;

namespace Force.Model.ViewModel.Response
{
    public class MResponse<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
        public string Msg { get; set; }
    }
}
