using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Otp.Net40.Tests
{
	[TestClass]
	public class OtpGeneratorFactoryTests
	{

		private static readonly byte[] TestSecretKeySha1 = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30 };

		private static readonly byte[] TestSecretKeySha512 = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30,
			0x31, 0x32, 0x33, 0x34
		};

		[TestMethod]
		public void Generate_Totp_Sha1_30s_6d_Url()
		{
			var expected = "otpauth://totp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&period=30";
			var actual = Otp.OnetimePasswordGeneratorFactory.GetOtpUrl("test:testaccount@test.com", "SHA1", 6, TestSecretKeySha1, TimeSpan.FromSeconds(30), null);
			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void Generate_Totp_Sha512_60_8d_Url()
		{
			var expected = "otpauth://totp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNA&algorithm=SHA512&digits=8&period=60";
			var actual = Otp.OnetimePasswordGeneratorFactory.GetOtpUrl("test:testaccount@test.com", "SHA512", 8, TestSecretKeySha512, TimeSpan.FromSeconds(60), null);
			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void Generate_Totp_EncodesMetadata()
		{
			var expected = "otpauth://totp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&period=30&TestKey=TestValue&TestKey2=TestValue2";
			var metadata = new Dictionary<string, string>();
			metadata.Add("TestKey", "TestValue");
			metadata.Add("TestKey2", "TestValue2");

			var actual = Otp.OnetimePasswordGeneratorFactory.GetOtpUrl("test:testaccount@test.com", "SHA1", 6, TestSecretKeySha1, TimeSpan.FromSeconds(30), metadata);
			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void Parse_Totp_RetrievesMetadata()
		{
			var testUri = "otpauth://totp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&period=30&TestKey=TestValue&TestKey2=TestValue2";

			var actual = Otp.OnetimePasswordGeneratorFactory.FromOtpUrl(new Uri(testUri));
			var generator =  actual.PasswordGenerator as Otp.TimeBasedPasswordGenerator;

			Assert.IsNotNull(actual);
			Assert.IsNotNull(actual.Label);
			Assert.AreEqual("test:testaccount@test.com", actual.Label);
			Assert.IsNotNull(actual.Secret);
			Assert.AreEqual("GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ", Otp.OnetimePasswordSecret.ToBase32(actual.Secret));
			Assert.IsNotNull(actual.Metadata);
			Assert.AreEqual(3, actual.Metadata.Count);
			Assert.AreEqual("TestValue", actual.Metadata["TestKey"]);
			Assert.AreEqual("TestValue2", actual.Metadata["TestKey2"]);
			Assert.AreEqual(6, generator.PasswordLength);
			Assert.AreEqual("SHA1", generator.HashAlgorithm.Name);
			Assert.AreEqual(generator.TimeInterval.TotalSeconds, 30);
		}

		[TestMethod]
		public void Parse_Totp_RetrievesIssuerFromLabel()
		{
			var testUri = "otpauth://totp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&period=30";

			var actual = Otp.OnetimePasswordGeneratorFactory.FromOtpUrl(new Uri(testUri));

			Assert.AreEqual("test", actual.Metadata["issuer"]);
		}

		[TestMethod]
		public void Parse_Totp_ParsesCorrectlyWithNoIssuer()
		{
			var testUri = "otpauth://totp/testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&period=30";

			var actual = Otp.OnetimePasswordGeneratorFactory.FromOtpUrl(new Uri(testUri));
		}

		[TestMethod]
		public void Generate_Hotp_Sha1_30s_6d_10c_Url()
		{
			var expected = "otpauth://hotp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&counter=10";
			var actual = Otp.OnetimePasswordGeneratorFactory.GetOtpUrl("test:testaccount@test.com", "SHA1", 6, TestSecretKeySha1, 10, null);
			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void Generate_Hotp_Sha512_60_8d_10c_Url()
		{
			var expected = "otpauth://hotp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQGEZDGNA&algorithm=SHA512&digits=8&counter=10";
			var actual = Otp.OnetimePasswordGeneratorFactory.GetOtpUrl("test:testaccount@test.com", "SHA512", 8, TestSecretKeySha512, 10, null);
			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void Generate_Hotp_EncodesMetadata()
		{
			var expected = "otpauth://hotp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&counter=10&TestKey=TestValue&TestKey2=TestValue2";
			var metadata = new Dictionary<string, string>();
			metadata.Add("TestKey", "TestValue");
			metadata.Add("TestKey2", "TestValue2");

			var actual = Otp.OnetimePasswordGeneratorFactory.GetOtpUrl("test:testaccount@test.com", "SHA1", 6, TestSecretKeySha1, 10, metadata);
			Assert.AreEqual(expected, actual.ToString());
		}

		[TestMethod]
		public void Parse_Hotp_RetrievesMetadata()
		{
			var testUri = "otpauth://hotp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&counter=10&TestKey=TestValue&TestKey2=TestValue2";

			var actual = Otp.OnetimePasswordGeneratorFactory.FromOtpUrl(new Uri(testUri));
			var generator = actual.PasswordGenerator as Otp.CounterBasedPasswordGenerator;

			Assert.IsNotNull(actual);
			Assert.IsNotNull(actual.Label);
			Assert.AreEqual("test:testaccount@test.com", actual.Label);
			Assert.IsNotNull(actual.Secret);
			Assert.AreEqual("GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ", Otp.OnetimePasswordSecret.ToBase32(actual.Secret));
			Assert.IsNotNull(actual.Metadata);
			Assert.AreEqual(3, actual.Metadata.Count);
			Assert.AreEqual("TestValue", actual.Metadata["TestKey"]);
			Assert.AreEqual("TestValue2", actual.Metadata["TestKey2"]);
			Assert.AreEqual(6, generator.PasswordLength);
			Assert.AreEqual("SHA1", generator.HashAlgorithm.Name);
			Assert.AreEqual(10, generator.Counter);
		}

		[TestMethod]
		public void Parse_Hotp_RetrievesIssuerFromLabel()
		{
			var testUri = "otpauth://hotp/test:testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&counter=10";

			var actual = Otp.OnetimePasswordGeneratorFactory.FromOtpUrl(new Uri(testUri)) ;
			var generator = actual.PasswordGenerator as Otp.CounterBasedPasswordGenerator;

			Assert.AreEqual("test", actual.Metadata["issuer"]);
		}

		[TestMethod]
		public void Parse_Hotp_ParsesCorrectlyWithNoIssuer()
		{
			var testUri = "otpauth://hotp/testaccount@test.com?secret=GEZDGNBVGY3TQOJQGEZDGNBVGY3TQOJQ&algorithm=SHA1&digits=6&counter=10";

			var actual = Otp.OnetimePasswordGeneratorFactory.FromOtpUrl(new Uri(testUri));
			var generator = actual.PasswordGenerator as Otp.CounterBasedPasswordGenerator;
		}

	}
}