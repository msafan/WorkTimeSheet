using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.Services
{
    public abstract class WebApiServiceBase : IWebApiService
    {
        protected WebApiServiceBase(IWebApiLayer webApiLayer)
        {
            WebApiLayer = webApiLayer;
        }

        protected IWebApiLayer WebApiLayer { get; }

        protected List<string> FetchUrlFilterParameters(params IUrlParamsConvertible[] parameters)
        {
            return parameters.Select(x => x?.ConvertToUrlParams()).Where(x => !string.IsNullOrEmpty(x)).ToList();
        }
    }
}
