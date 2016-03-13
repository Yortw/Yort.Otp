using System;

namespace Yort.Otp
{
	/// <summary>
	/// Common interface for classes that can generate second authentication factor passwords.
	/// </summary>
	public interface IOnetimePasswordGenerator : IDisposable
	{
		/// <summary>
		/// Returns a string containing the current password.
		/// </summary>
		string GeneratedPassword { get; }

#pragma warning disable 1574

		/// <summary>
		/// Sets or returns the hash algorithm used with the secret to generate passwords.
		/// </summary>
		/// <seealso cref="Sha1HashAlgorithm"/>
		/// <seealso cref="Sha256HashAlgorithm"/>
		/// <seealso cref="Sha512HashAlgorithm"/>
		/// <seealso cref="MD5HashAlgorithm"/>
		IHashAlgorithm HashAlgorithm { get; set; }

#pragma warning restore 1574

		/// <summary>
		/// Sets or returns then length of password to generate. Default value is 6, usual values are 6 or 8.
		/// </summary>
		int PasswordLength { get; set; }

		/// <summary>
		/// Sets the secret value used to generate passwords.
		/// </summary>
		/// <param name="secret">A byte array containing the secret value used to generate passwords.</param>
		void SetSecret(byte[] secret);
	}
}