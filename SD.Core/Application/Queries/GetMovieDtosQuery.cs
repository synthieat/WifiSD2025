using MediatR;
using SD.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Core.Application.Queries
{   
    public class GetMovieDtosQuery : IRequest<IEnumerable<MovieDto>>
    {
        public int? GenreId { get; set; }
        public string? MediumTypeCode { get; set; }

        public string? SearchText { get; set; }

        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;

    }
}
