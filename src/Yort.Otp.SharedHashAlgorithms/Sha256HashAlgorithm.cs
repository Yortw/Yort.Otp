using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Yort.Otp
{
	/// <summary>
	/// The SHA256 hash algorithm wrapped in the <see cref="IHashAlgorithm"/> interface so it can be used across .Net frameworks.
	/// </summary>
	public class Sha256HashAlgorithm : IHashAlgorithm
	{
		/// <summary>
		/// Computes a new hash of <paramref name="data"/> usign the <paramref name="key"/> specified.
		/// </summary>
		/// <param name="key">A byte array containing the key (secret) to use to generate a hash.</param>
		/// <param name="data">A byte arrasy containing the data to be hashed.</param>
		/// <returns>A new byte array containing the hash result.</returns>
		public byte[] ComputeHash(byte[] key, byte[] data)
		{
			using (var hasher = new System.Security.Cryptography.HMACSHA256(key))
			{
				return hasher.ComputeHash(data);
			}
		}

		/// <summary>
		/// Returns the string "SHA256".
		/// </summary>
		public string Name { get { return "SHA256"; } }

	}
}