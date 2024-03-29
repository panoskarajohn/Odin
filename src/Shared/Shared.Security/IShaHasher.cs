﻿namespace Shared.Security;

public interface IShaHasher
{
    string Sha256(string data);
    string Sha512(string data);
}