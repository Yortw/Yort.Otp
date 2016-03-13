using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace Yort.Otp
{
	/// <summary>
	/// Base cass used for onetime password generators, provides re-use as well as a "versionable" common interface.
	/// </summary>
	public abstract class OnetimePasswordGeneratorBase : IOnetimePasswordGenerator, IDisposable
	{

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>Initialises the instance so it will erase assigned secrets on dispose or change of secret, and with no current secret allocated.</para>
		/// </remarks>
		protected OnetimePasswordGeneratorBase() : this(true)
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
		protected OnetimePasswordGeneratorBase(bool eraseSecrets) : this(true, null)
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
		protected OnetimePasswordGeneratorBase(bool eraseSecrets, byte[] secret)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Sets or rturns an <see cref="IHashAlgorithm"/> implementation used to generate onetime passwords.
		/// </summary>
		/// <seealso cref="Sha1HashAlgorithm"/>
		/// <seealso cref="Sha256HashAlgorithm"/>
		/// <seealso cref="Sha512HashAlgorithm"/>
		/// <seealso cref="MD5HashAlgorithm"/>
		public IHashAlgorithm HashAlgorithm
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
		/// Returns a boolean indicating whether or not this instance has been disposed.
		/// </summary>
		/// <seealso cref="Dispose()"/>
		public bool IsDisposed
		{
			get
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
				return false;
			}
		}

		/// <summary>
		/// Sets or returns length of the password to generate.
		/// </summary>
		/// <remarks>
		/// <para>Allowed values are 1 through 8, though 6 is the default and 8 is the other commonly used values. Other values are discouraged and may not be supported by other standardised systems.</para>
		/// </remarks>
		public int PasswordLength
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

		/// <summary>
		/// Sets or returns a 64 bit integer which is the value that changes over time (either literally, or on some even such as a successful login) which is converted into a onetime password.
		/// </summary>
		protected virtual Int64 MovingFactor
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

		/// <summary>
		/// Returns the password generated for the current <seealso cref="MovingFactor"/> using the provided secret and <seealso cref="HashAlgorithm"/>.
		/// </summary>
		/// <see cref="SetSecret(byte[])"/>
		/// <see cref="HashAlgorithm"/>
		/// <see cref="PasswordLength"/>
		/// <see cref="MovingFactor"/>
		public string GeneratedPassword
		{
			get
			{
				ExceptionHelper.ThrowYoureDoingItWrong();
				return null;
			}
		}

		/// <summary>
		/// Used by derived classes to clean up resources held by the base class.
		/// </summary>
		/// <remarks>
		/// <para>If the instance was constructed with the eraseSecrets argument set to true, the currently assigned secret will be overwritten in memory with zeros.</para>
		/// </remarks>
		/// <param name="isDisposing">True if dispose is being explicitly called from code, false if it is being called from a destructor.</param>
		protected virtual void Dispose(bool isDisposing)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the secret used to generate passwords.
		/// </summary>
		/// <remarks>
		/// <para>If the instance was constructed with the eraseSecrets argument set to true, any existing secret will be overwritten in memory with zeros.</para>
		/// </remarks>
		/// <param name="secret">A byte array containing the secret value used with the <see cref="HashAlgorithm"/> to generate onetime passwords.</param>
		public void SetSecret(byte[] secret)
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		/// Disposes this class and any internal resources.
		/// </summary>
		/// <remarks>
		/// <para>If the instance was constructed with the eraseSecrets argument set to true, any existing secret will be overwritten in memory with zeros.</para>
		/// </remarks>
		public void Dispose()
		{
			ExceptionHelper.ThrowYoureDoingItWrong();
		}

		#endregion

	}
}