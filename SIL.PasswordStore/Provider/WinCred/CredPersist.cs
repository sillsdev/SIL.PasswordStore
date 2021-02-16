// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

namespace SIL.Secrets.Provider.WinCred
{
	// https://docs.microsoft.com/en-us/windows/win32/api/wincred/ns-wincred-credentiala
	internal enum CredPersist
	{
		/// <summary>
		/// The credential persists for the life of the logon session. It will not be visible to
		/// other logon sessions of this same user. It will not exist after this user logs off
		/// and back on.
		/// </summary>
		Session = 1,

		/// <summary>
		/// The credential persists for all subsequent logon sessions on this same computer. It is
		/// visible to other logon sessions of this same user on this same computer and not visible
		/// to logon sessions for this user on other computers.
		/// </summary>
		LocalMachine = 2,

		/// <summary>
		/// The credential persists for all subsequent logon sessions on this same computer. It is
		/// visible to other logon sessions of this same user on this same computer and to logon
		/// sessions for this user on other computers.
		/// </summary>
		Enterprise = 3,
	}
}