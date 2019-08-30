using nanoFramework.Hardware.Esp32.RMT.Tx;
using System;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel
{
	public class NeopixelChain : IDisposable
	{
		#region Fields

		// 80MHz / 4 => min pulse 0.05us
		protected const byte CLOCK_DEVIDER = 2;
		// one pulse duration in us
		protected const float min_pulse = 1000000.0f / (80000000 / CLOCK_DEVIDER);

		// default datasheet values
		protected readonly PulseCommand onePulse =
			new PulseCommand((ushort)(0.7 / min_pulse), true, (ushort)(0.6 / min_pulse), false);

		protected readonly PulseCommand zeroPulse =
			new PulseCommand((ushort)(0.3 / min_pulse), true, (ushort)(0.8 / min_pulse), false);

		protected readonly PulseCommand RETCommand =
			new PulseCommand((ushort)(25 / min_pulse), false, (ushort)(26 / min_pulse), false);

		protected RGBColor[] pixels;
		protected PulseCommandList commands;
		protected Transmitter Rmt_tx;

		#endregion Fields

		#region Constructors

		public NeopixelChain(int gpio_pin, int size)
		{
			Rmt_tx = Transmitter.Register(gpio_pin);
			configureTransmitter();
			pixels = new RGBColor[size];
			commands = new PulseCommandList(size * 8 * 3 + 1);
			commands[size * 8 * 3] = RETCommand;
			for (uint i = 0; i < size; ++i)
			{
				pixels[i] = new RGBColor();
			}
		}

		#endregion Constructors

		#region Destructors

		~NeopixelChain()
		{
			Dispose(false);
		}

		#endregion Destructors

		#region Properties

		public uint Size { get => (uint)pixels.Length; }

		public float T0H
		{
			get => zeroPulse.Duration1 * min_pulse;
			set => zeroPulse.Duration1 = (ushort)(value / min_pulse);
		}

		public float T0L
		{
			get => zeroPulse.Duration2 * min_pulse;
			set => zeroPulse.Duration2 = (ushort)(value / min_pulse);
		}

		public float T1H
		{
			get => onePulse.Duration1 * min_pulse;
			set => onePulse.Duration1 = (ushort)(value / min_pulse);
		}

		public float T1L
		{
			get => onePulse.Duration2 * min_pulse;
			set => onePulse.Duration2 = (ushort)(value / min_pulse);
		}

		#endregion Properties

		#region Methods

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Update()
		{
			int pixel = 0;
			int color_component = 0;
			for (; pixel < pixels.Length; color_component += 3, ++pixel)
			{
				var px = pixels[pixel];
				SerialiseColor((color_component + 0) * 8, px.bG);
				SerialiseColor((color_component + 1) * 8, px.bR);
				SerialiseColor((color_component + 2) * 8, px.bB);
			}
			Rmt_tx.Send(commands);
		}

		private void SerialiseColor(int index, byte b)
		{
			const byte mask = 1 << 7;
			for (int i = 0; i < 8; ++i)
			{
				commands[index++] = (b & mask) != 0 ? onePulse : zeroPulse;
				b <<= 1;
			}
		}

		protected void configureTransmitter()
		{
			Rmt_tx.CarierEnabled = false;
			Rmt_tx.ClockDivider = CLOCK_DEVIDER;
			Rmt_tx.isSource80MHz = true;
			Rmt_tx.IsTransmitIdleEnabled = true;
			Rmt_tx.TransmitIdleLevel = false;
		}

		protected virtual void Dispose(bool disposing) => Rmt_tx.Dispose();

		#endregion Methods

		#region Indexers

		public RGBColor this[int i]
		{
			get => pixels[i];
			set
			{
				pixels[i] = value;
			}
		}

		#endregion Indexers
	}
}