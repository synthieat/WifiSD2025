using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace SD.WS.Controllers
{
    public class MediatRBaseController : ControllerBase
    {
        private IMediator mediator;

        /* Konstante für ID - Parameter */
        protected const string ID_PARAMETER = "/{Id}";

        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected T SetLocationUri<T>(T result, string id)
        {
            if (result == null || string.IsNullOrWhiteSpace(id))
            {
                throw new HttpRequestException("Resource is found");
            }

            /* Aktueller URL ermitteln */
            var baseUrl = Request.HttpContext.Request.GetEncodedUrl();

            /* Base URL bis zum ersten (QueryString) Paramter, falls vorhanden, kürzen */
            var length = baseUrl.IndexOf('?') > 0 ? baseUrl.IndexOf('?') : baseUrl.Length;
            var uri = baseUrl.Substring(0, length);

            /* Id an den gekürzten URL anhängen
               ../Movie/MovieDto
               ../Movie/MovieDto/
               ../Movie/MovieDto/?Parmeter1=
               ../Movie/MovieDto?Parmeter1=
            */

            uri = string.Concat(uri, uri.EndsWith('/') ? string.Empty : "/", id);

            /* Location Header setzen */
            HttpContext.Response.Headers.Append("Location", uri);
            /* Http-Status Code auf 201 - Created setzen */
            HttpContext.Response.StatusCode = StatusCodes.Status201Created;

            return result;
        }
    }
}
