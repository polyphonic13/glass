
using System.Collections;

public enum SunshineRelativeResolutions
{
		Full,
		Half,
		Third,
		Quarter,
		Fifth,
		Sixth,
		Seventh,
		Eighth,
}

public enum SunshineCascadeCounts
{
		NoCascades,
		TwoCascades,
		FourCascades,
}

public enum SunshineUpdateInterval
{
		EveryFrame,
		AfterXFrames,
		AfterXMovement,
		Manual,
}

public enum SunshineDebugViews
{
		None,
		Status,
		Cascades,
		Scatter,
}

public enum SunshineCascadeResolutions
{
		Square128,
		Square256,
		Square512,
		Square1024,
}

public enum SunshineShadowFormats
{
		Linear,
}

public enum SunshineShadowFilters
{
		Hard,
		PCF2x2,
		PCF3x3,
		PCF4x4,
}

public enum SunshineScatterSamplingQualities
{
		Low,
		Medium,
		High,
		VeryHigh,
}

public static class SunshinePostScatterPass
{
		public static int DrawScatter = 0;
		public static int ApplyScatter = 1;
}

public static class SunshinePostDebugPass
{
		public static int DebugCascades = 0;
		public static int DebugAlpha = 1;
		public static int DebugLightmap = 2;
}

public enum SunshineLightResolutions
{
		LowResolution,
		MediumResolution,
		HighResolution,
		VeryHighResolution,
		Custom,
}

public enum SunshineShaderSets
{
		Auto,
		DesktopShaders,
		MobileShaders,
}
