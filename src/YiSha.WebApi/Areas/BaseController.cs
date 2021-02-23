using Furion.DynamicApiController;
using YiSha.WebApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.WebApi.Areas
{
    [ApiDescriptionSettings(SplitCamelCase = false)]
    [ServiceFilter(typeof(AuthorizeFilterAttribute))]
    public class BaseController : IDynamicApiController
    {
        public virtual JsonResult Json(object data) => new JsonResult(data);
    }
}
