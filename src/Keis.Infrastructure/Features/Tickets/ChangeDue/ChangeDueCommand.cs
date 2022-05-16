using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Tickets.ChangeDue;

public class ChangeDueCommand : IRequest<Result<DateTimeOffset>>
    {
        public string Id { get; set; }
		public DateTimeOffset DueAt { get; set; }
		public string CreatedBy { get; set; }
    }
