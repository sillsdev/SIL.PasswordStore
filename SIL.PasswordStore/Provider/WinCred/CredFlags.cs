// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;

namespace SIL.Secrets.Provider.WinCred
{

	// https://docs.microsoft.com/en-us/windows/win32/api/wincred/ns-wincred-credentiala
	[Flags]
	internal enum CredFlags
	{
		PromptNow      = 0x02,
		UsernameTarget = 0x04,
	}
}