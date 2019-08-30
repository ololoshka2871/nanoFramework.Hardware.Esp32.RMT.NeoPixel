namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel
{
	public class HSVColor
	{
		#region Fields

		public float H = 0f;
		public float S = 0f;
		public float V = 0f;

		#endregion Fields

		#region Methods

		public static HSVColor FromRGB(float R, float G, float B)
		{
			ColorTransform.RGB2HSV(R, G, B, out float h, out float s, out float v);
			return new HSVColor() { H = h, S = s, V = v };
		}

		public RGBColor ToRGB() => RGBColor.FromHSV(H, S, V);

		public HSVColor Apply(HSVColor newcolor)
		{
			H = newcolor.H;
			S = newcolor.S;
			V = newcolor.V;

			return this;
		}

		#endregion Methods
	}
}