// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;

namespace SIL.Secrets.Provider.WinCred
{
	// https://docs.microsoft.com/en-us/windows/win32/api/wincred/ns-wincred-credentiala
	internal enum CredType
	{
		/// <summary>
		/// The credential is a generic credential. The credential will not be used by any particular
		/// authentication package. The credential will be stored securely but has no other
		/// significant characteristics.
		/// </summary>
		Generic = 1,

		/// <summary>
		/// The credential is a password credential and is specific to Microsoft's authentication
		/// packages. The NTLM, Kerberos, and Negotiate authentication packages will automatically
		/// use this credential when connecting to the named target.
		/// </summary>
		DomainPassword = 2,

		/// <summary>
		/// The credential is a certificate credential and is specific to Microsoft's authentication
		/// packages. The Kerberos, Negotiate, and Schannel authentication packages automatically
		/// use this credential when connecting to the named target.
		/// </summary>
		DomainCertificate = 3,

		/// <summary>
		/// This value is no longer supported.
		/// </summary>
		DomainVisiblePassword = 4,

		/// <summary>
		/// The credential is a certificate credential that is a generic authentication package.
		/// </summary>
		GenericCertificate = 5,

		/// <summary>
		/// The credential is supported by extended Negotiate packages.
		/// </summary>
		DomainExtended = 6,

		/// <summary>
		/// The maximum number of supported credential types.
		/// </summary>
		Maximum = 7,

		/// <summary>
		/// The extended maximum number of supported credential types that now allow new applications
		/// to run on older operating systems.
		/// </summary>
		MaximumEx = Maximum + 1000,
	}
}