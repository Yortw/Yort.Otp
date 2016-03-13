using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Otp
{
	/// <summary>
	/// A onetime password generator based on a counter that increments on each successful login.
	/// </summary>
	/// <remarks>
	/// <para>Default values are SHA1, a 30 second time interval and a password length of 6. The default <see cref="Counter"/> value is zero.</para>
	/// </remarks>
	public sealed class CounterBasedPasswordGenerator : OnetimePasswordGeneratorBase
	{
		// Spec at;
		// https://tools.ietf.org/html/rfc6238#page-3

		// https://tools.ietf.org/html/rfc4226

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>Initialises the instance so it will erase assigned secrets on dispose or change of secret, and with no current secret allocated.</para>
		/// </remarks>
		public CounterBasedPasswordGenerator() : this(true)
		{
		}

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <remarks>
		/// <para>Use this if you are keeping secrets in memory and using them outside of class instances from this library, in which case <paramref name="eraseSecrets"/> should be false to prevent the data from being overwritten unexpectedly.</para>
		/// </remarks>
		/// <param name="eraseSecrets">If true, secrets asssigned are overwritten with zero value bytes when this instance is disposed, or if a new secret is assigned. This *helps* *reduce* the chance of secrets ending up in swap files or being scraped from RAM.</param>
		public CounterBasedPasswordGenerator(bool eraseSecrets) : this(eraseSecrets, null)
		{
		}

		/// <summary>
		/// Full constructor, initialises this instance with an already known secret.
		/// </summary>
		/// <remarks>
		/// <para>If you are keeping secrets in memory and using them outside of class instances from this library, <paramref name="eraseSecrets"/> should be false to prevent the data from being overwritten unexpectedly.</para>
		/// </remarks>
		/// <param name="eraseSecrets">If true, secrets asssigned are overwritten with zero value bytes when this instance is disposed, or if a new secret is assigned. This *helps* *reduce* the chance of secrets ending up in swap files or being scraped from RAM.</param>
		/// <param name="secret">A byte array containign the secret to use for generating passwords.</param>
		public CounterBasedPasswordGenerator(bool eraseSecrets, byte[] secret) : base(eraseSecrets, secret)
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Sets or returns the current value of the 'counter' which is turned into a onetime use password. 
		/// </summary>
		/// <remarks>
		/// <para>Each time a successful login occurs, this counter should be incremented. See the https://tools.ietf.org/html/rfc4226 specification on how to manage the counter.</para>
		/// </remarks>
		public Int64 Counter
		{
			get { return this.MovingFactor; }
			set
			{
				this.MovingFactor = value;
			}
		}

		#endregion

	}
}