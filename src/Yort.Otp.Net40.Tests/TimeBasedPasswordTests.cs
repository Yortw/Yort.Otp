using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yort.Otp;

namespace Yort.Otp.Net40.Tests
{
	[TestClass]
	public class TimeBasedPasswordTests
	{
		// http://tools.ietf.org/html/rfc6238#page-3
		
		private static readonly byte[] TestSecretKeySha1	 = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };
		private static readonly byte[] TestSecretKeySha256 = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32 };
		private static readonly byte[] TestSecretKeySha512 = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30,
			0x31, 0x32, 0x33, 0x34
		};

		private static readonly int TestPasswordLength = 8;
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		#region SHA1 Tests

		[TestMethod]
		public void TimeBasedPassword_SHA1_59()
		{
			var expected = "94287082";
			var timeSinceEpoch = TimeSpan.FromSeconds(59);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 1);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA1_1111111109()
		{
			var expected = "07081804";
			var timeSinceEpoch = TimeSpan.FromSeconds(1111111109);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 1);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA1_1111111111()
		{
			var expected = "14050471";
			var timeSinceEpoch = TimeSpan.FromSeconds(1111111111);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 1);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA1_1234567890()
		{
			var expected = "89005924";
			var timeSinceEpoch = TimeSpan.FromSeconds(1234567890);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 1);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA1_2000000000()
		{
			var expected = "69279037";
			var timeSinceEpoch = TimeSpan.FromSeconds(2000000000);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 1);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA1_20000000000()
		{
			var expected = "65353130";
			var timeSinceEpoch = TimeSpan.FromSeconds(20000000000);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 1);
		}

		#endregion

		#region SHA256 Tests

		[TestMethod]
		public void TimeBasedPassword_SHA256_59()
		{
			var expected = "46119246";
			var timeSinceEpoch = TimeSpan.FromSeconds(59);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 2);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA256_1111111109()
		{
			var expected = "68084774";
			var timeSinceEpoch = TimeSpan.FromSeconds(1111111109);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 2);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA256_1111111111()
		{
			var expected = "67062674";
			var timeSinceEpoch = TimeSpan.FromSeconds(1111111111);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 2);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA256_1234567890()
		{
			var expected = "91819424";
			var timeSinceEpoch = TimeSpan.FromSeconds(1234567890);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 2);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA256_2000000000()
		{
			var expected = "90698825";
			var timeSinceEpoch = TimeSpan.FromSeconds(2000000000);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 2);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA256_20000000000()
		{
			var expected = "77737706";
			var timeSinceEpoch = TimeSpan.FromSeconds(20000000000);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 2);
		}

		#endregion

		#region SHA512 Tests

		[TestMethod]
		public void TimeBasedPassword_SHA512_59()
		{
			var expected = "90693936";
			var timeSinceEpoch = TimeSpan.FromSeconds(59);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 3);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA512_1111111109()
		{
			var expected = "25091201";
			var timeSinceEpoch = TimeSpan.FromSeconds(1111111109);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 3);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA512_1111111111()
		{
			var expected = "99943326";
			var timeSinceEpoch = TimeSpan.FromSeconds(1111111111);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 3);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA512_1234567890()
		{
			var expected = "93441116";
			var timeSinceEpoch = TimeSpan.FromSeconds(1234567890);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 3);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA512_2000000000()
		{
			var expected = "38618901";
			var timeSinceEpoch = TimeSpan.FromSeconds(2000000000);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 3);
		}

		[TestMethod]
		public void TimeBasedPassword_SHA512_20000000000()
		{
			var expected = "47863826";
			var timeSinceEpoch = TimeSpan.FromSeconds(20000000000);

			PerformTest(expected, GetExpectedUntil(timeSinceEpoch), timeSinceEpoch, 3);
		}

		#endregion

		private DateTime GetExpectedUntil(TimeSpan timeSinceEpoch)
		{
			var retVal =  UnixEpoch.Add(timeSinceEpoch);
			if (retVal.Second == 0)
				retVal = retVal.Subtract(TimeSpan.FromSeconds(1));
			else if (retVal.Second >= 30)
				retVal = retVal.Add(TimeSpan.FromSeconds(59 - retVal.Second));
			else
				retVal = retVal.Add(TimeSpan.FromSeconds(29 - retVal.Second));

			return retVal;
		}

		private static void PerformTest(string expected, DateTime expectedValidUntil, TimeSpan timeSinceEpoch, int hashLevel)
		{
			using (var otpg = new TimeBasedPasswordGenerator())
			{
				if (hashLevel == 2)
				{
					otpg.HashAlgorithm = new Sha256HashAlgorithm();
					otpg.SetSecret((byte[])TestSecretKeySha256.Clone());
				}
				else if (hashLevel == 3)
				{
					otpg.HashAlgorithm = new Sha512HashAlgorithm();
					otpg.SetSecret((byte[])TestSecretKeySha512.Clone());
				}
				else
				{
					otpg.HashAlgorithm = new Sha1HashAlgorithm();
					otpg.SetSecret((byte[])TestSecretKeySha1.Clone());
				}

				otpg.Timestamp = UnixEpoch.Add(timeSinceEpoch);
				otpg.PasswordLength = TestPasswordLength;
				Assert.AreEqual(expected, otpg.GeneratedPassword);
				Assert.AreEqual(expectedValidUntil, otpg.ValidUntilUtc);
			}
		}
	}
}
