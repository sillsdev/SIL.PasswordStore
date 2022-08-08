// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using SIL.Secrets.Provider;

namespace SIL.Secrets
{
	public static class PasswordStore
	{
		private static readonly IPasswordStoreImpl _provider;

#if NET461
		private static string _unixName = string.Empty;
		private static string UnixName
		{
			get
			{
				if (string.IsNullOrEmpty(_unixName))
				{
					IntPtr buf = IntPtr.Zero;
					try
					{
						buf = Marshal.AllocHGlobal(8192);
						// This is a hacktastic way of getting sysname from uname ()
						if (uname(buf) == 0)
							_unixName = Marshal.PtrToStringAnsi(buf);
					}
					catch
					{
						_unixName = String.Empty;
					}
					finally
					{
						if (buf != IntPtr.Zero)
							Marshal.FreeHGlobal(buf);
					}
				}

				return _unixName ?? string.Empty;
			}
		}

		[DllImport("libc")]
		private static extern int uname(IntPtr buf);
#endif

		static PasswordStore()
		{
#if NET461
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				if (UnixName == "Linux")
					_provider = new LinuxProvider();
				else if (UnixName == "Darwin")
					_provider = new MacProvider();
				else
					_provider = new FallbackProvider();
			}
			else
				_provider = new WindowsProvider();
#else
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				_provider = new LinuxProvider();
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				_provider = new MacProvider();
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				_provider = new WindowsProvider();
			else
			{
				Console.Error.WriteLine("Unknown platform");
				_provider = new FallbackProvider();
			}
#endif
		}

		[PublicAPI]
		public static void SetPassword([NotNull] string service, [CanBeNull] string user, [CanBeNull] string password)
		{
			_provider.SetPassword(service, user, password);
		}

		[PublicAPI]
		public static string? GetPassword([NotNull] string service, [CanBeNull] string user)
		{
			return _provider.GetPassword(service, user);
		}

		[PublicAPI]
		public static bool DeletePassword([NotNull] string service, [CanBeNull] string user)
		{
			return _provider.DeletePassword(service, user);
		}
	}
}
