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
public class CurrentUserProfileQuery : IRequest<User>
{
    public Guid UserId { get; set; }
}

public class CurrentUserProfileQueryHandler : IRequestHandler<CurrentUserProfileQuery, User>
{
    private readonly IApplicationDbContext _context;

    public CurrentUserProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(CurrentUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.Users.FirstOrDefaultAsync(x => x.UserId == request.UserId);
        return userProfile;
    }
}
