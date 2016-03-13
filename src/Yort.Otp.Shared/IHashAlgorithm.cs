using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Otp
{
	/// <summary>
	/// Portable/cross framework interface for hash algorithms to be used with this library.
	/// </summary>
	public interface IHashAlgorithm
	{

		/// <summary>
		/// Computes a new hash of the <paramref name="data"/> value using the <pararef name="key"/>.
		/// </summary>
		/// <param name="key">A byte array containing the key (secret) to hash the data with.</param>
		/// <param name="data">A byte array that is the data to be hashed.</param>
		/// <returns>A new bte array containing the hashed data.</returns>
		byte[] ComputeHash(byte[] key, byte[] data);

		/// <summary>
		/// Returns a string containing the name of the hash algorithm, i,e SHA1.
		/// </summary>
		string Name { get; }

	}
}