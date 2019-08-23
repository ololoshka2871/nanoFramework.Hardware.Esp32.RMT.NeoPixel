using System.Collections;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel.Effects
{
	public class SolidColorEffect : IColorEffect
	{
		#region Fields
		public const string ColorString = "Color";

		public Color Color = new Color();

		#endregion Fields

		#region Properties

		// No period in this effect
		public uint Period { get => 0; set { } }

		public IDictionary Settings
		{
			get => new Hashtable(1)
				{
					{ ColorString, Color }
				};
			set
			{
				if (value[ColorString] is Color c)
				{
					Color = c;
				}
			}
		}

		#endregion Properties

		#region Methods

		public void ProcessFrame(uint frame_number, Color[] pixels)
		{
			foreach (var p in pixels)
			{
				p.Apply(Color);
			}
		}

		#endregion Methods
	}
}