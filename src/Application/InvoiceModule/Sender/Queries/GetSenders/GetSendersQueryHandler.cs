using System;
using System.Data;
using System.Text;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Common.Models;
using Dapper;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Sender.Queries;

public class GetSendersQueryHandler : IRequestHandler<GetSendersQuery, ErrorOr<PagedResult<SenderDto>>>
{
    private readonly IDbConnectionFactory _dbConnection;

    public GetSendersQueryHandler(IDbConnectionFactory dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ErrorOr<PagedResult<SenderDto>>> Handle(GetSendersQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnection.CreateConnection();

        var offset = (request.Page - 1) * request.PageSize;

        var sql = new StringBuilder(@"
        SELECT
            s.""Id"",
            s.""BusinessName"",
            s.""LegalName"",
            s.""Email"",
            s.""Phone"",
            s.""City"",
            s.""Country"",
            s.""LogoUrl"",
            s.""IsActive"",
            s.""CreatedAt"",
            COUNT(*) OVER() AS ""TotalCount""
        FROM ""Senders"" s
        WHERE s.""UserId"" = @UserId
            AND s.""IsDeleted"" = FALSE");

        if (request.IsActive.HasValue)
            sql.Append(@" AND s.""IsActive"" = @IsActive");

        sql.Append(@" ORDER BY s.""CreatedAt"" DESC
        LIMIT @PageSize OFFSET @Offset");

        var rows = await connection.QueryAsync<SenderRow>(sql.ToString(), new 
        {   
            UserId = request.UserId, 
            IsActive = request.IsActive,
            PageSize = request.PageSize,
            Offset = offset
        });

        var rowList = rows.ToList();

        var totalCount = rowList.FirstOrDefault()?.TotalCount ?? 0;

        //project from row to dto
        var items = rowList.Select(r => new SenderDto(
            r.Id,
            r.BusinessName,
            r.LegalName,
            r.Email,
            r.Phone,
            r.City,
            r.Country,
            r.LogoUrl,
            r.IsActive,
            r.CreatedAt
        )).ToList();

        return new PagedResult<SenderDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
      
    }
}
