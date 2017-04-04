using System;
using System.Collections.Generic;
using System.Text;

namespace vk_dotnet
{
    public class AutorizationException : ApiException
    {
        public AutorizationException(Error err) : base(err)
        {}
    }
}
