using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace SD.Web.Controllers
{
    public class MediatRBaseController : Controller
    {
        private IMediator mediator;

        /* Konstante für ID - Parameter */
        protected const string ID_PARAMETER = "/{Id}";

        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    }
}
