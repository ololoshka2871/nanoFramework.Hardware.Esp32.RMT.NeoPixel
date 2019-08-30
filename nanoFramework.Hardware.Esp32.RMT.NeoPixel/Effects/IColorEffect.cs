using System.Collections;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel.Effects
{
	/// <summary>
	/// All color effets mast inhariet this interface
	/// </summary>
	public interface IColorEffect
	{
		#region Properties

		/// <summary>
		/// Efect period in frames
		/// </summary>
		uint Period { get; set; }

		/// <summary>
		/// Effect settings
		/// "Name" : "value"
		/// </summary>
		IDictionary Settings { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Change pixels by some algorithm based on frame_number
		/// </summary>
		/// <param name="frame_number">frame couter value, Effect argument</param>
		/// <param name="pixels">pixels to accept effect to</param>
		void ProcessFrame(uint frame_number, RGBColor[] pixels);

		#endregion Methods
	}
}