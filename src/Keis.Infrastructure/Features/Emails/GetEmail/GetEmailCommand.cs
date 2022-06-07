using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Emails.GetEmail;

public class GetEmailCommand : GetType, IRequest<Result<FetchEmail>>
{
}
