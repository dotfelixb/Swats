using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeDepartment;

public class ChangeDepartmentCommand : IRequest<Result<string>>
	{
		public string Id { get; set; }
		public string Department { get; set; }
		public string CreatedBy { get; set; }
	}

