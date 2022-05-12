using System;
namespace Keis.Data.Repository.EmailManager.Interfaces
{
	public interface IEmailConfiguration
	{
			public string ImapServer { get; set; }

			public int ImapPort { get; set; }

			public string ImapUsername { get; set; }

			public string ImapPassword { get; set; }
	}
}

