using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sunshine's main class, responsible for configuration and setup.
/// </summary>
[ExecuteInEditMode]
public class Sunshine : MonoBehaviour
{
		public const string Version = "1.8.1";

		public static string FormatMessage (string message)
		{
				return string.Format ("Sunshine {0}: {1}", Version, message);
		}

		public static void LogMessage (string message, bool showInEditor = false)
		{
				if (showInEditor || Application.isPlaying)
						Debug.Log (FormatMessage (message));
		}

		public const string OccluderShaderName = "Hidden/Sunshine/Occluder";
		public const string PostScatterShaderName = "Hidden/Sunshine/PostProcess/Scatter";
		public const string PostBlurShaderName = "Hidden/Sunshine/PostProcess/Blur";
		public const string PostDebugShaderName = "Hidden/Sunshine/PostProcess/Debug";
	
		public const int MAX_CASCADES = 4;
	
		/// <summary>
		/// The main instance of Sunshine... There should be only one in the Scene.
		/// </summary>
		public static Sunshine Instance = null;

		/// <summary>
		/// If Sunshine is ready for the SunshineRenderers to do their thing.
		/// </summary>
		[NonSerialized]
		public bool Ready = false;
	
		/// <summary>
		/// If Sunshine is supported
		/// </summary>
		[NonSerialized]
		public bool Supported = true;

		/// <summary>
		/// The light to use as the Sun for Shadows and Scattering.
		/// </summary>
		public Light SunLight;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Sunshine"/> requires postprocessing.
		/// </summary>
		/// <value>
		/// <c>true</c> if requires postprocessing; otherwise, <c>false</c>.
		/// </value>
		public bool RequiresPostprocessing {
				get { return (ScatterActive || DebugView == SunshineDebugViews.Cascades) && PostProcessSupported; }
		}

		/// <summary>
		/// The layers that should occlude light in the scene.
		/// </summary>
		public int Occluders = -1;
	
		/// <summary>
		/// Override Occluders per Cascade
		/// </summary>
		public bool UsePerCascadeOccluders = false;
		/// <summary>
		/// Additional Occluders Mask
		/// </summary>
		public int Occluders1 = -1;
		/// <summary>
		/// Additional Occluders Mask
		/// </summary>
		public int Occluders2 = -1;
		/// <summary>
		/// Additional Occluders Mask
		/// </summary>
		public int Occluders3 = -1;

		public LayerMask GetCascadeOccluders (int cascade)
		{

				switch (cascade) {
				case 0:
						return Occluders;
				case 1:
						return UsePerCascadeOccluders ? Occluders1 : Occluders;
				case 2:
						return UsePerCascadeOccluders ? Occluders2 : Occluders;
				case 3:
						return UsePerCascadeOccluders ? Occluders3 : Occluders;
				}
				return Occluders;
		}

		/// <summary>
		/// Gets the diagonal radius of the filter kernel in texels.
		/// </summary>
		public float ShadowFilterKernelRadiusDiagonal { get { return Mathf.Sqrt (ShadowFilterKernelRadius * 2f); } }

		/// <summary>
		/// Gets the shadow filter kernel radius in texels.
		/// </summary>
		public float ShadowFilterKernelRadius {
				get {
						switch (ShadowFilter) {
						case SunshineShadowFilters.PCF2x2:
								return 1f;
						case SunshineShadowFilters.PCF3x3:
								return 1.5f;
						case SunshineShadowFilters.PCF4x4:
								return 2f;
						}
						return 0.5f;
				}
		}

		/// <summary>
		/// Calculates the physical size of a shadow texel in world space
		/// </summary>
		/// <returns>
		/// The physical size of a texel
		/// </returns>
		/// <param name='cascadeID'>
		/// the cascade index number
		/// </param>
		public float LightmapTexelPhysicalSize (int cascadeID)
		{
				return (SunLightCameras [cascadeID].orthographicSize * 2f) / (float)CascadeMapResolution;
		}

		/// <summary>
		/// Gets the shadow bias, a depth offset used to avoid "Shadow Acne"
		/// </summary>
		public float ShadowBias (int cID)
		{
				return SunLight.shadowBias * (1f + LightmapTexelPhysicalSize (cID) * 2f);
		}

