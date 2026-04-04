using System.Data;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Dapper;
using Domain.Errors;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Sender.Queries.GetSenderById;

public sealed class GetSenderByIdQueryHandler : IRequestHandler<GetSenderByIdQuery, ErrorOr<SenderDto>>
{
    private readonly IDbConnectionFactory _dbConnection;

    public GetSenderByIdQueryHandler(IDbConnectionFactory dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ErrorOr<SenderDto>> Handle(GetSenderByIdQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using IDbConnection connection = _dbConnection.CreateConnection();

        const string sql = """
            SELECT
                s."Id",
                s."BusinessName",
                s."LegalName",
                s."Email",
                s."Phone",
                s."City",
                s."Country",
                s."LogoUrl",
                s."IsActive",
                s."CreatedAt"
            FROM "Senders" s
            WHERE s."Id" = @SenderId
                AND s."UserId" = @UserId
                AND s."IsDeleted" = FALSE
            LIMIT 1
            """;

        var row = await connection.QuerySingleOrDefaultAsync<GetSenderByIdRow>(
            sql,
            new
            {
                request.SenderId,
                UserId = request.UserId.ToString()
            });

        if (row is null)
            return SenderErrors.NotFound;

        return new SenderDto(
            row.Id,
            row.BusinessName,
            row.LegalName,
            row.Email,
            row.Phone,
            row.City,
            row.Country,
            row.LogoUrl,
            row.IsActive,
            row.CreatedAt
        );
    }
}

internal sealed class GetSenderByIdRow
{
    public Guid Id { get; init; }
    public string BusinessName { get; init; } = string.Empty;
    public string? LegalName { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string? LogoUrl { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}
