// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

namespace SIL.Secrets.Provider;

using System;
using System.Runtime.InteropServices;

internal static class NativeMethods
{
	private const string LibSecret = "libsecret-1.so";

	internal struct GError
	{
		public int    domain;
		public int    code;
		public string message;
	}

	internal enum SecretSchemaAttributeType
	{
		SECRET_SCHEMA_ATTRIBUTE_STRING = 0,
		SECRET_SCHEMA_ATTRIBUTE_INTEGER = 1,
		SECRET_SCHEMA_ATTRIBUTE_BOOLEAN = 2,
	}

	internal struct SecretSchemaAttribute
	{
		public string name;
		public SecretSchemaAttributeType type;
	}

	internal enum SecretSchemaFlags {
		SECRET_SCHEMA_NONE = 0,
		SECRET_SCHEMA_DONT_MATCH_NAME = 1 << 1
	}

	[StructLayout(LayoutKind.Sequential, Size = 480)]
	public struct UnusedAttributes
	{
		// placeholder for 30 SecretSchemaAttribute elements (total of 32)
	}

	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct Reserved
	{
		// intentionally empty
	}

	[StructLayout(LayoutKind.Sequential, Size = 592, Pack = 8, CharSet = CharSet.Unicode)]
	internal class SecretSchema
	{
		public string?               name;
		public SecretSchemaFlags     flags;
		public SecretSchemaAttribute attribute1;
		public SecretSchemaAttribute attribute2;

		public UnusedAttributes unused;
		public Reserved         reserved;
	}

	[DllImport(LibSecret, CharSet = CharSet.Ansi)]
	internal static extern bool secret_password_store_sync(SecretSchema schema,
		[MarshalAs(UnmanagedType.LPStr)]
		string collection,
		[MarshalAs(UnmanagedType.LPStr)]
		string label,
		[MarshalAs(UnmanagedType.LPStr)]
		string password,
		IntPtr cancellable,
		ref IntPtr error,
		string serviceKey, string service, string userKey, string user, IntPtr args);

	[DllImport(LibSecret, CharSet = CharSet.Ansi)]
	internal static extern IntPtr secret_password_lookup_sync(SecretSchema schema,
		IntPtr cancellable,
		ref IntPtr error,
		string serviceKey, string service, string userKey, string user, IntPtr args);

	[DllImport(LibSecret, CharSet = CharSet.Ansi)]
	internal static extern void secret_password_free(IntPtr password);

	[DllImport(LibSecret, CharSet = CharSet.Ansi)]
	internal static extern bool secret_password_clear_sync(SecretSchema schema,
		IntPtr cancellable,
		ref IntPtr error,
		string serviceKey, string service, string userKey, string user, IntPtr args);
}
