using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Application.Commands
{
    public class DeleteMovieDtoCommand : IRequest
    {
        public Guid Id { get; set; }    
    }
}
