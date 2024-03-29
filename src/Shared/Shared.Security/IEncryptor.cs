﻿namespace Shared.Security;

public interface IEncryptor
{
    string Encrypt(string data, string? key = null);
    string Decrypt(string data, string? key = null);
}