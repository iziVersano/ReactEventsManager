using Application.Core;
using MediatR;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Users
{
    public class List
    {
        public class Query : IRequest<Result<List<string>>>
        {
            // You can add parameters for filtering if needed
        }

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
                var users = await _context.Users.Select(u => u.DisplayName).ToListAsync();
                return Result<List<string>>.Success(users);
            }
        }
    }
}
