using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Template.GetTemplate
{

    internal class GetTemplateCommandHandler : IRequestHandler<GetTemplateCommand, Result<FetchTemplate>>
    {
        private readonly IManageRepository _manageRepository;

        public GetTemplateCommandHandler(IManageRepository manageRepository)
        {
            _manageRepository = manageRepository;
        }

        public async Task<Result<FetchTemplate>> Handle(GetTemplateCommand request, CancellationToken cancellationToken)
        {
            var rst = await _manageRepository.GetTemplate(request.Id, cancellationToken);

            return rst is null
                ? Result.Fail<FetchTemplate>($"Template with id [{request.Id}] does not exist!")
                : Result.Ok(rst);
        }
    }
}
