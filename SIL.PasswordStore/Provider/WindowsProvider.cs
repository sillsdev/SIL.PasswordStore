// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using System.Text;
using SIL.Secrets.Provider.WinCred;

namespace SIL.Secrets.Provider;

internal class WindowsProvider: IPasswordStoreImpl
{
	private const int ErrorNotFound = 1168;

	private static string GetTargetName(string service, string user)
	{
		return $"{service}:{user}";
	}

	public void SetPassword(string service, string user, string password)
	{
		if (string.IsNullOrEmpty(service))
			throw new ArgumentNullException(nameof(service));

		var passwordLength = string.IsNullOrEmpty(password)
			? 0
			: (uint)Encoding.UTF8.GetBytes(password).Length + 1;
		var credential = new Credential {
			Flags = 0,
			Type = CredType.Generic,
			TargetName = GetTargetName(service, user),
			Comment = string.Empty,
			CredentialBlobSize = passwordLength * 2,
			CredentialBlob = Marshal.StringToCoTaskMemUni(password),
			Persist = CredPersist.LocalMachine,
			AttributeCount = 0
		};

		var result = Native.CredWrite(ref credential, 0);
		var error = Marshal.GetLastWin32Error();
		Marshal.FreeCoTaskMem(credential.CredentialBlob);
		if (result)
			return;

		throw new PasswordStoreException(error, "CredWrite failed");
	}

	public string? GetPassword(string  service, string user)
	{
		if (string.IsNullOrEmpty(service))
			throw new ArgumentNullException(nameof(service));

		if (Native.CredRead(GetTargetName(service, user), CredType.Generic, 0,
				out var credPtr))
		{
			using var credentialHandle = new CredentialHandle(credPtr);
			var credential = credentialHandle.GetCredential();
			if (credential == null || credential?.CredentialBlobSize == 0)
				return null;

			return Marshal.PtrToStringUni(credential?.CredentialBlob ?? IntPtr.Zero);
		}
		var error = Marshal.GetLastWin32Error();
		return error switch {
			ErrorNotFound => null,
			_ => throw new PasswordStoreException(error, "CredRead failed")
		};
	}

	public bool DeletePassword(string service, string user)
	{
		if (string.IsNullOrEmpty(service))
			throw new ArgumentNullException(nameof(service));

		if (Native.CredDelete(GetTargetName(service, user), CredType.Generic, 0))
			return true;

		var error = Marshal.GetLastWin32Error();
		return error switch {
			ErrorNotFound => false,
			_ => throw new PasswordStoreException(error, "Can't delete password")
		};
	}
}
