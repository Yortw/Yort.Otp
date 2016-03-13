using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Otp.Net40.Tests
{
	[TestClass]
	public class CounterBasePasswordTests
	{
		// https://tools.ietf.org/html/rfc4226#page-11

		private readonly byte[] Sha1Secret = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };

		[TestMethod]
		public void OneTimePassword_SHA1_0()
		{
			PerformTest(0, "755224");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_1()
		{
			PerformTest(1, "287082");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_2()
		{
			PerformTest(2, "359152");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_3()
		{
			PerformTest(3, "969429");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_4()
		{
			PerformTest(4, "338314");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_5()
		{
			PerformTest(5, "254676");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_6()
		{
			PerformTest(6, "287922");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_7()
		{
			PerformTest(7, "162583");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_8()
		{
			PerformTest(8, "399871");
		}

		[TestMethod]
		public void OneTimePassword_SHA1_9()
		{
			PerformTest(9, "520489");
		}

		private void PerformTest(int counter, string expected)
		{
			using (var passwordGenerator = new CounterBasedPasswordGenerator(false, Sha1Secret))
			{
				passwordGenerator.Counter = counter;
				Assert.AreEqual(expected, passwordGenerator.GeneratedPassword);
			}
		}

		/*
		Count Hexadecimal    Decimal HOTP
   0        4c93cf18       1284755224     755224
   1        41397eea       1094287082     287082
   2         82fef30        137359152     359152
   3        66ef7655       1726969429     969429
   4        61c5938a       1640338314     338314
   5        33c083d4        868254676     254676
   6        7256c032       1918287922     287922
   7         4e5b397         82162583     162583
   8        2823443f        673399871     399871
   9        2679dc69        645520489     520489
	 */

	}
}