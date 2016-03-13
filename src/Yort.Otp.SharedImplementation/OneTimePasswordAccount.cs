using System;
using System.Collections.Generic;
using System.Text;

namespace Yort.Otp
{
	/// <summary>
	/// Represents details of an account protected by OTP, and an associated OTP generator.
	/// </summary>
	public class OnetimePasswordAccount
	{
		/// <summary>
		/// The name of the account.
		/// </summary>
		public string Label { get; set; }
		/// <summary>
		/// Optional. The name of the organisation or company the accuont belongs to.
		/// </summary>
		public string Issuer { get; set; }
		/// <summary>
		/// The raw bytes for the account secret used to generate OTP codes.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public byte[] Secret { get; set; }
		/// <summary>
		/// A <see cref="IOnetimePasswordGenerator"/> instance used to generate OTP codes for this account.
		/// </summary>
		public IOnetimePasswordGenerator PasswordGenerator { get; set; }
		/// <summary>
		/// A dictionary of additional values associated with the account.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public Dictionary<string, string> Metadata { get; set; }
	}
}