using System;
using System.Data;

namespace Application.Common.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
