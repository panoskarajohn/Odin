﻿namespace Shared.Security;

public interface IPasswordManager
{
    string Secure(string password);
    bool IsValid(string password, string securedPassword);
}