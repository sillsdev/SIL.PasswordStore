// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SIL.Secrets.Provider.WinCred
{

	internal sealed class CredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
	{
		// Set the handle.
		internal CredentialHandle(IntPtr preexistingHandle)
		{
			SetHandle(preexistingHandle);
		}

		internal Credential? GetCredential()
		{
			if (!IsInvalid)
			{
				return (Credential?)Marshal.PtrToStructure(handle, typeof(Credential));
			}

			throw new InvalidOperationException("Invalid CriticalHandle!");
		}

		protected override bool ReleaseHandle()
		{
			if (IsInvalid)
				return false;

			Native.CredFree(handle);
			SetHandleAsInvalid();
			return true;
		}
	}
}