using System;
using System.Data;
using System.Text;
using Application.Common.Interfaces;
using Dapper;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Sender.Queries;

public class GetSendersQueryHandler : IRequestHandler<GetSendersQuery, ErrorOr<List<SenderResult>>>
{
    private readonly IDbConnectionFactory _dbConnection;

    public GetSendersQueryHandler(IDbConnectionFactory dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ErrorOr<List<SenderResult>>> Handle(GetSendersQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnection.CreateConnection();

        var sql = new StringBuilder(@"
        SELECT 
            s.""Id"",
            s.""BusinessName"",
            s.""LegalName"",
            s.""Email"",
            s.""Phone"",
            s.""Website"",
            s.""AddressLine1"",
            s.""AddressLine2"",
            s.""City"",
            s.""State"",
            s.""PostalCode"",
            s.""Country"",
            s.""TaxId"",
            s.""LogoUrl"",
            s.""PrimaryColor"",
            s.""IsActive"",
            s.""CreatedAt"",
            s.""UpdatedAt""
        FROM ""Senders"" s
        WHERE s.""UserId"" = @UserId
            AND s.""IsDeleted"" = FALSE");

        if (request.IsActive)
            sql.Append(@" AND s.""IsActive"" = @IsActive");

        sql.Append(@" ORDER BY s.""CreatedAt"" DESC");

        var results = await connection
        .QueryAsync<SenderResult>(sql.ToString(), new { UserId = request.UserId, IsActive = request.IsActive });

        return results.ToList();
    }
}
