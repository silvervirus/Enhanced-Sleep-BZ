using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Subnautica_Enhanced_Sleep
{
	[Menu("Enhanced Sleep")]
	public  class Config : ConfigFile
	{

		[Slider("100% Tiredness Damage", 1f, 100f, DefaultValue = 2F, Format = "{0:R0}")]
		public float FullSleepdamage = 2f;
		[Slider("Coffee Remove Tiredness Amount", 1f, 100f, DefaultValue = 5F, Format = "{0:R0}")]
		public float CoffeeTiredness = 2f;
		[Slider("Water Remove Tiredness Amount", 1f, 100f, DefaultValue = 3F, Format = "{0:R0}")]
		public float Watertiredness = 2f;
		public float Waterbigtiredness = 10f;
		[Slider("Time Before Gaining Tiredness", 1f, 200f, DefaultValue = 12F, Format = "{0:R0}")]
		public float timetosleep = 12f;
		[Slider("Mintues in a Day", 1f, 200f, DefaultValue = 60F, Format = "{0:R0}")]
		public float timePassedMinutes = 60f;
		[Slider("Hours in a Day", 1f, 200f, DefaultValue = 24F, Format = "{0:R0}")]
		public float timePassedHours = 24f;
		[Slider("RecoverFactor", 1f, 200f, DefaultValue = 6F, Format = "{0:R0}")]
		public float recoverFactor = 6f;
		[Slider("loseFactor", 1f, 200f, DefaultValue = 1F, Format = "{0:R0}")]
		public float looseFactor = 1f;
		[Slider("SleeprecoverFactor", 1f, 200f, DefaultValue = 7F, Format = "{0:R0}")]
		public float sleeprecoverFactor = 1f;
		[Slider("SleeploseFactor", 0.1f, 200f, DefaultValue = 0.5F, Format = "{0:R0}")]
		public float sleeplooseFactor = 0.5f;
		[Slider("cameraRotationSpeed", 0.1f, 200f, DefaultValue = 0.5F, Format = "{0:R0}")]
		public float cameraRotationSpeed = 0.1f;
		public float minusfromwidth = 110;
		public float minusfromheight = 1190;
	}
}

