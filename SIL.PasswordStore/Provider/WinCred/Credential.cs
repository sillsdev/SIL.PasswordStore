// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;

namespace SIL.Secrets.Provider.WinCred
{

// https://docs.microsoft.com/en-us/windows/win32/api/wincred/ns-wincred-credentialw

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

	/// <summary>
	/// An individual credential.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct Credential
	{
		/// <summary>
		/// A bit member that identifies characteristics of the credential. Undefined bits
		/// should be initialized as zero and not otherwise altered to permit future enhancement.
		/// </summary>
		public CredFlags Flags;

		/// <summary>
		/// The type of the credential. This member cannot be changed after the credential is created.
		/// </summary>
		public CredType Type;

		/// <summary>
		/// The name of the credential. The TargetName and Type members uniquely identify the
		/// credential. This member cannot be changed after the credential is created. Instead,
		/// the credential with the old name should be deleted and the credential with the new
		/// name created.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)] public string TargetName;

		/// <summary>
		/// A string comment from the user that describes this credential.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)] public string? Comment;

		/// <summary>
		/// The time, in Coordinated Universal Time (Greenwich Mean Time), of the last modification
		/// of the credential. For write operations, the value of this member is ignored.
		/// </summary>
		public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;

		/// <summary>
		/// The size, in bytes, of the CredentialBlob member.
		/// </summary>
		public uint CredentialBlobSize;

		/// <summary>
		/// Secret data for the credential. The CredentialBlob member can be both read and written.
		/// </summary>
		public IntPtr CredentialBlob;

		/// <summary>
		/// Defines the persistence of this credential. This member can be read and written.
		/// </summary>
		public CredPersist Persist;

		/// <summary>
		/// The number of application-defined attributes to be associated with the credential.
		/// </summary>
		public uint AttributeCount;

		/// <summary>
		/// Application-defined attributes that are associated with the credential. This member
		/// can be read and written.
		/// </summary>
		public IntPtr Attributes;

		/// <summary>
		/// Alias for the TargetName member. This member can be read and written.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)] public string TargetAlias;

		/// <summary>
		/// The user name of the account used to connect to TargetName.
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)] public string UserName;
	}
}
