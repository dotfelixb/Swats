using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;

namespace Swats.Data.Repository;

public class AuthUserRepository : BasePostgresRepository
    , IUserStore<AuthUser>
    , IUserPasswordStore<AuthUser>
    , IUserEmailStore<AuthUser>
    , IUserPhoneNumberStore<AuthUser>
    , IUserTwoFactorStore<AuthUser>
    , IUserRoleStore<AuthUser>
{
    public AuthUserRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task AddToRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var query = @"";

            var cmd = await conn.ExecuteAsync(query, new { });

            return cmd > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "User creation failed!" });
        });
    }

    public Task<IdentityResult> DeleteAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var query = @"";

            var cmd = await conn.ExecuteAsync(query, new { });

            return cmd > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError { Description = "User deletion failed!" });
        });
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }

    public Task<AuthUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var query = @"";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new { });
        });
    }

    public Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetEmailAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetEmailConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedEmailAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetPhoneNumberAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> GetTwoFactorEnabledAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<AuthUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasPasswordAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFromRoleAsync(AuthUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailAsync(AuthUser user, string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(AuthUser user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedEmailAsync(AuthUser user, string normalizedEmail, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPasswordHashAsync(AuthUser user, string passwordHash, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPhoneNumberAsync(AuthUser user, string phoneNumber, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPhoneNumberConfirmedAsync(AuthUser user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetTwoFactorEnabledAsync(AuthUser user, bool enabled, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(AuthUser user, string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(AuthUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}