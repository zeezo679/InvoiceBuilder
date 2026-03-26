using System;
using System.Data;
using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Persistence;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration
            .GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
