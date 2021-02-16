using System;

namespace SIL.Secrets;

public class PasswordStoreException: ApplicationException
{
	public PasswordStoreException(int code, string? message = null, Exception? innerException = null): base(message, innerException)
	{
		Code = code;
	}

	public int Code { get; }
}