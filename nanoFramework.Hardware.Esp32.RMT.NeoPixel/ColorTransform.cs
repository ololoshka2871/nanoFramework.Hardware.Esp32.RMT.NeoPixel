using System;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel
{
	internal static class ColorTransform
	{
		internal static void HSV2RGB(float H, float S, float V, out float R, out float G, out float B)
		{
			if (S == 0)
			{
				R = G = B = V;
				return;
			}
			float tempH = H / 60.0f;
			int i = (int)Math.Floor(tempH);
			float f = tempH - i;
			float p = V * (1 - S);
			float q = V * (1 - S * f);
			float t = V * (1 - S * (1 - f));

			switch (i)
			{
				case 0:
					R = V; G = t; B = p;
					break;
				case 1:
					R = q; G = V; B = p;
					break;
				case 2:
					R = p; G = V; B = t;
					break;
				case 3:
					R = p; G = q; B = V;
					break;
				case 4:
					R = t; G = p; B = V;
					break;
				default:
					R = V; G = p; B = q;
					break;
			}
		}

		internal static void RGB2HSV(float R, float G, float B, out float H, out float S, out float V)
		{
			H = 0;
			S = 0;
			V = 0;

			var min = R < G ? (R < B ? R : B) : (G < B ? G : B);
			var max = R > G ? (R > B ? R : B) : (G > B ? G : B);

			V = max;
			var delta = max - min;

			if (delta <= 0.00001)
			{
				return;
			}

			if (max <= 0)
			{
				// Undefined, achromatic grey
				V = 0;
				return;
			}

			S = delta / max;

			if (R == max)
			{
				H = (G - B) / delta;
			}
			else if (G == max)
			{
				H = 2 + (B - R) / delta;
			}
			else
			{
				H = 4 + (R - G) / delta;
			}

			H *= 60;
			if (H < 0f)
			{
				H += 360.0f;
			}
		}
	}
}