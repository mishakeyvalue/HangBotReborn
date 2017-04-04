using System;
using System.Collections.Generic;
using System.Text;

namespace vk_dotnet.Objects
{
    /// <summary>
    /// Represents errors thrown by vk API;
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Code of the error;
        /// For detailed info check https://vk.com/dev/errors
        /// </summary>
        public int Code { get; set; }
        public string Message { get; set; }
        public List<RequestParam> RequestedParams { get; set; }

        /// <summary>
        /// Represents parameters that were send with request;
        /// </summary>
        public class RequestParam
        {
            public string key { get; set; }
            public string value { get; set; }
        }

    }
}
