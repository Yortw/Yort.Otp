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
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
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
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		/// <summary>
		/// Creates either a <see cref="TimeBasedPasswordGenerator"/> or <see cref="CounterBasedPasswordGenerator"/> instance pre-configured with values from an otp style uri.
		/// </summary>
		/// <remarks>
		/// See https://github.com/google/google-authenticator/tree/master/mobile/ios for details of the uri format.
		/// </remarks>
		/// <param name="url">The url to parse and turn into a password generator.</param>
		/// <param name="label">An out parameter returning the name of the account or resource accessed using the generated passwords.</param>
		/// <param name="secret">An out paramete returning the secret used by the generator.</param>
		/// <param name="metadata">A dictionary containing additional metadata retrieved from the URL.</param>
		/// <returns>A pre-configured instance of either a <see cref="TimeBasedPasswordGenerator"/> or <see cref="CounterBasedPasswordGenerator"/>.</returns>
		/// <seealso cref="GetOtpUrl(string, string, int, byte[], Int64, IDictionary{string, string})"/>
		/// <seealso cref="GetOtpUrl(string, string, int, byte[], TimeSpan, IDictionary{string, string})"/>
		public static IOnetimePasswordGenerator FromOtpUrl(Uri url, out string label, out byte[] secret, out IDictionary<string, string> metadata)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			label = null;
			secret = null;
			metadata = null;
			return null;
		}

		/// <summary>
		/// Creates a new <seealso cref="System.Uri"/> that can be used to configure an authentication app. These codes are normally used as the content of a QR code.
		/// </summary>
		/// <param name="label">The name of the account or resource protected by this secret.</param>
		/// <param name="algorithmName">The name of the algorithm used with the secret to generate passwords.</param>
		/// <param name="passwordLength">The length of the password, default is 6, usual values are 6 or 8.</param>
		/// <param name="secret">The secret used to protect this account.</param>
		/// <param name="counter">The initial counter to use when generating passwords for this account.</param>
		/// <param name="metadata">A dictionary containing additional key value pairs to be encoded into the url.</param>
		/// <returns>A new <seealso cref="System.Uri"/> that can be used to configure an authentication app to generate compatible passwords.</returns>
		/// <seealso cref="FromOtpUrl(Uri, out string, out byte[], out IDictionary{string, string})"/>
		public static Uri GetOtpUrl(string label, string algorithmName, int passwordLength, byte[] secret, Int64 counter, IDictionary<string, string> metadata)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		/// <summary>
		/// Creates a new <seealso cref="System.Uri"/> that can be used to configure an authentication app. These codes are normally used as the content of a QR code.
		/// </summary>
		/// <param name="label">The name of the account or resource protected by this secret.</param>
		/// <param name="algorithmName">The name of the algorithm used with the secret to generate passwords.</param>
		/// <param name="passwordLength">The length of the password, default is 6, usual values are 6 or 8.</param>
		/// <param name="secret">The secret used to protect this account.</param>
		/// <param name="timeInterval">The time interval each generated password if valid for. Default value is 30.</param>
		/// <param name="metadata">A dictionary containing additional key value pairs to be encoded into the url.</param>
		/// <returns></returns>
		/// <seealso cref="FromOtpUrl(Uri, out string, out byte[], out IDictionary{string, string})"/>
		public static Uri GetOtpUrl(string label, string algorithmName, int passwordLength, byte[] secret, TimeSpan timeInterval, IDictionary<string, string> metadata)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		#endregion

		#region Instance Implementation

		private OnetimePasswordGeneratorFactory()
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		/// <summary>
		/// Returns a time based password generator.
		/// </summary>
		/// <param name="secret">The secret to use for the new password generator.</param>
		/// <returns>A pre-configured <seealso cref="TimeBasedPasswordGenerator"/> instance.</returns>
		public IOnetimePasswordGenerator CreateNewPasswordGenerator(byte[] secret)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		/// <summary>
		/// Returns a counter based password generator.
		/// </summary>
		/// <param name="secret">The secret to use for the new password generator.</param>
		/// <param name="counter">The current counter to use for the new password  generator.</param>
		/// <returns>A pre-configured <seealso cref="CounterBasedPasswordGenerator"/> instance.</returns>
		public IOnetimePasswordGenerator CreateNewPasswordGenerator(byte[] secret, Int64 counter)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
			return null;
		}

		#endregion

	}
}