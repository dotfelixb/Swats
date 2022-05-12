using System;
using Keis.Model.Domain;
using static Keis.Model.Domain.EmailMessage;

namespace Keis.Data.Repository.EmailManager.Interfaces
{
	public interface IEmailRepository
	{
		Task<EmailResponse<IEnumerable<EmailMessage>>> GetEmails();
	}
}

