namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel
{
	public class Color
	{
		#region Fields

		const int HUE_DEGREE = 512;

		public byte B;
		public byte G;
		public byte R;
		public byte W = 0;

		#endregion Fields

		#region Methods

		public static Color FromHSV(int H, int S, int V)
		{
			HSV2RGB(H, S, V, out byte R, out byte G, out byte B);
			return new Color() { R = R, G = G, B = B };
		}

		public Color SetHSV(int H, int S, int V)
		{
			HSV2RGB(H, S, V, out byte r, out byte g, out byte b);
			R = r;
			G = g;
			B = b;
			W = 0;

			return this;
		}

		private static void HSV2RGB(int H, int S, int V, out byte R, out byte G, out byte B)
		{
			R = 0;
			G = 0;
			B = 0;

			if (S != 0)
			{
				int i = H / (60 * HUE_DEGREE);
				int p = (256 * V - S * V) / 256;

				if ((i & 1) != 0)
				{
					int q = (256 * 60 * HUE_DEGREE * V - H * S * V + 60 * HUE_DEGREE * S * V * i) / (256 * 60 * HUE_DEGREE);
					switch (i)
					{
						case 1:
							R = (byte)q;
							G = (byte)V;
							B = (byte)p;
							break;
						case 3:
							R = (byte)p;
							G = (byte)q;
							B = (byte)V;
							break;
						case 5:
							R = (byte)V;
							G = (byte)p;
							B = (byte)q;
							break;
					}
				}
				else
				{
					int t = (256 * 60 * HUE_DEGREE * V + H * S * V - 60 * HUE_DEGREE * S * V * (i + 1)) / (256 * 60 * HUE_DEGREE);
					switch (i)
					{
						case 0:
							R = (byte)V;
							G = (byte)t;
							B = (byte)p;
							break;
						case 2:
							R = (byte)p;
							G = (byte)V;
							B = (byte)t;
							break;
						case 4:
							R = (byte)t;
							G = (byte)p;
							B = (byte)V;
							break;
					}
				}
			}
		}

		#endregion Methods
	}
}