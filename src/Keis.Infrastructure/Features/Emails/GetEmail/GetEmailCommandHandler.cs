using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Emails.GetEmail;

public class GetEmailCommandHandler : IRequestHandler<GetEmailCommand, Result<FetchEmail>>
{
    private readonly IManageRepository _manageRepository;

    public GetEmailCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchEmail>> Handle(GetEmailCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetEmail(request.Id, cancellationToken);

        return rst is null
            ? Result.Fail<FetchEmail>($"Email Settings with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}
