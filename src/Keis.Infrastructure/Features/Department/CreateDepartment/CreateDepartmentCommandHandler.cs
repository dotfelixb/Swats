using System.Text.Json;
using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Department.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateDepartmentCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = _mapper.Map<CreateDepartmentCommand, Model.Domain.Department>(request);

        var rst = await _manageRepository.CreateDepartment(department, cancellationToken);
        return rst > 0 
            ? Result.Ok(department.Id) 
            : Result.Fail<string>("Not able to create now!");
    }
}