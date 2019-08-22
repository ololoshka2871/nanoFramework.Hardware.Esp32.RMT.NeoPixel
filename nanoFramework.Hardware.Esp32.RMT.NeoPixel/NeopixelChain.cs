using nanoFramework.Hardware.Esp32.RMT.Tx;
using System;

namespace nanoFramework.Hardware.Esp32.RMT.NeoPixel
{
	public class NeopixelChain : IDisposable
	{
		#region Fields

		// 80MHz / 4 => min pulse 0.00us
		protected const byte CLOCK_DEVIDER = 2;
		// one pulse duration in us
		protected const float min_pulse = 1000000.0f / (80000000 / CLOCK_DEVIDER);

		// default datasheet values
		protected readonly PulseCommand onePulse =
			new PulseCommand((ushort)(0.7 / min_pulse), true, (ushort)(0.6 / min_pulse), false);

		protected readonly PulseCommand zeroPulse =
			new PulseCommand((ushort)(0.35 / min_pulse), true, (ushort)(0.8 / min_pulse), false);

		protected readonly PulseCommand RETCommand =
			new PulseCommand((ushort)(25 / min_pulse), false, (ushort)(26 / min_pulse), false);

		protected Color[] pixels;
		protected Transmitter Rmt_tx;

		#endregion Fields

		#region Constructors

		public NeopixelChain(int gpio_pin, uint size, bool is4BytesPrePixel = false)
		{
			Rmt_tx = Transmitter.Register(gpio_pin);
			configureTransmitter();
			Is4BytesPrePixel = is4BytesPrePixel;
			pixels = new Color[size];
			for (uint i = 0; i < size; ++i)
			{
				pixels[i] = new Color();
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

		public bool Is4BytesPrePixel { get; set; }
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
			var commandlist = new PulseCommandList();
			for(uint pixel = 0; pixel < pixels.Length; ++pixel)
			{
				SerialiseColor(pixels[pixel].G, commandlist);
				SerialiseColor(pixels[pixel].R, commandlist);
				SerialiseColor(pixels[pixel].B, commandlist);
				if (Is4BytesPrePixel)
					SerialiseColor(pixels[pixel].W, commandlist);
			}
			commandlist.AddCommand(RETCommand); // RET
			Rmt_tx.Send(commandlist);
		}

		private void SerialiseColor(byte b, PulseCommandList commandlist)
		{
			for (int i = 0; i < 8; ++i)
			{
				commandlist.AddCommand(((b & (1u << 7)) != 0) ? onePulse : zeroPulse);
				b <<= 1;
			}
		}

		protected void configureTransmitter()
		{
			Rmt_tx.CarierEnabled = false;
			Rmt_tx.ClockDivider = CLOCK_DEVIDER;
			Rmt_tx.isSource80MHz = true;
			Rmt_tx.TransmitIdleLevel = true;
			Rmt_tx.IsTransmitIdleEnabled = true;
		}

		protected virtual void Dispose(bool disposing)
		{
			Rmt_tx.Dispose();
		}

		#endregion Methods

		#region Indexers

		public Color this[uint i]
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