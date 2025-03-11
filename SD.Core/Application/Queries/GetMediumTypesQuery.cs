using MediatR;
using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Application.Queries
{
    public class GetMediumTypesQuery : IRequest<IEnumerable<MediumType>>
    {
    }
}
