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
            var query = @"INSERT INTO public.authuser
                    (id, username, normalizedusername, email, normalizedemail
                    , emailconfirmed, passwordhash, securitystamp, phone
                    , phoneconfirmed, twofactorenabled, lockout, failedcount
                    , rowversion, createdby, updatedby)
                VALUES
	                (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail
                    , FALSE, @PasswordHash, @SecurityStamp, @Phone, FALSE, FALSE, FALSE, 0
                    , @RowVersion, @CreatedBy, @UpdatedBy);
            ";

            var cmd = await conn.ExecuteAsync(query, new
            {
                user.Id,
                user.UserName,
                user.NormalizedUserName,
                user.Email,
                user.NormalizedEmail,
                user.PasswordHash,
                user.SecurityStamp,
                user.Phone,
                user.RowVersion,
                user.CreatedBy,
                user.UpdatedBy
            });

            // TODO : Save Audit log

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
            var query = @"SELECT * FROM public.authuser WHERE email = @Email";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new { Email = normalizedEmail });
        });
    }

    public Task<AuthUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var query = @"SELECT * FROM public.authuser WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new { Id = userId });
        });
    }

    public Task<AuthUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var query = @"SELECT * FROM public.authuser WHERE username = @Username or normalizedusername = @Username";

            return await conn.QueryFirstOrDefaultAsync<AuthUser>(query, new { Username = normalizedUserName });
        });
    }

    public Task<string> GetEmailAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<string> GetNormalizedEmailAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task<string> GetNormalizedUserNameAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetPasswordHashAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<string> GetPhoneNumberAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Phone);
    }

    public Task<bool> GetPhoneNumberConfirmedAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PhoneConfirmed);
    }

    public Task<IList<string>> GetRolesAsync(AuthUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection<IList<string>>(async conn =>
        {
            var query = @"
                SELECT r.""name""
                FROM authrole r
                JOIN authuserrole ur ON r.id = ur.authrole
                WHERE ur.authuser = @Id
                ";

            var result = await conn.QueryAsync<string>(query, new { user.Id });
            return result.ToList();
        });
    }

    public Task<bool> GetTwoFactorEnabledAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.TwoFactorEnabled);
    }

    public Task<string> GetUserIdAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(AuthUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
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
        return Task.Run(() =>
        {
            user.NormalizedEmail = normalizedEmail;
        }, cancellationToken);
    }

    public Task SetNormalizedUserNameAsync(AuthUser user, string normalizedName, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            user.NormalizedUserName = normalizedName;
        }, cancellationToken);
    }

    public Task SetPasswordHashAsync(AuthUser user, string passwordHash, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            user.PasswordHash = passwordHash;
        }, cancellationToken);
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