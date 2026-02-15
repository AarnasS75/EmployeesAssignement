using System.Data.Entity;
using Npgsql;

namespace EmployeesAssignement.Configuration;

public class NpgsqlConfiguration : DbConfiguration
{
    public NpgsqlConfiguration()
    {
        SetProviderServices("Npgsql", NpgsqlServices.Instance);
        SetProviderFactory("Npgsql", NpgsqlFactory.Instance);
        SetDefaultConnectionFactory(new NpgsqlConnectionFactory());
    }
}