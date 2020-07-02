using Force.Model.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Force.App.Util
{
    public static class ResponseHelper
    {
        public static MResponse<T> Success<T>(T Data, string msg = "")
        {
            return new MResponse<T> { Code = 1, Msg = msg ?? "ok", Data = Data };
        }

        public static MResponse<string> Error(string msg = "")
        {
            return new MResponse<string> { Code = 0, Msg = msg ?? "fail", Data = string.Empty };
        }
    }
}
