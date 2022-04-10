using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.HelpTopic.GetHelpTopic;

public class GetHelpTopicCommandHandler : IRequestHandler<GetHelpTopicCommand, Result<FetchHelpTopic>>
{
    private readonly IManageRepository _manageRepository;

    public GetHelpTopicCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchHelpTopic>> Handle(GetHelpTopicCommand request, CancellationToken cancellationToken)
    {
        var result = await _manageRepository.GetHelpTopic(request.Id, cancellationToken);

        return result is null
            ? Result.Fail<FetchHelpTopic>($"Help Topic with id [{request.Id}] does not exist!")
            : Result.Ok(result);
    }
}