﻿using System.Security.Cryptography.X509Certificates;

namespace Shared.Security;

public interface ISigner
{
    string Sign(object context, X509Certificate2 certificate);
    bool Verify(object context, X509Certificate2 certificate, string signature);
}