using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaProject.WebAPI.Responses
{
    public class CustomResponse<T>
    {
        public bool Erro { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }

        public CustomResponse()
        {
        }

        public CustomResponse(T data, bool erro, string message, int status)
        {
            Data = data;
            Erro = erro;
            Message = message;
            StatusCode = status;
        }
    }
}
