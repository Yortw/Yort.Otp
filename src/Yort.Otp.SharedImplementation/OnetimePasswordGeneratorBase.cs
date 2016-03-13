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
		#region Fields

		private IHashAlgorithm _HashAlgorithm;
		private int _PasswordLength;
		private byte[] _Secret;
		private GCHandle _SecretGCHandle;
		private object _SecretSynchroniser;
		private bool _EraseSecrets;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>Initialises the instance so it will erase assigned secrets on dispose or change of secret, and with no current secret allocated.</para>
		/// </remarks>
		protected OnetimePasswordGeneratorBase() : this(true)
		{
		}

		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <remarks>
		/// <para>Use this if you are keeping secrets in memory and using them outside of class instances from this library, in which case <paramref name="eraseSecrets"/> should be false to prevent the data from being overwritten unexpectedly.</para>
		/// </remarks>
		/// <param name="eraseSecrets">If true, secrets asssigned are overwritten with zero value bytes when this instance is disposed, or if a new secret is assigned. This *helps* *reduce* the chance of secrets ending up in swap files or being scraped from RAM.</param>
		protected OnetimePasswordGeneratorBase(bool eraseSecrets) : this(eraseSecrets, null)
		{
		}

#pragma warning disable 1574

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
			_SecretSynchroniser = new object();
			_EraseSecrets = eraseSecrets;
			_HashAlgorithm = new Sha1HashAlgorithm();
			_PasswordLength = 6;

			if (secret != null && secret.Length > 0)
				SetSecret(secret);
		}

#pragma warning restore 1574

		#endregion

		#region Public Properties

#pragma warning disable 1574

		/// <summary>
		/// Sets or rturns an <see cref="IHashAlgorithm"/> implementation used to generate onetime passwords.
		/// </summary>
		/// <seealso cref="Sha1HashAlgorithm"/>
		/// <seealso cref="Sha256HashAlgorithm"/>
		/// <seealso cref="Sha512HashAlgorithm"/>
		/// <seealso cref="MD5HashAlgorithm"/>
		public IHashAlgorithm HashAlgorithm
		{
			get { return _HashAlgorithm; }
			set
			{
				if (_HashAlgorithm == null) throw new ArgumentNullException("value", "value cannot be null.");
				_HashAlgorithm = value;
			}
		}

#pragma warning restore 1574

		/// <summary>
		/// Returns a boolean indicating whether or not this instance has been disposed.
		/// </summary>
		/// <seealso cref="Dispose()"/>
		public bool IsDisposed { get; private set; }

		/// <summary>
		/// Sets or returns length of the password to generate.
		/// </summary>
		/// <remarks>
		/// <para>Allowed values are 1 through 8, though 6 is the default and 8 is the other commonly used values. Other values are discouraged and may not be supported by other standardised systems.</para>
		/// </remarks>
		public int PasswordLength
		{
			get { return _PasswordLength; }
			set
			{
				if (value < 0 || value > 8) throw new ArgumentOutOfRangeException("value", "value must be between 0 and 8, inclusive.");

				_PasswordLength = value;
			}
		}

		/// <summary>
		/// Sets or returns a 64 bit integer which is the value that changes over time (either literally, or on some even such as a successful login) which is converted into a onetime password.
		/// </summary>
		protected virtual Int64 MovingFactor
		{
			get;
			set;
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
				if (_Secret == null || _Secret.Length == 0) throw new InvalidOperationException("Secret has not been provided, or is zero length.");

				var data = BitConverter.GetBytes(MovingFactor);
				if (BitConverter.IsLittleEndian)
					data = data.Reverse().ToArray();

				var hash = _HashAlgorithm.ComputeHash(_Secret, data);
				var Offset = hash.Last() & 0x0F;
				return
				(
					(
						((hash[Offset + 0] & 0x7f) << 24) |
						((hash[Offset + 1] & 0xff) << 16) |
						((hash[Offset + 2] & 0xff) << 8) |
						(hash[Offset + 3] & 0xff)
					) % Globals.DigitPowers[PasswordLength]
				).ToString().PadLeft(this.PasswordLength, '0');
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
			IsDisposed = true;
			if (_EraseSecrets)
			{
				lock (_SecretSynchroniser)
				{
					EraseAndRelaseSecret();
				}
			}
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
			if (secret == null) throw new ArgumentNullException(nameof(secret), nameof(secret) + " cannot be null.");
			if (secret.Length == 0) throw new ArgumentNullException(nameof(secret), nameof(secret) + " cannot be zero length.");
			if (IsDisposed) throw new ObjectDisposedException(nameof(TimeBasedPasswordGenerator));

			var tempHandle = GCHandle.Alloc(secret);
			try
			{
				lock (_SecretSynchroniser)
				{
					if (_EraseSecrets)
						EraseAndRelaseSecret();

					_Secret = secret;
					_SecretGCHandle = tempHandle;
				}
			}
			catch
			{
				if (tempHandle.IsAllocated)
					tempHandle.Free();

				throw;
			}
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
			try
			{
				Dispose(true);
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}

		#endregion

		#region Private Methods

		private void EraseAndRelaseSecret()
		{
			try
			{
				var secret = _Secret;
				if (secret != null)
				{
					for (int cnt = 0; cnt < secret.Length; cnt++)
					{
						secret[cnt] = 0;
					}
				}
			}
			finally
			{
				if (_SecretGCHandle.IsAllocated)
					_SecretGCHandle.Free();
			}
		}

		#endregion

	}
}