		/// <summary>
		/// Calculates a slope-scale depth bias.
		/// </summary>
		/// <returns>
		/// The shadow slope bias.
		/// </returns>
		/// <param name='cID'>
		/// The cascade index
		/// </param>
		public float ShadowSlopeBias (int cID)
		{
				return SunLight.shadowNormalBias * LightmapTexelPhysicalSize (cID) * 10f;
		}

		/// <summary>
		/// How to handle Desktop/Mobile versions...
		/// </summary>
		public SunshineShaderSets ShaderSet = SunshineShaderSets.Auto;
	
		/// <summary>
		/// When should the Lightmap be updated
		/// </summary>
		public SunshineUpdateInterval UpdateInterval = SunshineUpdateInterval.EveryFrame;
	
		/// <summary>
		/// When UpdateInterval is set to AfterXFrames, the number of frames required to trigger a refresh.
		/// </summary>
		public int UpdateIntervalFrames = 2;

		/// <summary>
		/// When UpdateInterval is set to AfterXFrames, the amount of padding to give to the ShadowMap bounds, to account for camera movement.
		/// </summary>
		public float UpdateIntervalPadding = 0f;
	
		/// <summary>
		/// When UpdateInterval is set to AfterXMovement, the amount of movement required to trigger a refresh.
		/// </summary>
		public float UpdateIntervalMovement = 1f;

		public bool UsingCustomBounds { get { return CustomLightBoundsOrigin != null; } }

		public SunshineMath.BoundingSphere CustomBounds { get { return new SunshineMath.BoundingSphere () {
						origin = CustomLightBoundsOrigin.position,
						radius = CustomLightBoundsRadius
				}; } }

		public Transform CustomLightBoundsOrigin = null;
		public float CustomLightBoundsRadius = 1.0f;

		/// <summary>
		/// True if we're targetting a mobile platform.
		/// </summary>
		public bool IsMobile { //Not a const, to avoid "Unreachable Code" warnings
				get {
						switch (ShaderSet) {
						case SunshineShaderSets.DesktopShaders:
								return false;
						case SunshineShaderSets.MobileShaders:
								return true;
						}
						#if UNITY_ANDROID || UNITY_IPHONE || UNITY_BLACKBERRY || UNITY_WP8
				return true;
						#else
						return false;
						#endif
				}
		}

		/// <summary>
		/// The Lightmap resolution (in Unity terminology).
		/// The final resolution is calculated to closly match Unity's internal calculations.
		/// </summary>
		/// 
		public SunshineLightResolutions LightmapResolution = SunshineLightResolutions.MediumResolution;
	
		/// <summary>
		/// The custom Lightmap resolution.  This is used when LightmapResolution is set to Custom.
		/// </summary>
		public int CustomLightmapResolution = 512;

		public int IdealLightmapResolution {
				get {
						if (LightmapResolution == SunshineLightResolutions.Custom)
								return Mathf.Clamp (CustomLightmapResolution, 1, 4096);
						return Mathf.Max (SunshineMath.UnityStyleLightmapResolution (LightmapResolution), 1);
				}
		}

		/// <summary>
		/// Gets the ideal Lightmap Width.
		/// </summary>
		/// 
		public int IdealLightmapWidth {
				get {
						if (CascadeCount == 2)
								return IdealLightmapResolution / 2;
						return IdealLightmapResolution;
				}
		}

		/// <summary>
		/// Gets the ideal shadow map Height.
		/// </summary>
		/// 
		public int IdealLightmapHeight { get { return IdealLightmapResolution; } }

		/// <summary>
		/// Gets the inverse ratio of a Cascade's resolution to the Lightmap resolution
		/// </summary>
		/// 
		public int CascadeResolutionInverseRatio { get { return (CascadeCount) > 1 ? 2 : 1; } }

		/// <summary>
		/// Gets the cascade map resolution.
		/// </summary>
		public int CascadeMapResolution { get { return IdealLightmapResolution / CascadeResolutionInverseRatio; } }

		/// <summary>
		/// Should Lightmap cameras attempt to correct for LOD mismatching?
		/// Cascade Cameras will match Eye Camera position/properties, then offsetting with matrices
		/// </summary>
		public bool UseLODFix = false;

		/// <summary>
		/// Should Lightmap cameras use Occlusion Culling.
		/// </summary>
		public bool UseOcclusionCulling = false;
	
