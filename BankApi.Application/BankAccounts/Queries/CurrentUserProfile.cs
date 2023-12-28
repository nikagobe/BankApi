using BankApi.Application.Common.Interfaces;
using BankApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.BankAccounts.Queries;
public class GetCurrentUserProfileQuery : IRequest<User>
{
    public Guid UserId { get; set; }
}

public class GetCurrentUserProfileQueryHandler : IRequestHandler<GetCurrentUserProfileQuery, User>
{
    private readonly IApplicationDbContext _context;

    public GetCurrentUserProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == request.UserId);
        return userProfile;
    }
}
