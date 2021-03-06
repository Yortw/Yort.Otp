﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Otp
{
	/// <summary>
	/// Used to generate passwords based on a specific time, using the <see cref="!:https://tools.ietf.org/html/rfc6238">rfc6238 specification</see> for TOTP.
	/// </summary>
	/// <remarks>
	/// <para>Default values are SHA1, a 30 second time interval and a password length of 6. The current time from the device's clock is used (dynamically) to create the password, so the <see cref="OnetimePasswordGeneratorBase.GeneratedPassword"/> property will change over time.</para>
	/// <para>
	/// <code>
	/// //Configures a password generator using a base 32 secret (as typically provided by Google).
	/// var passwordGenerator = new TimeBasedPassword();
	/// passwordGenerator.SetSecret(TimeBasedPassword.SecretFromBase32String("JBSWY3DPEHPK3PXP"));
	/// var password = passwordGenerator.GeneratedPassword;
	/// </code>
	/// </para>
	/// </remarks>
	public sealed class TimeBasedPasswordGenerator : OnetimePasswordGeneratorBase
	{

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>Initialises the instance so it will erase assigned secrets on dispose or change of secret, and with no current secret allocated.</para>
		/// </remarks>
		public TimeBasedPasswordGenerator() : this(true)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <remarks>
		/// <para>Use this if you are keeping secrets in memory and using them outside of class instances from this library, in which case <paramref name="eraseSecrets"/> should be false to prevent the data from being overwritten unexpectedly.</para>
		/// </remarks>
		/// <param name="eraseSecrets">If true, secrets asssigned are overwritten with zero value bytes when this instance is disposed, or if a new secret is assigned. This *helps* *reduce* the chance of secrets ending up in swap files or being scraped from RAM.</param>
		public TimeBasedPasswordGenerator(bool eraseSecrets) : this(eraseSecrets, null)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		/// <summary>
		/// Full constructor, initialises this instance with an already known secret.
		/// </summary>
		/// <remarks>
		/// <para>If you are keeping secrets in memory and using them outside of class instances from this library, <paramref name="eraseSecrets"/> should be false to prevent the data from being overwritten unexpectedly.</para>
		/// </remarks>
		/// <param name="eraseSecrets">If true, secrets asssigned are overwritten with zero value bytes when this instance is disposed, or if a new secret is assigned. This *helps* *reduce* the chance of secrets ending up in swap files or being scraped from RAM.</param>
		/// <param name="secret">A byte array containign the secret to use for generating passwords.</param>
		public TimeBasedPasswordGenerator(bool eraseSecrets, byte[] secret) : base(eraseSecrets, secret)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Sets or returns the amount of time each generated password is valid for. The default is 30 seconds.
		/// </summary>
		public TimeSpan TimeInterval
		{
			get
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
				return TimeSpan.Zero;
			}
			set
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
			}
		}

		/// <summary>
		/// Sets or returns the date and time to calculate a password for. If null, the current date and time from the device/OS clock is used.
		/// </summary>
		public DateTime? Timestamp
		{
			get
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
				return null;
			}
			set
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
			}
		}

		/// <summary>
		/// Sets or returns the last second at which the value of <see cref="OnetimePasswordGeneratorBase.GeneratedPassword"/> is valid until.
		/// </summary>
		public DateTime ValidUntilUtc
		{
			get
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
				return DateTime.MinValue;
			}
		}

		/// <summary>
		/// Uses the <see cref="Timestamp"/> or current clock time and <see cref="TimeInterval"/> to generate the counter/moving factor for password generation.
		/// </summary>
		protected override long MovingFactor
		{
			get
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
				return 0;
			}

			set
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
			}
		}

		#endregion

	}
}