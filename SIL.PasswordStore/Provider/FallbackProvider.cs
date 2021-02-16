// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

namespace SIL.Secrets.Provider
{
	internal class FallbackProvider: IPasswordStoreImpl
	{
		public void SetPassword(string    service, string user, string password)
		{
			throw new System.NotImplementedException();
		}

		public string GetPassword(string  service, string user)
		{
			throw new System.NotImplementedException();
		}

		public bool DeletePassword(string service, string user)
		{
			throw new System.NotImplementedException();
		}
	}
}
