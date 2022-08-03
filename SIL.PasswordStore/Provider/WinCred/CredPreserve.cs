// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

namespace SIL.Secrets.Provider.WinCred
{

	internal enum CredPreserve
	{
		/// <summary>
		/// The credential BLOB from an existing credential is preserved with the same credential
		/// name and credential type. The CredentialBlobSize of the passed in Credential structure
		/// must be zero.
		/// </summary>
		CredentialBlob = 1,
	}
}
