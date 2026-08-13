// Copyright (c) 2022-2026 SIL Global
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using System.Text;
using SIL.Secrets.Provider.WinCred;

namespace SIL.Secrets.Provider
{

	internal class WindowsProvider : IPasswordStoreImpl
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

			var passwordByteLength = string.IsNullOrEmpty(password)
				? 0
				: Encoding.Unicode.GetByteCount(password);
			var credential = new Credential {
				Flags = 0,
				Type = CredType.Generic,
				TargetName = GetTargetName(service, user),
				Comment = null,
				CredentialBlobSize = (uint)passwordByteLength,
				CredentialBlob = Marshal.StringToCoTaskMemUni(password),
				Persist = CredPersist.LocalMachine,
				AttributeCount = 0
			};

			var result = Native.CredWrite(ref credential, 0);
			var error = Marshal.GetLastWin32Error();
			Marshal.FreeCoTaskMem(credential.CredentialBlob);
			if (result)
				return;

			throw new PasswordStoreException(error, $"CredWrite failed with 0x{Marshal.GetHRForLastWin32Error():x}");
		}

		public string? GetPassword(string service, string user)
		{
			if (string.IsNullOrEmpty(service))
				throw new ArgumentNullException(nameof(service));

			if (Native.CredRead(GetTargetName(service, user), CredType.Generic, 0,
					out var credPtr))
			{
				using var credentialHandle = new CredentialHandle(credPtr);
				var credential = credentialHandle.GetCredential();
				if (!credential.HasValue)
					return null;
				if (credential.Value.CredentialBlobSize == 0)
					return string.Empty;

				var charCount = checked((int)(credential.Value.CredentialBlobSize / sizeof(char)));
				var value = Marshal.PtrToStringUni(credential.Value.CredentialBlob, charCount);
				// Older versions of the Windows provider stored a wrong blob size, so it's
				// possible that we read more than the original password. Therefore we look for a
				// null character and return everything before it.
				var nulIndex = value?.IndexOf('\0') ?? -1;
				return nulIndex >= 0 ? value!.Substring(0, nulIndex) : value;
			}

			var error = Marshal.GetLastWin32Error();
			return error switch {
				ErrorNotFound => null,
				_ => throw new PasswordStoreException(error, $"CredRead failed with 0x{Marshal.GetHRForLastWin32Error():x}")
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
				_ => throw new PasswordStoreException(error, $"Can't delete password. Error 0x{Marshal.GetHRForLastWin32Error():x}")
			};
		}
	}
}
