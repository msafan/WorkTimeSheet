using WorkTimeSheet.Interfaces;

namespace WorkTimeSheet.Services
{
    public abstract class WebApiServiceBase
    {
        protected WebApiServiceBase(IWebApiLayer webApiLayer)
        {
            WebApiLayer = webApiLayer;
        }

        protected IWebApiLayer WebApiLayer { get; }
    }
}
