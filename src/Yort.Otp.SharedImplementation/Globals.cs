using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Otp
{
	internal static class Globals
	{

		internal static readonly DateTime UnixEpochUtc = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc);
		internal const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
		internal static readonly int[] DigitPowers = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

		internal const string OtpScheme = "otpauth";

		internal const string TotpAuthority = "totp";
		internal const string HotpAuthority = "hotp";

	}
}