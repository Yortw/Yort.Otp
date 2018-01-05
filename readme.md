# Yort.Otp
A portable .Net class library for creating onetime passwords (based on [[rfc4226](https://tools.ietf.org/html/rfc4226)] and [[rfc6238](https://tools.ietf.org/html/rfc6238)]).
These are the sorts of authentication codes used by Google/Microsoft/Facebook/Amazon etc. for second factor authentication.

[![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/Yortw/Yort.Otp/blob/master/LICENSE.md) 

## Supported Platforms
Currently;

* .Net Framework 4.0
* Windows Phone Silverlight (8.1+) 
* Xamarin.iOS
* Xamarin.Android 
* WinRT (Windows Store Apps 8.1)
* UWP 10+ (Windows 10 Universal Programs)

## Build Status
[![Build status](https://ci.appveyor.com/api/projects/status/e8116lsaf7oeb74d?svg=true)](https://ci.appveyor.com/project/Yortw/yort-otp)

## Available on Nuget

```powershell
    PM> Install-Package Yort.Otp
```

[![NuGet Badge](https://buildstats.info/nuget/Yort.Otp)](https://www.nuget.org/packages/Yort.Otp/)

## Samples
If you're implementing this on a server you should really read and understand the RFC specifications (linked above). There are sever specific issues around managing out of sync counters or client/server times that need to be understood.

Here are some quick start samples;

```c#
public void Main(string[] args)
{
	// Generate a "time based" password from the current time (retrieved via DateTime.UtcNow), 
	// using the ASCII secret in the sample section of https://tools.ietf.org/html/rfc6238.
	// This uses a default interval of 30 seconds (maximum length of time the password is value for).
	using (var passwordGenerator = new TimeBasedPasswordGenerator(false, OnetimePasswordSecret.FromAscii("12345678901234567890")))
	{
		System.Diagnostics.Debug.WriteLine($"Password: {passwordGenerator.GeneratedPassword} valid until {((TimeBasedPasswordGenerator)passwordGenerator).ValidUntilUtc.ToLocalTime()}");
	}
}
```


```c#
public void Main(string[] args)
{
	// Generate a "counter based" one time password from a counter value of 10, 
	// using the ASCII secret in the sample section of https://tools.ietf.org/html/rfc6238
    // The counter should be incremented on each *successful* login, and stored between sessions. See the RFC spec for details.
	using (var passwordGenerator = new CounterBasedPasswordGenerator(false, OnetimePasswordSecret.FromAscii("12345678901234567890")))
	{
		passwordGenerator.Counter = 10;
		System.Diagnostics.Debug.WriteLine("Password: " + passwordGenerator.GeneratedPassword);
	}
}
```

If you're going to generate passwords for many different secrets but using an otherwise consistent configuration, factories can make that easier/more concise;

```c#
// Counter based factory sample
//During startup, obtain and keep a reference to a factory
_OtpFactory = OnetimePasswordGeneratorFactory.CreateFactory(true, new Sha512HashAlgorithm(), 8);

//When you want to generate a password...
using (var passwordGenerator = _OtpFactory.CreateNewPasswordGenerator(OnetimePasswordSecret.FromAscii("12345678901234567890"), 10))
{
	System.Diagnostics.Debug.WriteLine($"Password: {passwordGenerator.GeneratedPassword}");
}

// Time based factory sample
//During startup, obtain and keep a reference to a factory
_OtpFactory = OnetimePasswordGeneratorFactory.CreateFactory(true, new Sha512HashAlgorithm(), 8, TimeSpan.FromMinutes(1));

//When you want to generate a password...
using (var passwordGenerator = _OtpFactory.CreateNewPasswordGenerator(OnetimePasswordSecret.FromAscii("12345678901234567890")))
{
	System.Diagnostics.Debug.WriteLine($"Password: {passwordGenerator.GeneratedPassword} valid until {passwordGenerator.ValidUntilUtc.ToLocalTime()}");
}
```

This library *doesn't* generate QR codes, but it can generate the OTP uri to be turned into a QR code for configuring authenticator apps;

```c#
var configureAuthenticatorUri =	OnetimePasswordGeneratorFactory.GetOtpUrl("account name or identifier", "SHA1", 8, OnetimePasswordSecret.FromAscii("12345678901234567890"), TimeSpan.FromMinutes(1));
```

Likewise, if given an OTP uri it can configure a password generator appropriately. This also returns the secret and account name, so they can be stored if required.

```c#
string label = null;
byte[] secret = null;
var passwordGenerator = OnetimePasswordGeneratorFactory.FromOtpUrl(configureAuthenticatorUri, out label, out secret);
```

