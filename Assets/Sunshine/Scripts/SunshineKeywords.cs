using UnityEngine;
using System.Collections;

/// <summary>
/// Manages Shader.EnableKeyword() and Shader.DisableKeyword()...
/// </summary>
public static class SunshineKeywords
{
		private class ChangeTracker
		{
				private int lastValue = -1;

				public bool Change (int newValue)
				{
						if (newValue != lastValue) {
								lastValue = newValue;
								return true;
						}
						return false;
				}

				public bool Change (bool newValue)
				{
						return Change (newValue ? 1 : 0);
				}

				public int Value { get { return lastValue; } }

				public bool ValueBool { get { return lastValue > 0; } }
		}

		private static void SetKeyword (int index, string[] keywords)
		{
				for (int i = 0; i < keywords.Length; i++) {
						if (i == index)
								Shader.EnableKeyword (keywords [i]);
						else
								Shader.DisableKeyword (keywords [i]);
				}
		}

		private static void SetKeywordWithFallbacks (int index, string[] keywords, int minimumFallback)
		{
				for (int i = 0; i < keywords.Length; i++) {
						if (i == index || (i < index && i >= minimumFallback))
								Shader.EnableKeyword (keywords [i]);
						else
								Shader.DisableKeyword (keywords [i]);
				}
		}

		private static void ToggleKeyword (bool toggle, string keywordON, string keywordOFF)
		{
				Shader.DisableKeyword (toggle ? keywordOFF : keywordON);
				Shader.EnableKeyword (toggle ? keywordON : keywordOFF);
		}

		private static void ToggleKeyword (bool toggle, string keyword)
		{
				if (toggle)
						Shader.EnableKeyword (keyword);
				else
						Shader.DisableKeyword (keyword);
		}

		private const string ONE_CASCADE = "SUNSHINE_ONE_CASCADE";
		private const string TWO_CASCADES = "SUNSHINE_TWO_CASCADES";
		private const string THREE_CASCADES = "SUNSHINE_THREE_CASCADES";
		private const string FOUR_CASCADES = "SUNSHINE_FOUR_CASCADES";
		private static readonly string[] X_CASCADES = new string[] {
				ONE_CASCADE,
				TWO_CASCADES,
				THREE_CASCADES,
				FOUR_CASCADES
		};
		private static ChangeTracker cascadeCount = new ChangeTracker ();

		public static void SetCascadeCount (int i)
		{
				if (cascadeCount.Change (Mathf.Clamp (i - 1, 0, 3)))
						SetKeyword (cascadeCount.Value, X_CASCADES);
		}

		private const string OVERCAST_ON = "SUNSHINE_OVERCAST_ON";
		private const string OVERCAST_OFF = "SUNSHINE_OVERCAST_OFF";
		private static ChangeTracker overcast = new ChangeTracker ();

		public static void ToggleOvercast (bool enabled)
		{
				if (overcast.Change (enabled))
						ToggleKeyword (enabled, OVERCAST_ON, OVERCAST_OFF);
		}

		private const string FILTER_DISABLED = "SUNSHINE_DISABLED";
		private const string FILTER_HARD = "SUNSHINE_FILTER_HARD";
		private const string FILTER_PCF_2x2 = "SUNSHINE_FILTER_PCF_2x2";
		private const string FILTER_PCF_3x3 = "SUNSHINE_FILTER_PCF_3x3";
		private const string FILTER_PCF_4x4 = "SUNSHINE_FILTER_PCF_4x4";
		private static readonly string[] FILTER_STYLES = new string[] {
				FILTER_DISABLED,
				FILTER_HARD,
				FILTER_PCF_2x2,
				FILTER_PCF_3x3,
				FILTER_PCF_4x4
		};

		public static void SetFilterStyle (int style)
		{
				SetKeywordWithFallbacks (style, FILTER_STYLES, 1);
		}

		public static void SetFilterStyle (SunshineShadowFilters style)
		{
				SetFilterStyle ((int)style + 1);
		}

		public static void DisableShadows ()
		{
				SetFilterStyle (0);
		}
	
		// To save Shader Keywords, we'll recycle Filter Keywords:
		private const string SCATTER_QUALITY_LOW = FILTER_HARD;
		//"SUNSHINE_SCATTER_QUALITY_LOW";
		private const string SCATTER_QUALITY_MEDIUM = FILTER_PCF_2x2;
		//"SUNSHINE_SCATTER_QUALITY_MEDIUM";
		private const string SCATTER_QUALITY_HIGH = FILTER_PCF_3x3;
		//"SUNSHINE_SCATTER_QUALITY_HIGH";
		private const string SCATTER_QUALITY_VERYHIGH = FILTER_PCF_4x4;
		//"SUNSHINE_SCATTER_QUALITY_VERYHIGH";
		private static readonly string[] SCATTER_QUALITIES = new string[] {
				SCATTER_QUALITY_LOW,
				SCATTER_QUALITY_MEDIUM,
				SCATTER_QUALITY_HIGH,
				SCATTER_QUALITY_VERYHIGH
		};

		public static void SetScatterQuality (SunshineScatterSamplingQualities quality)
		{
				SetKeyword ((int)quality, SCATTER_QUALITIES);
		}

}