using MediatR;
using SD.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Application.Commands
{
    public class UpdateMovieDtoCommand : IRequest<MovieDto>
    {
        public Guid Id { get; set; }
        public MovieDto MovieDto { get; set; }  
    }
}
