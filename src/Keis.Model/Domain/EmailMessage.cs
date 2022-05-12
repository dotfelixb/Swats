using System;
namespace Keis.Model.Domain
{
	public class EmailMessage
	{
        public class EmailAddress
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public class EmailResponse<T>
        {
            public T Data { get; set; }
            public string Error { get; set; }
        }

        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }
        public string Id { get; set; }
        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}

