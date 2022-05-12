namespace Keis.Model;

public class ConnectionStringOptions
{
    public string Default { get; set; }
}

public class SecurityKeyOptions
{
    public string Bearer { get; set; }
}

public class EmailStringOptions
{
    public string ImapServer { get; set; }

    public string ImapPort { get; set; }

    public string ImapUsername { get; set; }

    public string ImapPassword { get; set; }
}