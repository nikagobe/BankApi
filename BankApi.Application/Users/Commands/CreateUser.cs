using BankApi.Application.Common;
using BankApi.Application.Common.Interfaces;
using BankApi.Application.Identity.Models.Login;
using BankApi.Application.Identity.Service;
using BankApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Application.Users.Commands;


public class CreateUserCommand : IRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalNumber { get; set; }
    public DateTime BirthDate { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;

    public CreateUserCommandHandler(UnitOfWork unitOfWork,
                                    IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await AddUser(request, cancellationToken);

        await AddCredentials(request, user, cancellationToken);

        await AddBankAccount(user, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task AddBankAccount(User user, CancellationToken cancellationToken)
    {
        var bankAccount = new BankAccount()
        {
            UserId = user.UserId,
            BankAccountId = Guid.NewGuid(),
            MoneyAmount = 0
        };

        await _unitOfWork.DbContext.BankAccounts.AddAsync(bankAccount, cancellationToken);
    }

    private async Task AddCredentials(CreateUserCommand request, User user, CancellationToken cancellationToken)
    {
        var createCredentialsRequest = new CreateCredentialsRequest()
        {
            Password = request.Password,
            UserId = user.UserId,
            UserName = request.UserName
        };

        await _identityService.CreateCredentials(createCredentialsRequest, cancellationToken);
    }

    private async Task<User> AddUser(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            BirthDate = request.BirthDate,
            CreatedAt = DateTime.UtcNow,
            FirstName = request.FirstName,
            LastName = request.LastName,
            NationalNumber = request.NationalNumber,
            UserId = Guid.NewGuid()
        };

        await _unitOfWork.DbContext.Users.AddAsync(user, cancellationToken);
        return user;
    }
}
