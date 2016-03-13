using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Otp
{
	/// <summary>
	/// A static class containing utility methods useful for converting onetime password secrets to and from various formats.
	/// </summary>
	public static class OnetimePasswordSecret
	{

		/// <summary>
		/// Converts a string containing hexadecimal characters to a byte array suitable for use as a onetime password secret.
		/// </summary>
		/// <param name="hex">A string containing hexadecimal characters to convert to a secret.</param>
		/// <returns>A byte array that is the secret to use.</returns>
		public static byte[] FromHex(string hex)
		{
			if (hex == null) throw new ArgumentNullException(nameof(hex));

			int NumberChars = hex.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			return bytes;
		}

		/// <summary>
		/// Converts a string containing base 32 characters to a byte array suitable for use as a onetime password secret.
		/// </summary>
		/// <param name="base32">A string containing base 32 characters to convert to a secret.</param>
		/// <returns>A byte array that is the secret to use.</returns>
		public static byte[] FromBase32(string base32)
		{
			return Base32.FromBase32String(base32);

			//TODO: Remove this if we get approval to use the code in Base32, stolen shamelessly from the web.
		//	if (base32 == null) throw new ArgumentNullException(nameof(base32));

		//	var bits = new StringBuilder(base32.Length * 5);
		//	for (var i = 0; i < base32.Length; i++)
		//	{
		//		var val = Globals.Base32Chars.IndexOf(Char.ToUpper(base32[i]));
		//		bits.Append(Convert.ToString(val, 2).PadLeft(5, '0'));
		//	}

		//	var bitsStr = bits.ToString();
		//	var sb = new StringBuilder();
		//	for (var i = 0; i + 4 <= bits.Length; i += 4)
		//	{
		//		var chunk = bitsStr.Substring(i, 4);
		//		sb.Append(Convert.ToInt32(chunk, 2).ToString("X"));
		//	}

		//	return FromHex(sb.ToString().ToLower());
		}

		/// <summary>
		/// Converts a raw secret (in byte array form) to a base 32 string.
		/// </summary>
		/// <param name="secret">A byte array containing the secret to convert.</param>
		/// <returns>A string containing the base 32 encoded version of the secret.</returns>
		public static string ToBase32(byte[] secret)
		{
			return Base32.ToBase32String(secret);

			//TODO: Remove this if we get approval to use the code in Base32, stolen shamelessly from the web.
			//if (secret == null) throw new ArgumentNullException(nameof(secret));

			//var bits = new StringBuilder(secret.Length * 5);
			//for (var i = 0; i < secret.Length; i++)
			//{
			//	var val = Globals.Base32Chars.IndexOf(((char)secret[i]).ToString().ToUpperInvariant());
			//	bits.Append(Convert.ToString(val, 2).PadLeft(5, '0'));
			//}

			//var bitsStr = bits.ToString();
			//var sb = new StringBuilder();
			//for (var i = 0; i + 4 <= bits.Length; i += 4)
			//{
			//	var chunk = bitsStr.Substring(i, 4);
			//	sb.Append(Convert.ToInt32(chunk, 2).ToString("X"));
			//}

			//return FromHex(sb.ToString().ToLower());
		}

		/// <summary>
		/// Converts a string containing ASCII characters to a byte array suitable for use as a onetime password secret.
		/// </summary>
		/// <param name="text">A string containing ASCII characters to convert to a secret.</param>
		/// <returns>A byte array that is the secret to use.</returns>
		public static byte[] FromAscii(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));

			var sb = new StringBuilder(text.Length * 2);
			foreach (byte b in text)
			{
				sb.AppendFormat("{0:X}", b);
			}
			return FromHex(sb.ToString());
		}

	}
}