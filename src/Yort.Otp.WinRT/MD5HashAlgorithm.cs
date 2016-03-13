using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Yort.Otp
{
	/// <summary>
	/// The MD5 hash algorithm wrapped in the <see cref="IHashAlgorithm"/> interface so it can be used across .Net frameworks.
	/// </summary>
	public class MD5HashAlgorithm : IHashAlgorithm
	{

		MacAlgorithmProvider _MacProvider = MacAlgorithmProvider.OpenAlgorithm(Windows.Security.Cryptography.Core.MacAlgorithmNames.HmacMd5);

		/// <summary>
		/// Computes a new hash of <paramref name="data"/> usign the <paramref name="key"/> specified.
		/// </summary>
		/// <param name="key">A byte array containing the key (secret) to use to generate a hash.</param>
		/// <param name="data">A byte arrasy containing the data to be hashed.</param>
		/// <returns>A new byte array containing the hash result.</returns>
		public byte[] ComputeHash(byte[] key, byte[] data)
		{
			var keyBuffer = CryptographicBuffer.CreateFromByteArray(key);
			var cryptoKey = _MacProvider.CreateKey(keyBuffer);

			var dataBuffer = CryptographicBuffer.CreateFromByteArray(data);

			var result = CryptographicEngine.Sign(cryptoKey, dataBuffer);

			// Verify that the HMAC length is correct for the selected algorithm
			if (result.Length != _MacProvider.MacLength) throw new InvalidOperationException("Error computing digest");

			return result.ToArray();
		}

		/// <summary>
		/// Returns the string "MD5".
		/// </summary>
		public string Name { get { return "MD5"; } }

	}
}