using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yort.Otp
{
	internal static class ExceptionHelper
	{
		internal static void ThrowYoureDoingItWrong()
		{
			throw new InvalidOperationException("This assembly should never be loaded at runtime, and should only be referenced from other portable class libraries. Please ensure your application references a platform specific assembly.");
		}
	}
}