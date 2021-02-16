// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

namespace SIL.Secrets.Provider
{
	internal interface IPasswordStoreImpl
	{
		void SetPassword(string    service, string user, string password);
		string? GetPassword(string service, string user);
		bool DeletePassword(string service, string user);
	}
}