		/// <summary>
		/// The amount of padding on a cascade's Z axis to ensure it can view all occluders.
		/// A value of 0.0 adds no padding.
		/// A value of 5.0 adds 500% padding.
		/// </summary>
		public float LightPaddingZ = 100f;
	
		/// <summary>
		/// The ratio of the shadow map's size that should be used for fading out.
		/// </summary>
		public float LightFadeRatio = 0.1f;
	
		/// <summary>
		/// The ratio of a cascade's size to the parent cascade's size.
		/// </summary>
		public float CascadeSpacing = 0.425f;

		/// <summary>
		/// Use manual cascade spacing.
		/// </summary>
		public bool UseManualCascadeSplits = false;

		/// <summary>
		/// The manual split between cascade0 and cascade1
		/// </summary>
		public float ManualCascadeSplit0 = 0.425f;

		/// <summary>
		/// The manual split between cascade1 and cascade2
		/// </summary>
		public float ManualCascadeSplit1 = 0.425f;

		/// <summary>
		/// The manual split between cascade2 and cascade3
		/// </summary>
		public float ManualCascadeSplit2 = 0.425f;
	
		/// <summary>
		/// The amount to disolve between shadow cascades.
		/// This number is not linear
		/// </summary>
		public float CascadeFade = 0.1f;
	
		/// <summary>
		/// Amount to tweak the Terrain LOD to prevent self-shadowing.
		/// </summary>
		public float TerrainLODTweak = 0f;
	
		/// <summary>
		/// The shadow filter to use.
		/// These will soften shadows, but will also impact performance.
		/// </summary>
		public SunshineShadowFilters ShadowFilter = SunshineShadowFilters.PCF3x3;

		/// <summary>
		/// Gets the shadow format.
		/// Right now, only Linear shadows are supported, as opposed to Exponential/Variance
		/// </summary>
		public SunshineShadowFormats ShadowFormat { get { return SunshineShadowFormats.Linear; } }

		/// <summary>
		/// Gets the RenderTextureFormat to use for shadows
		/// </summary>
		public RenderTextureFormat LightmapFormat { get { return RenderTextureFormat.ARGB32; } }

		[NonSerialized]
		public RenderTexture Lightmap = null;

		/// <summary>
		/// Gets the FilterMode to use for Shadow Maps... Right now, this will only be FilterMode.Point
		/// </summary>
		public FilterMode LightmapFilterMode { get { return FilterMode.Point; } }

		/// <summary>
		/// The scatter's resolution relative to the screen.
		/// </summary>
		public SunshineRelativeResolutions ScatterResolution = SunshineRelativeResolutions.Half;
	
		/// <summary>
		/// The number of scatter samples to take per-pixel.
		/// Low = 8 Samples
		/// Medium = 12 Samples
		/// High = 16 Samples
		/// VeryHigh = 20 Samples
		/// </summary>
		public SunshineScatterSamplingQualities ScatterSamplingQuality = SunshineScatterSamplingQualities.Medium;
	
		/// <summary>
		/// The scatter dither texture.
		/// This is used in a method very similar to SSAO, to create predictable sampling pattern.
		/// </summary> 
		[NonSerialized]
		public Texture2D ScatterDitherTexture;
	
		/// <summary>
		/// The blurring filter to use on scattering.
		/// </summary>
		public bool ScatterBlur = true;
	
		/// <summary>
		/// The the allowed ratio of depth disrepency between neighboring pixels for blur to operate.
		/// </summary>
		public float ScatterBlurDepthTollerance = 0.1f;
	
		/// <summary>
		/// True if scatter's dither texture should be animated/randomized
		/// </summary>
		public bool ScatterAnimateNoise = true;
	
		/// <summary>
		/// The scatter animate noise speed.
		/// </summary>
		public float ScatterAnimateNoiseSpeed = 0.1f;
	
		/// <summary>
		/// The color of the scatter.
		/// </summary>
		public Color ScatterColor = new Color (0.60f, 0.60f, 0.60f, 1.0f);

