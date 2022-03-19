using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swats.Infrastructure.Features.Tags.GetTag;

public class GetTagCommandHandler : IRequestHandler<GetTagCommand, Result<FetchTag>>
{
    private readonly IManageRepository _manageRepository;

    public GetTagCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchTag>> Handle(GetTagCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetTag(request.Id, cancellationToken);

        return rst.Id == Guid.Empty
            ? Result.Fail<FetchTag>($"Tag with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}
