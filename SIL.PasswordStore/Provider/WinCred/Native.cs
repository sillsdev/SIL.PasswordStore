// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;

namespace SIL.Secrets.Provider.WinCred
{
	internal static class Native
	{
		private const string Advapi32 = "Advapi32.dll";

		/// <summary>
		/// The CredRead function reads a credential from the user's credential set. The credential
		/// set used is the one associated with the logon session of the current token. The token
		/// must not have the user's SID disabled.
		/// </summary>
		/// <param name="targetName">Pointer to a null-terminated string that contains the name of
		/// the credential to read.</param>
		/// <param name="type">Type of the credential to read.</param>
		/// <param name="flags">Currently reserved and must be zero.</param>
		/// <param name="credential">Pointer to a single allocated block buffer to return the
		/// credential. Any pointers contained within the buffer are pointers to locations within
		/// this single allocated block. The single returned buffer must be freed by calling CredFree.</param>
		/// <returns>The function returns <c>true</c> on success and <c>false</c> on failure. The
		/// GetLastError function can be called to get a more specific status code.</returns>
		/// <see cref="https://docs.microsoft.com/en-us/windows/desktop/api/wincred/nf-wincred-credreadw"/>
		[DllImport(Advapi32, EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError =
			true)]
		public static extern bool CredRead(string targetName, CredType type, uint flags,
			out IntPtr                            credential);

		/// <summary>
		/// The CredWrite function creates a new credential or modifies an existing credential in
		/// the user's credential set. The new credential is associated with the logon session of
		/// the current token. The token must not have the user's security identifier (SID) disabled.
		/// </summary>
		/// <param name="credential">A pointer to the Credential structure to be written.</param>
		/// <param name="flags">Flags that control the function's operation.</param>
		/// <returns>If the function succeeds, the function returns <c>true</c>. If the function
		/// fails, it returns <c>false</c>. Call the GetLastError function to get a more specific
		/// status code.</returns>
		/// <see cref="https://docs.microsoft.com/en-us/windows/desktop/api/wincred/nf-wincred-credwritew"/>
		[DllImport(Advapi32, EntryPoint = "CredWriteW", CharSet = CharSet.Unicode,
			SetLastError = true)]
		public static extern bool CredWrite(ref Credential credential, CredPreserve flags);

		/// <summary>
		/// The CredDelete function deletes a credential from the user's credential set. The
		/// credential set used is the one associated with the logon session of the current token.
		/// The token must not have the user's SID disabled.
		/// </summary>
		/// <param name="targetName">Pointer to a null-terminated string that contains the name of
		/// the credential to delete.</param>
		/// <param name="type">Type of the credential to delete.</param>
		/// <param name="flags">Reserved and must be zero.</param>
		/// <returns>The function returns <c>true</c> on success and <c>false</c> on failure. The
		/// GetLastError function can be called to get a more specific status code.</returns>
		/// <see cref="https://docs.microsoft.com/en-us/windows/desktop/api/wincred/nf-wincred-creddeletew"/>
		[DllImport(Advapi32, EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode,
			SetLastError = true)]
		public static extern bool CredDelete(string targetName, CredType type, uint flags);

		/// <summary>
		/// The CredFree function frees a buffer returned by any of the credentials management
		/// functions.
		/// </summary>
		/// <param name="credential">Pointer to the buffer to be freed.</param>
		[DllImport(Advapi32, CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern void CredFree(IntPtr credential);
	}
}