// Copyright (c) 2022 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using NUnit.Framework;

namespace SIL.Secrets.Tests
{
	[TestFixture]
	[TestOf(typeof(PasswordStore))]
	public class PasswordStoreTests
	{
		private const string TestService  = "test-service";
		private const string TestUser     = "test-user";
		private const string TestPassword = "test-password";

		[TearDown]
		public void TearDown()
		{
			PasswordStore.DeletePassword(TestService, TestUser);
		}

		[Test]
		[Combinatorial]
		public void SetPassword(
			[Values(TestUser, "", null)] string user,
			[Values(TestPassword, "", null)] string password)
		{
			Assert.That(() => PasswordStore.SetPassword(TestService, user, password), Throws.Nothing);
		}

		[Test]
		[Combinatorial]
		public void SetPassword_NoService_Fails(
			[Values("", null)]  string service,
			[Values(TestUser, "", null)]     string user,
			[Values(TestPassword, "", null)] string password)
		{
			Assert.That(() => PasswordStore.SetPassword(service, user, password), Throws.ArgumentNullException);
		}

		[Test]
		public void SetPassword_UpdateExisting()
		{
			// Setup
			PasswordStore.SetPassword(TestService, TestUser, TestPassword);
			const string expected = "New password";

			// Execute
			Assert.That(() => PasswordStore.SetPassword(TestService, TestUser, expected), Throws.Nothing);

			// Verify
			var password = PasswordStore.GetPassword(TestService, TestUser);
			Assert.That(password, Is.EqualTo(expected));
		}

		[Test]
		public void GetPassword_Existing()
		{
			PasswordStore.SetPassword(TestService, TestUser, TestPassword);

			string password = null;
			Assert.That(() => password = PasswordStore.GetPassword(TestService, TestUser), Throws.Nothing);
			Assert.That(password, Is.EqualTo(TestPassword));
		}

		[Test]
		public void GetPassword_SingleLineHex()
		{
			const string expected = "abcdef1234567890abcdef0123456789";
			PasswordStore.SetPassword(TestService, TestUser, expected);

			string password = null;
			Assert.That(() => password = PasswordStore.GetPassword(TestService, TestUser), Throws.Nothing);
			Assert.That(password, Is.EqualTo(expected));
		}

		[Test]
		public void GetPassword_MultiLine()
		{
			const string expected = @"this password
has multiple
lines and will be
encoded by some keyring implementations
like osx";
			PasswordStore.SetPassword(TestService, TestUser, expected);

			string password = null;
			Assert.That(() => password = PasswordStore.GetPassword(TestService, TestUser), Throws.Nothing);
			Assert.That(password, Is.EqualTo(expected));
		}

		[Test]
		public void GetPassword_NonExisting()
		{
			string password = null;
			Assert.That(() => password = PasswordStore.GetPassword(TestService, TestUser + "NonExisting"),
				Throws.Nothing);
			Assert.That(password, Is.Null);
		}

		[Test]
		public void GetPassword_NoService_Fails()
		{
			PasswordStore.SetPassword(TestService, TestUser, TestPassword);
			Assert.That(() => PasswordStore.GetPassword(null, TestUser),
				Throws.ArgumentNullException);
		}

		[Test]
		public void DeletePassword_Existing()
		{
			// Setup
			PasswordStore.SetPassword(TestService, TestUser, TestPassword);

			// Execute
			bool result = false;
			Assert.That(() => result = PasswordStore.DeletePassword(TestService, TestUser), Throws.Nothing);

			// Verify
			Assert.That(result, Is.True);
			var password = PasswordStore.GetPassword(TestService, TestUser);
			Assert.That(password, Is.Null);
		}

		[Test]
		public void DeletePassword_NonExisting()
		{
			var result = false;
			Assert.That(() => result = PasswordStore.DeletePassword(TestService, TestUser + "NonExisting"), Throws.Nothing);
			Assert.That(result, Is.False);
		}

		[Test]
		public void DeletePassword_NoService_Fails()
		{
			PasswordStore.SetPassword(TestService, TestUser, TestPassword);
			Assert.That(() => PasswordStore.DeletePassword(null, TestUser),
				Throws.ArgumentNullException);
		}

	}
}
