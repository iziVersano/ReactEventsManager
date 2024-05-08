using Application.Core;
using MediatR;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Cities
{
    public class List
    {
        public class Query : IRequest<Result<List<string>>>
        {
            // You can add parameters for filtering if needed
        }

        // Mock handler for testing purposes
        // public class Handler : IRequestHandler<Query, Result<List<string>>>
        // {
        //     public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
        //     {
        //         // Define a list of dummy cities
        //         var cities = new List<string> { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix" };

        //         // Return the list of dummy cities
        //         return Result<List<string>>.Success(cities);
        //     }
        // }

        // Original handler
        public class Handler : IRequestHandler<Query, Result<List<string>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<string>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cities = await _context.Cities.Select(c => c.Name).ToListAsync();
                return Result<List<string>>.Success(cities);
            }
        }
    }
}
