﻿namespace Swats.Model.Commands;

public class LoginCommand
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}