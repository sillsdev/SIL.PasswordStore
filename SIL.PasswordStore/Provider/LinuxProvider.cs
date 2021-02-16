// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;

namespace SIL.Secrets.Provider;
internal class LinuxProvider: IPasswordStoreImpl
{
	private readonly NativeMethods.SecretSchema _schema;
	private const    string                     SecretCollectionDefault = "default";

	public LinuxProvider()
	{
		_schema = new NativeMethods.SecretSchema {
			name = "io.github.ermshiperete.passwords",
			flags = NativeMethods.SecretSchemaFlags.SECRET_SCHEMA_NONE,
			attribute1 = new NativeMethods.SecretSchemaAttribute() {
				name = "service",
				type = NativeMethods.SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING
			},
			attribute2 = new NativeMethods.SecretSchemaAttribute() {
				name = "user",
				type = NativeMethods.SecretSchemaAttributeType.SECRET_SCHEMA_ATTRIBUTE_STRING
			},
		};
	}

	public void SetPassword(string service, string user, string password)
	{
		if (string.IsNullOrEmpty(service))
			throw new ArgumentNullException(nameof(service));

		var errorPtr = new IntPtr();
		NativeMethods.secret_password_store_sync(_schema, SecretCollectionDefault, user,
				password, IntPtr.Zero, ref errorPtr,
				"service", service, "user", user, IntPtr.Zero);
		if (errorPtr == IntPtr.Zero)
			return;

		var error = Marshal.PtrToStructure<NativeMethods.GError>(errorPtr);
		throw new PasswordStoreException(error.code, error.message);
	}

	public string? GetPassword(string service, string user)
	{
		if (string.IsNullOrEmpty(service))
			throw new ArgumentNullException(nameof(service));

		var errorPtr = new IntPtr();
		var passwordPtr = NativeMethods.secret_password_lookup_sync(_schema, IntPtr.Zero, ref errorPtr,
			"service", service, "user", user, IntPtr.Zero);
		if (errorPtr == IntPtr.Zero)
		{
			var password = Marshal.PtrToStringAuto(passwordPtr);
			NativeMethods.secret_password_free(passwordPtr);
			return password;
		}

		var error = Marshal.PtrToStructure<NativeMethods.GError>(errorPtr);
		throw new PasswordStoreException(error.code, error.message);
	}

	public bool DeletePassword(string service, string user)
	{
		if (string.IsNullOrEmpty(service))
			throw new ArgumentNullException(nameof(service));

		var errorPtr = new IntPtr();
		var success = NativeMethods.secret_password_clear_sync(_schema, IntPtr.Zero, ref errorPtr,
			"service", service, "user", user, IntPtr.Zero);
		if (errorPtr == IntPtr.Zero)
		{
			return success;
		}

		var error = Marshal.PtrToStructure<NativeMethods.GError>(errorPtr);
		throw new PasswordStoreException(error.code, error.message);
	}
}
