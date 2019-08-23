using System.Collections;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel.Effects
{
	public class StaticGradientEffect : IColorEffect
	{
		#region Fields

		public const string Color1String = "Color1";
		public const string Color2String = "Color2";

		public Color Color1 = new Color();
		public Color Color2 = new Color();

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
				if (value[Color1String] is Color c1)
				{
					Color1 = c1;
				}
				if (value[Color2String] is Color c2)
				{
					Color2 = c2;
				}
			}
		}

		#endregion Properties

		#region Methods

		public void ProcessFrame(uint frame_number, Color[] pixels)
		{
			// TODO
		}

		#endregion Methods
	}
}