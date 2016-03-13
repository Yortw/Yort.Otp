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
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		/// <summary>
		/// Converts a string containing base 32 characters to a byte array suitable for use as a onetime password secret.
		/// </summary>
		/// <param name="base32">A string containing base 32 characters to convert to a secret.</param>
		/// <returns>A byte array that is the secret to use.</returns>
		public static byte[] FromBase32(string base32)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		/// <summary>
		/// Converts a raw secret (in byte array form) to a base 32 string.
		/// </summary>
		/// <param name="secret">A byte array containing the secret to convert.</param>
		/// <returns>A string containing the base 32 encoded version of the secret.</returns>
		internal static string ToBase32(byte[] secret)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		/// <summary>
		/// Converts a string containing ASCII characters to a byte array suitable for use as a onetime password secret.
		/// </summary>
		/// <param name="text">A string containing ASCII characters to convert to a secret.</param>
		/// <returns>A byte array that is the secret to use.</returns>
		public static byte[] FromAscii(string text)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

	}
}