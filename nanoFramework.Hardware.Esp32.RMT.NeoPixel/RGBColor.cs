using System;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel
{
	public class RGBColor
	{
		#region Fields

		private float mB = 0f;
		private float mG = 0f;
		private float mR = 0f;

		private byte mbB = 0;
		private byte mbG = 0;
		private byte mbR = 0;

		private static byte[] GammaTable;
		private static float mGamma = 2.2f;

		#endregion Fields

		#region Properties

		public static float Gamma
		{
			get => mGamma;
			set
			{
				mGamma = value;
				UpdateGammaTable();
			}
		}

		public float B
		{
			get => mB;
			set
			{
				mB = value;
				mbB = GammaCorrectComponent(Colorf2byte(mB));
			}
		}

		public float G
		{
			get => mG;
			set
			{
				mG = value;
				mbG = GammaCorrectComponent(Colorf2byte(mG));
			}
		}

		public float R
		{
			get => mR;
			set
			{
				mR = value;
				mbR = GammaCorrectComponent(Colorf2byte(mR));
			}
		}

		public byte bB
		{
			get => mbB;
			set
			{
				mbB = value;
				B = ColorByte2f(mbB);
			}
		}

		public byte bG
		{
			get => mbG;
			set
			{
				mbG = value;
				G = ColorByte2f(mbG);
			}
		}

		public byte bR
		{
			get => mbR;
			set
			{
				mbR = value;
				R = ColorByte2f(mbR);
			}
		}

		#endregion Properties

		#region Constructors

		public RGBColor() => UpdateGammaTable();

		#endregion Constructors

		#region Methods

		public static float ColorByte2f(byte component) => component / (float)255;

		public static byte Colorf2byte(float cc) => cc > 1.0f ? (byte)255 : (byte)(cc * 255);

		public static RGBColor FromHSV(float H, float S, float V)
		{
			ColorTransform.HSV2RGB(H, S, V, out float R, out float G, out float B);
			return new RGBColor() { R = R, G = G, B = B };
		}

		public static byte GammaCorrectComponent(byte In) => GammaTable[In];

		public RGBColor Apply(RGBColor newcolor)
		{
			R = newcolor.R;
			G = newcolor.G;
			B = newcolor.B;

			return this;
		}

		public RGBColor SetHSV(float H, float S, float V)
		{
			ColorTransform.HSV2RGB(H, S, V, out float r, out float g, out float b);
			R = r;
			G = g;
			B = b;

			return this;
		}

		public HSVColor ToHSV()
		{
			ColorTransform.RGB2HSV(R, G, B, out float h, out float s, out float v);
			return new HSVColor() { H = h, S = s, V = v };
		}

		private static void UpdateGammaTable()
		{
			if (GammaTable == null)
			{
				GammaTable = new byte[256];
			}
			for (int i = 0; i < GammaTable.Length; ++i)
			{
				GammaTable[i] = (byte)(Math.Pow(i / (float)255, mGamma) * 255 + 0.5);
			}
		}

		#endregion Methods
	}
}