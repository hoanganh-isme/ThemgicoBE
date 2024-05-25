using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Themgico.DTO
{
    public class ResponseAPI
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}

