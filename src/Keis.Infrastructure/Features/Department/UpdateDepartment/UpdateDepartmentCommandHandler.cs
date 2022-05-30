using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Department.UpdateDepartment;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public UpdateDepartmentCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = _mapper.Map<UpdateDepartmentCommand, Keis.Model.Domain.Department>(request);
        
        var rst = await _manageRepository.UpdateDepartment(department, cancellationToken);
        return rst > 0
            ? Result.Ok(department.Id)
            : Result.Fail<string>("Not able to update department now!");
    }
}
