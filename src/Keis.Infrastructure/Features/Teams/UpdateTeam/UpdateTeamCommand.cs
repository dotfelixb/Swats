using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Teams.UpdateTeam
{
    public class UpdateTeamCommand : IRequest<Result<string>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Manager { get; set; }
        public DefaultStatus Status { get; set; }
        public string Response { get; set; }
        public string UpdatedBy { get; set; }
    }
}