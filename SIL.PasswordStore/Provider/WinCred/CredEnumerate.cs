// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;

namespace SIL.Secrets.Provider.WinCred
{
	[Flags]
	internal enum CredEnumerate
	{
		None = 0x00,

		/// <summary>
		/// This function enumerates all of the credentials in the user's credential set. The target
		/// name of each credential is returned in the "namespace:attribute=target" format. If this
		/// flag is set and the Filter parameter is not <c>null</c>, the function fails and returns
		/// ERROR_INVALID_FLAGS.
		/// </summary>
		AllCredentials = 0x01,
	}
}