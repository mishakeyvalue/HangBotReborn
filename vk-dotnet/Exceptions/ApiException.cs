using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace vk_dotnet
{
    /// <summary>
    /// Represents errors thrown by vk API;
    /// </summary>
    public class ApiException : Exception
    {

        /// <summary>
        /// Code of the error;
        /// For detailed info check https://vk.com/dev/errors
        /// </summary>
        public int Code { get; private set; }


        /// <summary>
        /// Represents parameters that were send with request;
        /// </summary>
        public List<RequestParam> RequestedParams { get; private set; }


        public ApiException(Error err) : base(err.error_msg)
        {
            Code = err.error_code;
            RequestedParams = err.request_params;
        }

        #region Useless at that moment
        //public ApiException(int code, string message, List<RequestParam> requestedParams) : base(message)
        //{
        //    Code = code;
        //    RequestedParams = requestedParams;
        //} 
        #endregion

    }

    /// <summary>
    /// Represents one key-value request parameter.
    /// </summary>
    public class RequestParam
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    /// <summary>
    /// Represents error returned by vk`s server;
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Code of the error;
        /// For detailed info check https://vk.com/dev/errors
        /// </summary>
        public int error_code { get; set; }

        /// <summary>
        /// Returned message from the vk server.
        /// </summary>
        public string error_msg { get; set; }

        /// <summary>
        /// Parameters of the failed request;
        /// </summary>
        public List<RequestParam> request_params { get; set; }
    }
}
