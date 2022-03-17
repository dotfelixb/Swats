﻿using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Queries;

internal class AdminManageQuery
{
}

public class FetchTicketType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
}

public class GetTicketTypeCommand : IRequest<Result<TicketType>>
{
    public Guid Id { get; set; }
}

public class ListTicketTypeCommand : IRequest<Result<IEnumerable<TicketType>>>
{
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 20;
}