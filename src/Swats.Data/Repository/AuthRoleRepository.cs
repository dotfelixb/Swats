using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;

namespace Swats.Data.Repository;

public class AuthRoleRepository : BasePostgresRepository
    , IRoleStore<AuthRole>
{
    public AuthRoleRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<IdentityResult> CreateAsync(AuthRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(AuthRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<AuthRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedRoleNameAsync(AuthRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleIdAsync(AuthRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleNameAsync(AuthRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(AuthRole role, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(AuthRole role, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(AuthRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}