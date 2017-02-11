using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Otp.Net40.Tests
{
	[TestClass]
	public class OtpSecretTests
	{

		//Probably need some more tests here, but not sure what the test cases should be.

		[TestMethod]
		public void OtpSecret_FromHex_DecodesCorrectly()
		{
			var expected = new byte[] { 72,101, 108, 108, 111, 33, 222, 173, 190, 239 };
			var actual = OnetimePasswordSecret.FromHex("48656c6c6f21deadbeef");

			AssertSecretsAreEqual(expected, actual);
		}

		[TestMethod]
		public void OtpSecret_FromBase32_DecodesCorrectly()
		{
			var expected = new byte[] { 72, 101, 108, 108, 111, 33, 222, 173, 190, 239 };
			var actual = OnetimePasswordSecret.FromBase32("JBSWY3DPEHPK3PXP");

			AssertSecretsAreEqual(expected, actual);
		}

		[TestMethod]
		public void OtpSecret_FromBase32_WithPadding_DecodesCorrectly()
		{
			var expected = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
			var actual = OnetimePasswordSecret.FromBase32("JBSWY3DPEBLW64TMMQQQ====");

			AssertSecretsAreEqual(expected, actual);
		}

		[TestMethod]
		public void OtpSecret_FromAscii_DecodesCorrectly()
		{
			var expected = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };
			var actual = OnetimePasswordSecret.FromAscii("12345678901234567890");

			AssertSecretsAreEqual(expected, actual);
		}

		private void AssertSecretsAreEqual(byte[] expected, byte[] actual)
		{
			if (expected == actual) return;
			Assert.IsFalse(expected == null || actual == null, "One secret is null");
			Assert.AreEqual(expected.Length, actual.Length, "Secret lengths do not match.");

			for (int cnt = 0; cnt < expected.Length; cnt++)
			{
				Assert.AreEqual(expected[cnt], actual[cnt], "Secret contents do not match.");
			}
		}
	}
}