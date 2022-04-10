using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Keis.Model;
using Keis.Model.Domain;

namespace Keis.Data.Repository;

public class AuthRoleRepository : BasePostgresRepository
    , IRoleStore<AuthRole>
{
    public AuthRoleRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<IdentityResult> CreateAsync(AuthRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(AuthRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }

    public Task<AuthRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<AuthRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedRoleNameAsync(AuthRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<string> GetRoleIdAsync(AuthRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<string> GetRoleNameAsync(AuthRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(AuthRole role, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(AuthRole role, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(AuthRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }
}