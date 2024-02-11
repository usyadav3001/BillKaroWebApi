using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public class Result
    {
        private dynamic data;
        private string message;
        public string Status { get; set; }
        public long timestamp { get; set; }
        public dynamic Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }
        public Result()
        {

        }
        public Result(dynamic data, string message, string status)
        {
            Data = data;
            Message = message;
            Status = status;
        }
    }
}
