using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Otp
{
	/// <summary>
	/// Factory that makes instantiating and configuring onetime password generators more concise.
	/// Also provides methods for creating password generators from otp urls, and generating new urls.
	/// </summary>
	/// <seealso cref="TimeBasedPasswordGenerator"/>
	/// <seealso cref="CounterBasedPasswordGenerator"/>
	public class OnetimePasswordGeneratorFactory
	{

		#region Static Implementation

		/// <summary>
		/// Returns a factory instance that creates pre-configured <see cref="CounterBasedPasswordGenerator"/> instances.
		/// </summary>
		/// <param name="eraseSecrets">True if the secrets provided to the generated <see cref="CounterBasedPasswordGenerator"/> instances should be overwritten in memory when the generator is disposed.</param>
		/// <param name="hashAlgorithm">A <see cref="IHashAlgorithm"/> implementation that will be used by the <see cref="CounterBasedPasswordGenerator"/> instances to generate passwords.</param>
		/// <param name="passwordLength">The number of digits in the generated passwords, default is 6, usual values are 6 or 8.</param>
		/// <returns>A <see cref="OnetimePasswordGeneratorFactory"/> instance that can be used to create <see cref="CounterBasedPasswordGenerator"/> instances.</returns>
		/// <seealso cref="OnetimePasswordGeneratorFactory"/>
		public static OnetimePasswordGeneratorFactory CreateFactory(bool eraseSecrets, IHashAlgorithm hashAlgorithm, int passwordLength)
		{
			return new OnetimePasswordGeneratorFactory(eraseSecrets, hashAlgorithm, passwordLength);
		}

		/// <summary>
		/// Returns a factory instance that creates pre-configured <see cref="TimeBasedPasswordGenerator"/> instances.
		/// </summary>
		/// <param name="eraseSecrets">True if the secrets provided to the generated <see cref="TimeBasedPasswordGenerator"/> instances should be overwritten in memory when the generator is disposed.</param>
		/// <param name="hashAlgorithm">A <see cref="IHashAlgorithm"/> implementation that will be used by the <see cref="TimeBasedPasswordGenerator"/> instances to generate passwords.</param>
		/// <param name="passwordLength">The number of digits in the generated passwords, default is 6, usual values are 6 or 8.</param>
		/// <param name="timeInterval">The interval for which each password is valid, the default is 30 seconds.</param>
		/// <returns>A <see cref="OnetimePasswordGeneratorFactory"/> instance that can be used to create <see cref="TimeBasedPasswordGenerator"/> instances.</returns>
		/// <seealso cref="TimeBasedPasswordGenerator"/>
		public static OnetimePasswordGeneratorFactory CreateFactory(bool eraseSecrets, IHashAlgorithm hashAlgorithm, int passwordLength, TimeSpan timeInterval)
		{
			return new OnetimePasswordGeneratorFactory(eraseSecrets, hashAlgorithm, passwordLength, timeInterval);
		}

		/// <summary>
		/// Creates either a <see cref="TimeBasedPasswordGenerator"/> or <see cref="CounterBasedPasswordGenerator"/> instance pre-configured with values from an otp style uri.
		/// </summary>
		/// <remarks>
		/// See https://github.com/google/google-authenticator/wiki/Key-Uri-Format for details of the uri format.
		/// </remarks>
		/// <param name="url">The url to parse and turn into a password generator.</param>
		/// <returns>A pre-configured instance of either a <see cref="TimeBasedPasswordGenerator"/> or <see cref="CounterBasedPasswordGenerator"/>.</returns>
		/// <seealso cref="GetOtpUrl(string, string, int, byte[], long, IDictionary{string, string})"/>
		/// <seealso cref="GetOtpUrl(string, string, int, byte[], TimeSpan, IDictionary{string, string})"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "3")]
		public static OnetimePasswordAccount FromOtpUrl(Uri url)
		{
			if (url == null) throw new ArgumentNullException(nameof(url));

			if (String.Compare(url.Scheme, Globals.OtpScheme, StringComparison.OrdinalIgnoreCase) != 0) throw new ArgumentException($"Url authority is not {Globals.OtpScheme}.", nameof(url));

			if (url.Segments.Length < 2) throw new ArgumentException("Url does not contain sufficient segments for an otp url.", nameof(url));

			var passwordType = url.Authority;
			if (String.Compare(passwordType, Globals.HotpAuthority, StringComparison.OrdinalIgnoreCase) != 0 && String.Compare(passwordType, Globals.TotpAuthority, StringComparison.OrdinalIgnoreCase) != 0)
				throw new ArgumentException("Url contains an unknown password type.", nameof(url));

			var metadata = new Dictionary<string, string>();
			byte[] secret = null;
			var label = Uri.UnescapeDataString(url.Segments[1]);
			if (label.Contains(":"))
			{
				metadata["issuer"] = label.Substring(0, label.IndexOf(":"));
			}
			var queryParts = url.Query.Substring(1).Split('&');

			var passwordGenerator = (String.Compare(passwordType, Globals.HotpAuthority, StringComparison.OrdinalIgnoreCase) == 0 ? (IOnetimePasswordGenerator)new CounterBasedPasswordGenerator() : (IOnetimePasswordGenerator)new TimeBasedPasswordGenerator());
			foreach (var queryPart in queryParts)
			{
				var queryOption = queryPart.Split('=');
				var key = Uri.UnescapeDataString(queryOption[0]);
				var value = Uri.UnescapeDataString(queryOption[1]);

				switch (key.ToUpperInvariant())
				{
					case "ALGORITHM":
						passwordGenerator.HashAlgorithm = HasherFromName(value) ?? passwordGenerator.HashAlgorithm;
						break;

					case "DIGITS":
						passwordGenerator.PasswordLength = Convert.ToInt32(value);
						break;

					case "COUNTER":
						((CounterBasedPasswordGenerator)passwordGenerator).Counter = Convert.ToInt64(value);
						break;

					case "PERIOD":
					case "INTERVAL":
						((TimeBasedPasswordGenerator)passwordGenerator).TimeInterval = TimeSpan.FromSeconds(Convert.ToInt64(value));
						break;

					case "SECRET":
						secret = OnetimePasswordSecret.FromBase32(value);
						passwordGenerator.SetSecret(secret);
						break;

					default:
						metadata[key] = value;
						break;
				}
			}

			var retVal = new OnetimePasswordAccount()
			{
				Label = label,
				Secret = secret,
				Metadata = metadata,
				Issuer = metadata.ContainsKey("Issuer") ? metadata["Issuer"] : null,
				PasswordGenerator = passwordGenerator
			};

			return retVal;
		}

		/// <summary>
		/// Creates a new <seealso cref="System.Uri"/> that can be used to configure an authentication app. These codes are normally used as the content of a QR code.
		/// </summary>
		/// <param name="label">The name of the account or resource protected by this secret.</param>
		/// <param name="algorithmName">The name of the algorithm used with the secret to generate passwords.</param>
		/// <param name="passwordLength">The length of the password, default is 6, usual values are 6 or 8.</param>
		/// <param name="secret">The secret used to protect this account.</param>
		/// <param name="counter">The initial counter to use when generating passwords for this account.</param>
		/// <param name="metadata">A dictionary containing additional metadata to be encoded into the url. If this value is null or empty, no additional values are added.</param>
		/// <returns>A new <seealso cref="System.Uri"/> that can be used to configure an authentication app to generate compatible passwords.</returns>
		/// <seealso cref="FromOtpUrl(Uri)"/>
		public static Uri GetOtpUrl(string label, string algorithmName, int passwordLength, byte[] secret, Int64 counter, IDictionary<string, string> metadata)
		{
			return BuildUri(label, Globals.HotpAuthority, algorithmName, passwordLength, secret, EncodeAdditionalArguments($"counter={counter}", metadata));
		}

		/// <summary>
		/// Creates a new <seealso cref="System.Uri"/> that can be used to configure an authentication app. These codes are normally used as the content of a QR code.
		/// </summary>
		/// <param name="label">The name of the account or resource protected by this secret.</param>
		/// <param name="algorithmName">The name of the algorithm used with the secret to generate passwords.</param>
		/// <param name="passwordLength">The length of the password, default is 6, usual values are 6 or 8.</param>
		/// <param name="secret">The secret used to protect this account.</param>
		/// <param name="timeInterval">The time interval each generated password if valid for. Default value is 30.</param>
		/// <param name="metadata">A dictionary containing additional metadata to be encoded into the url. If this value is null or empty, no additional values are added.</param>
		/// <returns>A new <seealso cref="System.Uri"/> that can be used to configure an authentication app to generate compatible passwords.</returns>
		/// <seealso cref="FromOtpUrl(Uri)"/>
		public static Uri GetOtpUrl(string label, string algorithmName, int passwordLength, byte[] secret, TimeSpan timeInterval, IDictionary<string, string> metadata)
		{
			return BuildUri(label, Globals.TotpAuthority, algorithmName, passwordLength, secret, EncodeAdditionalArguments($"period={(timeInterval == TimeSpan.Zero ? TimeSpan.FromSeconds(30) : timeInterval).TotalSeconds}", metadata));
		}

		private static Uri BuildUri(string label, string passwordType, string algorithmName, int passwordLength, byte[] secret, string additionalQueryArguments)
		{
			string base32Secret = OnetimePasswordSecret.ToBase32(secret);

			var uriString = $"{Globals.OtpScheme}://{passwordType}/{label}?secret={Uri.EscapeDataString(base32Secret)}&algorithm={Uri.EscapeDataString(algorithmName)}&digits={passwordLength}";
			if (!String.IsNullOrEmpty(additionalQueryArguments))
				uriString += "&" + additionalQueryArguments;

			return new Uri(uriString, UriKind.Absolute);
		}

		private static IHashAlgorithm HasherFromName(string name)
		{
			switch (name.ToUpperInvariant())
			{
				case "SHA1":
					return new Sha1HashAlgorithm();

				case "SHA256":
					return new Sha256HashAlgorithm();

				case "SHA512":
#if SUPPORTS_SHA512
					return new Sha512HashAlgorithm();
#else
					throw new NotImplementedException("SHA512 is not supported on this platform.");
#endif

				case "MD5":
#if SUPPORTS_MD5
					return new MD5HashAlgorithm();
#else
					throw new NotImplementedException("MD5 is not supported on this platform.");
#endif
			}

			return null;
		}

		private static string EncodeAdditionalArguments(string baseArguments, IDictionary<string, string> metadata)
		{
			var retVal = new StringBuilder(baseArguments);
			if (metadata != null)
			{
				foreach (var kvp in metadata)
				{
					if (retVal.Length > 0)
						retVal.Append("&");

					retVal.Append(Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value));
				}
			}
			return retVal.ToString();
		}

		#endregion

		#region Instance Implementation

		private Func<byte[], Int64, IOnetimePasswordGenerator> _FactoryFunc;

		private OnetimePasswordGeneratorFactory(bool eraseSecrets, IHashAlgorithm hashAlgorithm, int passwordLength)
		{
			_FactoryFunc = (secret, counter) => new CounterBasedPasswordGenerator(eraseSecrets, secret)
			{
				Counter = counter,
				HashAlgorithm = hashAlgorithm,
				PasswordLength = passwordLength
			};
		}

		private OnetimePasswordGeneratorFactory(bool eraseSecrets, IHashAlgorithm hashAlgorithm, int passwordLength, TimeSpan timeInterval)
		{
			_FactoryFunc = (secret, counter) => new TimeBasedPasswordGenerator(eraseSecrets, secret)
			{
				TimeInterval = timeInterval,
				HashAlgorithm = hashAlgorithm,
				PasswordLength = passwordLength
			};
		}

		/// <summary>
		/// Returns a time based password generator.
		/// </summary>
		/// <param name="secret">The secret to use for the new password generator.</param>
		/// <returns>A pre-configured <seealso cref="TimeBasedPasswordGenerator"/> instance.</returns>
		public IOnetimePasswordGenerator CreateNewPasswordGenerator(byte[] secret)
		{
			return _FactoryFunc(secret, 0);
		}

		/// <summary>
		/// Returns a counter based password generator.
		/// </summary>
		/// <param name="secret">The secret to use for the new password generator.</param>
		/// <param name="counter">The current counter to use for the new password  generator.</param>
		/// <returns>A pre-configured <seealso cref="CounterBasedPasswordGenerator"/> instance.</returns>
		public IOnetimePasswordGenerator CreateNewPasswordGenerator(byte[] secret, Int64 counter)
		{
			return _FactoryFunc(secret, counter);
		}

		#endregion

	}
}