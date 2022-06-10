using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Template.UpdateTemplate
{
    public class UpdateTemplateCommand : IRequest<Result<string>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string[] MergeTags { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
    }

    internal class UpdateTemplateCommandHandler : IRequestHandler<UpdateTemplateCommand, Result<string>>
    {
        private readonly IManageRepository _manageRepository;
        private readonly IMapper _mapper;

        public UpdateTemplateCommandHandler(IManageRepository manageRepository, IMapper mapper)
        {
            _manageRepository = manageRepository;
            _mapper = mapper;
        }


        public async Task<Result<string>> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = _mapper.Map<UpdateTemplateCommand, Model.Domain.Template>(request);

            var rst = await _manageRepository.UpdateTemplate(template, cancellationToken);
            return rst > 0
                ? Result.Ok(template.Id)
                : Result.Fail<string>("Not able to update template now!");
        }
    }
}
