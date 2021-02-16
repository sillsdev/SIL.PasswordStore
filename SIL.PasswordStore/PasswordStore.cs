// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Runtime.InteropServices;
using SIL.Secrets.Provider;

namespace SIL.Secrets
{
	public static class PasswordStore
	{
		private static readonly IPasswordStoreImpl _provider;

		static PasswordStore()
		{
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
		}

		public static void SetPassword(string service, string user, string password)
		{
			_provider.SetPassword(service, user, password);
		}

		public static string? GetPassword(string service, string user)
		{
			return _provider.GetPassword(service, user);
		}

		public static bool DeletePassword(string service, string user)
		{
			return _provider.DeletePassword(service, user);
		}
	}
}