		/// <summary>
		/// True if post processing is supported
		/// </summary>
		public bool PostProcessSupported {
				get {
						return 
						//Not required, but if a system doesn't support Depth, postprocessing is a bad idea!
				SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth) &&
					
						SystemInfo.supportsImageEffects &&
						PostScatterShader.isSupported && (PostScatterMaterial && PostScatterMaterial.passCount == 2 && ScatterDitherTexture != null);
				}
		
		}

		/// <summary>
		/// True if post blur is supported
		/// </summary>
		public bool PostBlurSupported { get { return  (PostBlurShader.isSupported && PostBlurMaterial.passCount == 1); } }

		/// <summary>
		/// Enables/disables the scatter effect
		/// </summary>
		public bool ScatterEnabled = true;

		/// <summary>
		/// Gets whether or not scattering is enabled and supported
		/// </summary>
		public bool ScatterActive { get { return ScatterEnabled && ScatterIntensity > 0f && PostProcessSupported; } }

		/// <summary>
		/// The scatter's intensity.
		/// </summary>
		public float ScatterIntensity = 0.5f;
	
		/// <summary>
		/// The scatter's exaggeration.  This ramps up how quickly scatter achieves "full volume"
		/// </summary>
		public float ScatterExaggeration = 0.5f;
	
		/// <summary>
		/// The amount of scatter effect to apply to the skybox.
		/// </summary>
		public float ScatterSky = 0.0f;
	
		/// <summary>
		/// The ramp texture defining how lighting is attenuated based on view angle
		/// </summary>
		public Texture2D ScatterRamp = null;

		/// <summary>
		/// The texture to use for overcast shadows
		/// </summary>
		public Texture2D OvercastTexture = null;
	
		/// <summary>
		/// The scale of the overcast texture
		/// </summary>
		public float OvercastScale = 10f;
	
		/// <summary>
		/// The movevement of the overcast texture
		/// </summary>
		public Vector2 OvercastMovement = new Vector2 (1f, 0.5f);
	
		/// <summary>
		/// The height of the overcast plane in world space.
		/// </summary>
		public float OvercastPlaneHeight = 100f;
	
		/// <summary>
		/// Should overcast shadows affect scatter?
		/// </summary>
		public bool OvercastAffectsScatter = false;
	
		/// <summary>
		/// Should scattering use a custom set of overcast settings?
		/// </summary>
		public bool CustomScatterOvercast = false;
	
		/// <summary>
		/// The scatter's custom overcast texture.
		/// </summary>
		/// <seealso cref="ScatterOvercastTexture"/>
		public Texture2D ScatterOvercastTexture;
	
		/// <summary>
		/// The scatter's custom overcast scale.
		/// </summary>
		/// <seealso cref="OvercastScale"/>
		public float ScatterOvercastScale = 10f;
	
		/// <summary>
		/// The scatter's custom overcast movement.
		/// </summary>
		/// <seealso cref="OvercastMovement"/>
		public Vector2 ScatterOvercastMovement = new Vector2 (1f, 0.5f);

		/// <summary>
		/// The height of the scatter overcast plane.
		/// </summary>
		public float ScatterOvercastPlaneHeight = 100f;
	
		/// <summary>
		/// Internal: Used as a dummy overcast texture
		/// </summary>
		public Texture2D BlankOvercastTexture = null;
	
		/// <summary>
		/// Internal: The shader to use for Occluders
		/// </summary>
		public Shader OccluderShader;
		/// <summary>
		/// Internal: The shader to use for postprocess scattering
		/// </summary>
		public Shader PostScatterShader;
		/// <summary>
		/// A generated material for the postprocess scatter shader
		/// </summary>
		[NonSerialized]
		public Material PostScatterMaterial;
		/// <summary>
		/// Internal: The shader to use for postprocess scattering
		/// </summary>
		public Shader PostBlurShader;
		/// <summary>
		/// A generated material for the postprocess scatter shader
		/// </summary>
		[NonSerialized]
		public Material PostBlurMaterial;
		/// <summary>
		/// Internal: The shader to use for postprocess scattering
		/// </summary>
		public Shader PostDebugShader;
		/// <summary>
		/// A generated material for the postprocess scatter shader
		/// </summary>
		[NonSerialized]
		public Material PostDebugMaterial;
	
		/// <summary>
		/// The debug view.  Useful for visualizing Sunshine's status or effects.
		/// </summary>
		public SunshineDebugViews DebugView = SunshineDebugViews.None;

	
		/// <summary>
		/// Gets the primary cascade's camera.
		/// </summary>
		public Camera SunLightCamera { get { return SunLightCameras [0]; } }

		/// <summary>
		/// The cascade cameras.
		/// </summary>
	
		[NonSerialized]
		public Camera[] SunLightCameras = new Camera[MAX_CASCADES];
	
		public int CustomCascadeCount = 1;
		public float CustomLightDistance = 40f;

		/// <summary>
		/// Gets the number of cascades.
		/// </summary>
		public int CascadeCount {
				get {
						if (UsingCustomBounds || IsMobile || !SunshineProjectPreferences.Instance.UseCustomShadows)
								return 1;
						if (IdealLightmapResolution < 32)
								return 1;
						return CustomCascadeCount;
				}
		}

		public float LightDistance {
				get {
						if (UsingCustomBounds)
								return 9999f;
						return CustomLightDistance;
				}
		}

		/// <summary>
		/// Gets the cascade rects within the Lightmap.
		/// </summary>
		public Rect[] CascadeRects { get { return SunshineMath.CascadeViewportArrangements [Mathf.Clamp (CascadeCount - 1, 0, 3)]; } }

		/// <summary>
		/// Get a particular cascade rect within the Lightmap
		/// </summary>
		/// <returns>
		/// The cascade rect.
		/// </returns>
		/// <param name='cID'>
		/// The cascade index.
		/// </param>
		public Rect CascadeRect (int cID)
		{
				return CascadeRects [cID];
		}

		/// <summary>
		/// Gets a cascade rect in pixels.
		/// </summary>
		/// <returns>
		/// The cascade's pixel rect.
		/// </returns>
		/// <param name='cID'>
		/// The cascade index.
		/// </param>
		public Rect CascadePixelRect (int cID)
		{
				Rect result = CascadeRects [cID];
				float width = (float)(Lightmap != null ? Lightmap.width : 1);
				float height = (float)(Lightmap != null ? Lightmap.height : 1);
				result.x *= width;
				result.y *= height;
				result.width *= width;
				result.height *= height;
				return result;
		}

		/// <summary>
		/// Gets a cascade's near clip
		/// </summary>
		/// <returns>
		/// The near clip
		/// </returns>
		/// <param name='cID'>
		/// The cascade index
		/// </param>
		public float CascadeNearClip (int cID)
		{
				return CascadeNearClipScale (cID) * LightDistance;
		}

		/// <summary>
		/// Gets a cascade's far clip
		/// </summary>
		/// <returns>
		/// The far clip
		/// </returns>
		/// <param name='cID'>
		/// The cascade index
		/// </param>
		public float CascadeFarClip (int cID)
		{
				return CascadeFarClipScale (cID) * LightDistance;
		}

		/// <summary>
		/// Gets a cascade's near clip normalized to [0-1], where 1 is LightDistance 
		/// </summary>
		/// <returns>
		/// The near clip
		/// </returns>
		/// <param name='cID'>
		/// The cascade index
		/// </param>
		public float CascadeNearClipScale (int cID)
		{
				int previousClip = cID - 1;
				return (previousClip < 0) ? 0f : CascadeFarClipScale (previousClip); //Technically the nearest-clip isn't 0... but we'll ignore this for now...
		}

		/// <summary>
		/// Gets a cascade's far clip normalized to [0-1], where 1 is LightDistance
		/// </summary>
		/// <returns>
		/// The far clip
		/// </returns>
		/// <param name='cID'>
		/// The cascade index
		/// </param>
		public float CascadeFarClipScale (int cID)
		{
				if (cID >= CascadeCount - 1)
						return 1.0f;
				float farClip = 1.0f;
				if (UseManualCascadeSplits) {
						farClip = 0f;
						for (int i = 0; i <= cID; i++) {
								switch (i) {
								case 0:
										farClip += (1.0f - farClip) * ManualCascadeSplit0;
										break;
								case 1:
										farClip += (1.0f - farClip) * ManualCascadeSplit1;
										break;
								case 2:
										farClip += (1.0f - farClip) * ManualCascadeSplit2;
										break;
								}
						}
				} else {
						for (int i = CascadeCount - 1; i > cID; i--)
								farClip *= CascadeSpacing;
				}
				return farClip;
		}

		/// <summary>
		/// Finds a directional light for use as the Sun.
		/// </summary>
		/// <returns>
		/// An appropriate directional light.
		/// </returns>
		public Light FindAppropriateSunLight ()
		{
				Light[] lights = Light.GetLights (LightType.Directional, -1);
				if (lights.Length > 0)
						return lights [0];
				return null;
		}

		/// <summary>
		/// Initialization
		/// </summary>
		public bool Setup ()
		{
				SetupSingleton ();
				if (Application.isPlaying)
						Supported = Supported && SystemInfo.supportsRenderTextures;
				else
						Supported = SystemInfo.supportsRenderTextures;
		
				if (!Supported) {
						DestroyLightmap ();
						DisableShadows ();
						return false;
				}
		
				SetupLightmap ();
		
				if (Ready == true)
						return true;
		
				if (!Lightmap) {
						Supported = false;
						LogMessage ("Unable to create Lightmap");
				}
				if (!SunLight && Application.isPlaying)
						SunLight = FindAppropriateSunLight ();
				if (!SunLight) {
						LogMessage ("Sun Light was not configured, and couldn't find appropriate Direction Light...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
				if (!OccluderShader) {
						LogMessage ("Occluder Shader Missing...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
				if (!OccluderShader.isSupported) {
						LogMessage ("Occluder Shader Not Supported...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
				if (!PostScatterShader) {
						LogMessage ("Post Process Scatter Shader Missing...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
				if (!PostBlurShader) {
						LogMessage ("Post Process Blur Shader Missing...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
				if (!PostDebugShader) {
						LogMessage ("Post Process Debug Shader Missing...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
				if (!BlankOvercastTexture) {
						LogMessage ("Blank Overcast Texture Missing...");
						if (Application.isPlaying)
								enabled = false;
						return false;
				}
		
				RecreateMaterials ();
				RecreateRenderCameras ();
				RecreateTextures ();
		
				Ready = true;
				return true;
		}

		void OnDrawGizmos ()
		{
				if (UsingCustomBounds) {
						Gizmos.color = Color.yellow;
						Gizmos.DrawWireSphere (CustomLightBoundsOrigin.position, CustomLightBoundsRadius);
				}
		}

		void RecreateMaterials ()
		{
				DestroyMaterials ();
				if (!PostScatterMaterial) {
						PostScatterMaterial = new Material (PostScatterShader);
						PostScatterMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				if (!PostBlurMaterial) {
						PostBlurMaterial = new Material (PostBlurShader);
						PostBlurMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				if (!PostDebugMaterial) {
						PostDebugMaterial = new Material (PostDebugShader);
						PostDebugMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
		}

		void DestroyMaterials ()
		{
				if (PostScatterMaterial) {
						DestroyImmediate (PostScatterMaterial);
						PostScatterMaterial = null;
				}
				if (PostBlurMaterial) {
						DestroyImmediate (PostBlurMaterial);
						PostBlurMaterial = null;
				}
				if (PostDebugMaterial) {
						DestroyImmediate (PostDebugMaterial);
						PostDebugMaterial = null;
				}
		}

		public bool IsCascadeCamera (Camera camera)
		{
				for (int cascadeID = 0; cascadeID < MAX_CASCADES; cascadeID++) {
						if (SunLightCameras [cascadeID] == camera)
								return true;
				}
				return false;
		}

		void RecreateRenderCameras ()
		{
				DestroyRenderCameras ();
				for (int cascadeID = 0; cascadeID < MAX_CASCADES; cascadeID++) {
						if (SunLightCameras [cascadeID])
								continue;
						SunLightCameras [cascadeID] = CreateRenderCamera (string.Format ("Sunshine Cascade Camera {0}", cascadeID));
				}
		}

		void DestroyRenderCameras ()
		{
				for (int cascadeID = 0; cascadeID < MAX_CASCADES; cascadeID++) {
						if (SunLightCameras [cascadeID] == null)
								continue;
						DestroyImmediate (SunLightCameras [cascadeID].gameObject);
						SunLightCameras [cascadeID] = null;
				}
		}

		void RecreateTextures ()
		{
				DestroyTextures ();
				int max = 16;
				ScatterDitherTexture = new Texture2D (4, 4, TextureFormat.ARGB32, false, false);
				ScatterDitherTexture.hideFlags = HideFlags.HideAndDontSave;
				ScatterDitherTexture.filterMode = FilterMode.Point;
				var indices = new int[] {
						0,  8,  2,  10,
						12, 4,  14, 6,
						3,  11, 1,  9,
						15,  7,  13, 5
				};
				var colors = new Color[max];
				for (int i = 0; i < max; i++)
						colors [i] = new Color (0f, 0f, 0f, (float)indices [i] / (float)max);
				ScatterDitherTexture.SetPixels (colors);
		
				ScatterDitherTexture.Apply ();
		}

		void DestroyTextures ()
		{
				if (ScatterDitherTexture != null) {
						DestroyImmediate (ScatterDitherTexture);
						ScatterDitherTexture = null;
				}
		}

		void SetupSingleton ()
		{
				if (Instance == null)
						Instance = this;
				else if (Instance != this) {
						if (Application.isPlaying) {
								LogMessage ("Multiple Sunshine Instances detected!", true);
								Destroy (gameObject);
						}
				}
		}

		void Awake ()
		{
				SetupSingleton ();
				#if !UNITY_EDITOR
			Setup();
				#endif
		}

		void OnEnable ()
		{
				SetupSingleton ();
				#if !UNITY_EDITOR
			Setup();
				#endif
		}

		void Start ()
		{
				Setup ();
		}

		void OnDisable ()
		{
				DisableShadows ();
				DestroyResources ();
		}

		void OnDestroy ()
		{
				OnDisable ();
				if (Instance == this)
						Instance = null;
		}

	
		void Update ()
		{
				if (!Setup ())
						return;
		}

		/// <summary>
		/// Frees Resources
		/// </summary>
		void DestroyResources ()
		{
				DestroyLightmap ();
				DestroyRenderCameras ();
				DestroyMaterials ();
				DestroyTextures ();
				Ready = false;
				Supported = true;
		}

		/// <summary>
		/// Recreates the Lightmap.
		/// </summary>
		void RecreateLightmap ()
		{
				DestroyLightmap ();
				Lightmap = new RenderTexture (IdealLightmapWidth, IdealLightmapHeight, 16, LightmapFormat, RenderTextureReadWrite.Linear);
				if (Lightmap) {
						Lightmap.name = "Sunshine Lightmap";
						Lightmap.hideFlags = HideFlags.HideAndDontSave;
						//Lightmap.useMipMap = false;
						Lightmap.filterMode = LightmapFilterMode;
						Lightmap.wrapMode = TextureWrapMode.Clamp;
						Lightmap.Create ();
						Shader.SetGlobalTexture ("sunshine_Lightmap", Lightmap);
				}
		}

		/// <summary>
		/// Ensures the Lightmap is created, and in the correct format.
		/// </summary>
		void SetupLightmap ()
		{
				if (!Lightmap || Lightmap.width != IdealLightmapWidth || Lightmap.height != IdealLightmapHeight || Lightmap.format != LightmapFormat)
						RecreateLightmap ();
		}

		/// <summary>
		/// Destroies the Lightmap.
		/// </summary>
		void DestroyLightmap ()
		{
				if (Lightmap) {
						DestroyImmediate (Lightmap);
						Lightmap = null;
				}
		}

		/// <summary>
		/// Disables Shadows using Keywords...
		/// </summary>
		public void DisableShadows ()
		{
				SunshineKeywords.DisableShadows ();
		}

		/// <summary>
		/// Creates an inactive camera for rendering support data.
		/// </summary>
		/// <returns>
		/// A new camera.
		/// </returns>
		/// <param name='name'>
		/// Name to give the camera.
		/// </param>
		private static Camera CreateRenderCamera (string name)
		{
				var go = GameObject.Find (name);
				if (!go)
						go = new GameObject (name);
				Camera goCamera = go.GetComponent<Camera> ();
				if (!goCamera)
						goCamera = go.AddComponent<Camera> ();

				go.hideFlags = HideFlags.HideAndDontSave;
				goCamera.enabled = false;
				goCamera.renderingPath = RenderingPath.Forward;
				goCamera.nearClipPlane = 0.1f;
				goCamera.farClipPlane = 100f;
				goCamera.depthTextureMode = DepthTextureMode.None;
				goCamera.clearFlags = CameraClearFlags.SolidColor;
				goCamera.backgroundColor = Color.white;
				goCamera.orthographic = true;
				goCamera.hideFlags = HideFlags.HideAndDontSave;
		
#if UNITY_3_5
			go.active = false;
#else
				go.SetActive (false);
#endif
				return goCamera;
		}
	
}