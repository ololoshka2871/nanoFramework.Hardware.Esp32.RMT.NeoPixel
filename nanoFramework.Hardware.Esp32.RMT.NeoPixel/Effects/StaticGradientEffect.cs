using System.Collections;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel.Effects
{
	public class StaticGradientEffect : IColorEffect
	{
		#region Fields

		public const string Color1String = "Color1";
		public const string Color2String = "Color2";

		public RGBColor Color1 = new RGBColor();
		public RGBColor Color2 = new RGBColor();

		#endregion Fields

		#region Properties

		// No period in this effect
		public uint Period { get => 0; set { } }

		public IDictionary Settings
		{
			get => new Hashtable(1)
				{
					{ Color1String, Color1 },
					{ Color2String, Color2 }
				};
			set
			{
				if (value[Color1String] is RGBColor c1)
				{
					Color1 = c1;
				}
				if (value[Color2String] is RGBColor c2)
				{
					Color2 = c2;
				}
			}
		}

		#endregion Properties

		#region Methods

		public void ProcessFrame(uint frame_number, RGBColor[] pixels)
		{
			// TODO
		}

		#endregion Methods
	}
}