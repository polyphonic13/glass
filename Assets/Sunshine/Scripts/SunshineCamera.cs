using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sunshine's renderer.
/// This should be attached to each camera, which is done automatically for you by default.
/// If "Auto Register Cameras" is disabled, you will need to add this component to Cameras as needed.
/// </summary>
[ExecuteInEditMode]
[RequireComponent (typeof(SunshinePostprocess))]
public class SunshineCamera : MonoBehaviour
{
		/// <summary>
		/// The stereoscopic master camera.
		/// This camera's shadowmap data will be reused for this camera.
		/// </summary>
		public SunshineCamera StereoscopicMasterCamera;
	
	
		Camera attachedCamera = null;

		/// <summary>
		/// Camera attached to the current object...
		/// Unity 5 drops the 'camera' getter, so we'll just add our own.
		/// </summary>
		Camera AttachedCamera {
				get {
						if (!attachedCamera)
								attachedCamera = GetComponent<Camera> ();
						return attachedCamera;
				}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="SunshineCamera"/> is ready for action.
		/// </summary>
		/// <value>
		/// <c>true</c> if ready; otherwise, <c>false</c>.
		/// </value>
		public bool GoodToGo {
				get {
						if (!enabled)
								return false;
						if (!Sunshine.Instance || !Sunshine.Instance.enabled)
								return false;
						if (ShadowsActive) {
								if (Sunshine.Instance.IsMobile && SystemInfo.graphicsShaderLevel < 20) //Require Shader Model 2 on Mobile...
					return false;
								if (!Sunshine.Instance.IsMobile && SystemInfo.graphicsShaderLevel < 30) //Require Shader Model 3 on Desktops...
					return false;
						} else if (!Sunshine.Instance.RequiresPostprocessing)
								return false;
						if (Sunshine.Instance.LightDistance <= 0f)
								return false;
						if (!Sunshine.Instance.Lightmap)
								return false;
						if (!Sunshine.Instance.SunLight)
								return false;
						if (!Sunshine.Instance.SunLight.enabled)
								return false;
						return Sunshine.Instance.Ready;
				}
		}

		public bool ForwardShadersOK {
				get {
						return SunshineProjectPreferences.Instance &&
						SunshineProjectPreferences.Instance.UseCustomShadows &&
						(SunshineProjectPreferences.Instance.ForwardShadersInstalled || SunshineProjectPreferences.Instance.ManualShaderInstallation);
				}
		}

		public bool ShadowsActive {
				get {
						return Sunshine.Instance && SunshineProjectPreferences.Instance.UseCustomShadows && Sunshine.Instance.SunLight &&
						(
						        (AttachedCamera.actualRenderingPath == RenderingPath.DeferredLighting) ||
						        (AttachedCamera.actualRenderingPath == RenderingPath.DeferredShading) ||
						        (ForwardShadersOK && AttachedCamera.actualRenderingPath == RenderingPath.Forward)
						);
				}
		}

		public float BoundsPadding {
				get {
						switch (Sunshine.Instance.UpdateInterval) {
						case SunshineUpdateInterval.AfterXFrames:
								return Sunshine.Instance.UpdateIntervalPadding;
						case SunshineUpdateInterval.AfterXMovement:
								return Sunshine.Instance.UpdateIntervalMovement; //The amount of Movement allowed is the ideal padding value...
						}
						return 0f;
				}
		}

		bool refreshRequested = false;

		public void RequestRefresh ()
		{
				refreshRequested = true;
		}

		Vector3 lastBoundsOrigin = Vector3.zero;

		public bool NeedsRefresh (Vector3 boundsOrigin)
		{
				if (!Application.isPlaying)
						return true;
				if (StereoscopicMasterCamera != null)
						return false;
				bool needsRefresh = refreshRequested;
				switch (Sunshine.Instance.UpdateInterval) {
				case SunshineUpdateInterval.EveryFrame:
						needsRefresh = true;
						break;
				case SunshineUpdateInterval.AfterXFrames:
						needsRefresh = needsRefresh || Time.frameCount <= 3 || (Time.frameCount % Sunshine.Instance.UpdateIntervalFrames) == 0;
						break;
				case SunshineUpdateInterval.AfterXMovement:
						if (Time.frameCount <= 3)
								needsRefresh = true;
						else {
								Vector3 delta = boundsOrigin - lastBoundsOrigin;
								needsRefresh = needsRefresh || delta.sqrMagnitude >= (Sunshine.Instance.UpdateIntervalMovement * Sunshine.Instance.UpdateIntervalMovement);
						}
						break;
				}
		
				if (needsRefresh)
						lastBoundsOrigin = boundsOrigin;

				return needsRefresh;
		}


	
		private SunshinePostprocess sunshinePostprocess = null;
	
		/// <summary>
		/// Temporarily stores the lightShadow type for restoration after rendering
		/// </summary>
		private LightShadows _lightShadows;
	
		/// <summary>
		/// Temporarily stores the renderMode type for restoration after rendering
		/// </summary>
		private LightRenderMode _lightRenderMode;

		void OnEnable ()
		{
				sunshinePostprocess = GetComponent<SunshinePostprocess> ();
				if (sunshinePostprocess == null)
						sunshinePostprocess = gameObject.AddComponent<SunshinePostprocess> ();
		}

		void OnDisable ()
		{
				SunshineKeywords.DisableShadows ();
		}

		void OnDestroy ()
		{
				SunshineKeywords.DisableShadows ();
		}

	
		List<Terrain> tempTerrainList = new List<Terrain> ();
		List<float> tempTerrainPixelError = new List<float> ();
		Dictionary<Camera, float[]> cachedCullDistances = new Dictionary<Camera, float[]> ();

		float[] GetCachedCullDistances (Camera cam)
		{
				if (!cachedCullDistances.ContainsKey (cam))
						cachedCullDistances.Add (cam, cam.layerCullDistances);
				return cachedCullDistances [cam];
		}

		SunshineMath.ShadowCameraTemporalData[] cascadeTemporalData = new SunshineMath.ShadowCameraTemporalData[Sunshine.MAX_CASCADES];

		/// <summary>
		/// Renders the cascades.
		/// </summary>
		void RenderCascades ()
		{
				SunshineMath.BoundingSphere shadowBoundingSphere = new SunshineMath.BoundingSphere ();
				if (Sunshine.Instance.UsingCustomBounds)
						shadowBoundingSphere = Sunshine.Instance.CustomBounds;
				else
						shadowBoundingSphere = SunshineMath.FrustumBoundingSphereBinarySearch (AttachedCamera, AttachedCamera.nearClipPlane, Sunshine.Instance.LightDistance, true, BoundsPadding, 0.01f, 20);
				if (!NeedsRefresh (shadowBoundingSphere.origin))
						return;
				bool tweakTerrain = (ShadowsActive && Sunshine.Instance.TerrainLODTweak > 0f);
				if (tweakTerrain) {
						tempTerrainList.Clear ();
						tempTerrainPixelError.Clear ();
						foreach (var t in Terrain.activeTerrains) {
								if (t) {
										tempTerrainList.Add (t);
										tempTerrainPixelError.Add (t.heightmapPixelError);
										t.heightmapPixelError *= (1.0f - Sunshine.Instance.TerrainLODTweak);
										//t.detailObjectDistance;
										//t.treeDistance;
										//t.treeBillboardDistance;
								}
						}
				}		
				for (int cID = 0; cID < Sunshine.Instance.CascadeCount; cID++) {
						Camera cascadeCamera = Sunshine.Instance.SunLightCameras [cID];
						cascadeCamera.cullingMask = Sunshine.Instance.GetCascadeOccluders (cID);
						SunshineMath.SetupShadowCamera (Sunshine.Instance.SunLight, cascadeCamera, AttachedCamera, Sunshine.Instance.CascadeNearClip (cID), Sunshine.Instance.CascadeFarClip (cID), Sunshine.Instance.LightPaddingZ, BoundsPadding, Sunshine.Instance.CascadeMapResolution, ref shadowBoundingSphere, ref cascadeTemporalData [cID]);
			
						//float cascadeScale = SunshineMath.ShadowTexelWorldSize(Sunshine.Configuration.CascadeMapResolution, cascadeCamera.orthographicSize) / cascade0TexelSize;
						//Shader.SetGlobalFloat("sunshine_DepthBias", Sunshine.Configuration.SunLight.shadowBias * cascadeScale);
			
						Shader.SetGlobalVector ("sunshine_DepthBiases", new Vector2 (Sunshine.Instance.ShadowBias (cID), Sunshine.Instance.ShadowSlopeBias (cID)));
			
						cascadeCamera.rect = Sunshine.Instance.CascadeRects [cID];
						cascadeCamera.targetTexture = Sunshine.Instance.Lightmap;

						cascadeCamera.useOcclusionCulling = Sunshine.Instance.UseOcclusionCulling;
						//Reset camera "time" somehow?.... Grass has different wind time on different cascades!?

						// New LOD and Visibility Hack...
						// Positions camera to match EyeCamera...
						// Tweaks Matrices to render correctly...
						if (Sunshine.Instance.UseLODFix) {
								var worldToCameraMatrix = cascadeCamera.worldToCameraMatrix;
								var projectionMatrix = cascadeCamera.projectionMatrix;
								var position = cascadeCamera.transform.position;
								var rotation = cascadeCamera.transform.rotation;
								var aspect = cascadeCamera.aspect;
								var fieldOfView = cascadeCamera.fieldOfView;
								var orthographic = cascadeCamera.orthographic;
								var orthographicSize = cascadeCamera.orthographicSize;
								//var nearClipPlane = cascadeCamera.nearClipPlane;
								//var farClipPlane = cascadeCamera.farClipPlane;

								cascadeCamera.transform.position = transform.position;
				
								//float resolutionScale = (float)cascadeCamera.pixelHeight / (float)AttachedCamera.pixelHeight;
				
								// This causes Billboards to be oriented improperly...
								// However, billboards are already problematic, so go for it...
								cascadeCamera.transform.rotation = transform.rotation;
								cascadeCamera.aspect = AttachedCamera.aspect;
								cascadeCamera.fieldOfView = AttachedCamera.fieldOfView;// * resolutionScale;
								cascadeCamera.orthographic = AttachedCamera.orthographic;
								if (cascadeCamera.orthographic)
										cascadeCamera.orthographicSize = AttachedCamera.orthographicSize;// * resolutionScale
								cascadeCamera.worldToCameraMatrix = worldToCameraMatrix;
								cascadeCamera.projectionMatrix = projectionMatrix;
				
								cascadeCamera.layerCullDistances = GetCachedCullDistances (AttachedCamera);
								cascadeCamera.layerCullSpherical = AttachedCamera.layerCullSpherical;

								cascadeCamera.RenderWithShader (Sunshine.Instance.OccluderShader, "RenderType");
				
								cascadeCamera.transform.position = position;
								cascadeCamera.transform.rotation = rotation;
				
								cascadeCamera.aspect = aspect;
								cascadeCamera.fieldOfView = fieldOfView;
								cascadeCamera.orthographic = orthographic;
								cascadeCamera.orthographicSize = orthographicSize;
				
								cascadeCamera.ResetWorldToCameraMatrix ();
								cascadeCamera.ResetProjectionMatrix ();
						} else
								cascadeCamera.RenderWithShader (Sunshine.Instance.OccluderShader, "RenderType");
				}
		
				if (tweakTerrain) {
						for (int i = tempTerrainList.Count - 1; i >= 0; i--) {
								var t = tempTerrainList [i];
								t.heightmapPixelError = tempTerrainPixelError [i];
						}
						tempTerrainList.Clear ();
						tempTerrainPixelError.Clear ();
				}

		
				refreshRequested = false;
		}

		/// <summary>
		/// Configures the cascade clip data for shaders.
		/// </summary>
		/// <param name='cameraFarClip'>
		/// The camera's far clip.
		/// </param>
		void ConfigureCascadeClips (float cameraFarClip)
		{
				float shadowToEyeRange = Sunshine.Instance.LightDistance / cameraFarClip;
				if (AttachedCamera.orthographic)
						shadowToEyeRange = 99999f;
				Vector4 cascadeNearRatios = new Vector4 (
						                            Sunshine.Instance.CascadeNearClipScale (0) * shadowToEyeRange,
						                            Sunshine.Instance.CascadeNearClipScale (1) * shadowToEyeRange,
						                            Sunshine.Instance.CascadeNearClipScale (2) * shadowToEyeRange,
						                            Sunshine.Instance.CascadeNearClipScale (3) * shadowToEyeRange);
				Shader.SetGlobalVector ("sunshine_CascadeNearRatiosSq", new Vector4 (cascadeNearRatios.x * cascadeNearRatios.x, cascadeNearRatios.y * cascadeNearRatios.y, cascadeNearRatios.z * cascadeNearRatios.z, cascadeNearRatios.w * cascadeNearRatios.w));
				Vector4 cascadeFarRatios = new Vector4 (
						                           Sunshine.Instance.CascadeFarClipScale (0) * shadowToEyeRange,
						                           Sunshine.Instance.CascadeFarClipScale (1) * shadowToEyeRange,
						                           Sunshine.Instance.CascadeFarClipScale (2) * shadowToEyeRange,
						                           Sunshine.Instance.CascadeFarClipScale (3) * shadowToEyeRange);
				Shader.SetGlobalVector ("sunshine_CascadeFarRatiosSq", new Vector4 (cascadeFarRatios.x * cascadeFarRatios.x, cascadeFarRatios.y * cascadeFarRatios.y, cascadeFarRatios.z * cascadeFarRatios.z, cascadeFarRatios.w * cascadeFarRatios.w));

				float lightDistance = AttachedCamera.orthographic ? 999999999f : Sunshine.Instance.LightDistance;
				float lightFadeRatio = AttachedCamera.orthographic ? 0.00001f : Sunshine.Instance.LightFadeRatio;
				float fadeRange = Mathf.Clamp (lightFadeRatio, 0.000001f, 1f);
				//float fadeOffset = 1f / fadeRange;
				float fadeOffset = 1f / Mathf.Sqrt (fadeRange); //Work in r^2 space to save sqrt() in shader!
				float fadeScale = (cameraFarClip / lightDistance) * fadeOffset;
				fadeOffset *= fadeOffset; //Work in r^2 space to save sqrt() in shader!
				fadeScale *= fadeScale; //Work in r^2 space to save sqrt() in shader!
				Shader.SetGlobalVector ("sunshine_ShadowFadeParams", new Vector3 (fadeOffset, fadeScale, shadowToEyeRange));

		}

		/// <summary>
		/// Configures the overcast settings for shaders
		/// </summary>
		/// <param name='overcastEnabled'>
		/// True, if overcast is enabled
		/// </param>
		/// <param name='overcastTexture'>
		/// Overcast texture.
		/// </param>
		/// <param name='overcastScale'>
		/// Overcast scale.
		/// </param>
		/// <param name='overcastMovement'>
		/// Overcast movement vector.
		/// </param>
		/// <param name='rotateMovement'>
		/// True if movement should maintain the same heading regardless of the sun direction.
		/// </param>
		void ConfigureOvercast (bool overcastEnabled, Texture2D overcastTexture, float overcastScale, Vector2 overcastMovement, float overcastPlaneHeight = 0f)
		{
				if (!overcastTexture)
						overcastEnabled = false;
				Shader.SetGlobalTexture ("sunshine_OvercastMap", overcastEnabled ? overcastTexture : Sunshine.Instance.BlankOvercastTexture);

				SunshineKeywords.ToggleOvercast (overcastEnabled);
				if (overcastEnabled) {
						Camera sunC = Sunshine.Instance.SunLightCamera;
			
						Ray startRay = sunC.ViewportPointToRay (new Vector3 (0f, 0f, 0f));
			
						//All rays are parallel, so these values only need to be calculated once:
						float rayLength = sunC.farClipPlane - sunC.nearClipPlane;
						float rayHeight = startRay.direction.y * rayLength;
			
						//Avoid divide-by-zero:
						if (Mathf.Abs (rayHeight) < 0.001f)
								rayHeight = 0.001f;
			
						float startHitRatio = (overcastPlaneHeight - startRay.origin.y) / rayHeight;
						Vector3 startPoint = startRay.GetPoint (startHitRatio * rayLength);
			
						Ray finishRayU = sunC.ViewportPointToRay (new Vector3 (1f, 0f, 0f));
						float finishHitRatioU = (overcastPlaneHeight - finishRayU.origin.y) / rayHeight;
						Vector3 finishPointU = finishRayU.GetPoint (finishHitRatioU * rayLength);
			
						Ray finishRayV = sunC.ViewportPointToRay (new Vector3 (0f, 1f, 0f));
						float finishHitRatioV = (overcastPlaneHeight - finishRayV.origin.y) / rayHeight;
						Vector3 finishPointV = finishRayV.GetPoint (finishHitRatioV * rayLength);

						Vector3 movement = new Vector3 (overcastMovement.x, 0f, overcastMovement.y) * Time.timeSinceLevelLoad;
			
						startPoint += movement;
						finishPointU += movement;
						finishPointV += movement;
			
						Vector2 startCoord = new Vector2 (startPoint.x, startPoint.z);
						Vector2 rayCoordU = new Vector2 (finishPointU.x, finishPointU.z) - startCoord;
						Vector2 rayCoordV = new Vector2 (finishPointV.x, finishPointV.z) - startCoord;
						
						Rect rect0 = Sunshine.Instance.CascadeRect (0);

						Vector2 overcastCoord = new Vector2 (
								                        startCoord.x,
								                        startCoord.y
						                        );
						Vector4 overcastVectorsUV = new Vector4 (
								                            rayCoordU.x / rect0.width,
								                            rayCoordU.y / rect0.width,
								                            rayCoordV.x / rect0.height,
								                            rayCoordV.y / rect0.height
						                            );
						Shader.SetGlobalVector ("sunshine_OvercastCoord", overcastCoord * (1f / overcastScale));
						Shader.SetGlobalVector ("sunshine_OvercastVectorsUV", overcastVectorsUV * (1f / overcastScale));
				}
		}

		/// <summary>
		/// Configures shaders for rendering.
		/// </summary>
		void ConfigureShaders ()
		{
				if (ShadowsActive)
						SunshineKeywords.SetFilterStyle (Sunshine.Instance.ShadowFilter);
				else
						SunshineKeywords.DisableShadows ();

				Matrix4x4 sunProjectionMatrix = Matrix4x4.identity;
		
				//if(Sunshine.PRE_TRANSFORM_INTO_CASCADE0)
				{
						//Optimization: Pre-transform into the first Cascade's coordinates...
						//Modify the Sun's Projection Matrix to be in Texture Space of the first cascade...
						sunProjectionMatrix = SunshineMath.ToRectSpaceProjection (Sunshine.Instance.CascadeRect (0)) * Sunshine.Instance.SunLightCamera.projectionMatrix;
				}
				/*
		else
		{
			//Modify the Sun's Projection Matrix to be in Texture Space (0-1)
			sunProjectionMatrix = SunshineMath.ToTextureSpaceProjection * Sunshine.Instance.SunLightCamera.projectionMatrix;
		}
		*/
		
				//Modify the Sun's Projection Matrix again to make it preserve linear depth, and pre-scale it into 0-1 space
				SunshineMath.SetLinearDepthProjection (ref sunProjectionMatrix, Sunshine.Instance.SunLightCamera.farClipPlane);
		
				Matrix4x4 sunshine_WorldToSunVP =
						sunProjectionMatrix *
						Sunshine.Instance.SunLightCamera.worldToCameraMatrix;

				Matrix4x4 sunshine_CameraVToSunVP =
						sunshine_WorldToSunVP *
						AttachedCamera.cameraToWorldMatrix;

				Shader.SetGlobalMatrix ("sunshine_CameraVToSunVP", sunshine_CameraVToSunVP);
				Shader.SetGlobalMatrix ("sunshine_WorldToSunVP", sunshine_WorldToSunVP);
		
				float resolution = (float)Sunshine.Instance.Lightmap.width;
		
				Shader.SetGlobalVector ("sunshine_ShadowParamsAndHalfTexel", new Vector4 (Sunshine.Instance.SunLight.shadowStrength, Sunshine.Instance.CascadeFade, 0.5f / resolution, 0.5f / resolution));
		
				ConfigureCascadeClips (AttachedCamera.farClipPlane);
		
				Vector3 worldEyeOrigin = AttachedCamera.orthographic ? AttachedCamera.ViewportToWorldPoint (new Vector3 (0f, 0f, 0f)) : transform.position;
				Vector3 worldEyeFar00 = AttachedCamera.ViewportToWorldPoint (new Vector3 (0f, 0f, AttachedCamera.farClipPlane));
				Vector3 worldEyeFar10 = AttachedCamera.ViewportToWorldPoint (new Vector3 (1f, 0f, AttachedCamera.farClipPlane));
				Vector3 worldEyeFar01 = AttachedCamera.ViewportToWorldPoint (new Vector3 (0f, 1f, AttachedCamera.farClipPlane));
		
				var sunLightCameraTransform = Sunshine.Instance.SunLightCamera.transform; // Can't globally cache, SunLightCamera can change...
				Vector3 sunEyePos = sunLightCameraTransform.InverseTransformPoint (worldEyeOrigin);
				Vector3 sunEyeFarPos00 = sunLightCameraTransform.InverseTransformPoint (worldEyeFar00);
				Vector3 sunEyeFarPos10 = sunLightCameraTransform.InverseTransformPoint (worldEyeFar10);
				Vector3 sunEyeFarPos01 = sunLightCameraTransform.InverseTransformPoint (worldEyeFar01);
		
				Vector2 sunEyeTex = SunshineMath.xy (Sunshine.Instance.SunLightCamera.WorldToViewportPoint (worldEyeOrigin));
				Vector2 sunEyeFarTex00 = SunshineMath.xy (Sunshine.Instance.SunLightCamera.WorldToViewportPoint (worldEyeFar00));
				Vector2 sunEyeFarTex10 = SunshineMath.xy (Sunshine.Instance.SunLightCamera.WorldToViewportPoint (worldEyeFar10));
				Vector2 sunEyeFarTex01 = SunshineMath.xy (Sunshine.Instance.SunLightCamera.WorldToViewportPoint (worldEyeFar01));
		
				Vector4 shadowCoordDepthFar00 = new Vector4 (sunEyeFarTex00.x, sunEyeFarTex00.y, sunEyeFarPos00.z / Sunshine.Instance.SunLightCamera.farClipPlane, sunEyePos.y);
		
				Vector4 shadowCoordDepthStart = new Vector4 (sunEyeTex.x, sunEyeTex.y, sunEyePos.z / Sunshine.Instance.SunLightCamera.farClipPlane, sunEyePos.y);
				Vector4 shadowCoordDepthRayZ = shadowCoordDepthFar00 - shadowCoordDepthStart;
				Vector4 shadowCoordDepthRayU = new Vector4 (sunEyeFarTex10.x, sunEyeFarTex10.y, sunEyeFarPos10.z / Sunshine.Instance.SunLightCamera.farClipPlane, sunEyeFarPos10.y) - shadowCoordDepthFar00;
				Vector4 shadowCoordDepthRayV = new Vector4 (sunEyeFarTex01.x, sunEyeFarTex01.y, sunEyeFarPos01.z / Sunshine.Instance.SunLightCamera.farClipPlane, sunEyeFarPos01.y) - shadowCoordDepthFar00;
		
				Rect cascadeViewport0 = Sunshine.Instance.CascadeRect (0);
				//if(Sunshine.PRE_TRANSFORM_INTO_CASCADE0)
				{
						SunshineMath.ShadowCoordDataInRect (ref shadowCoordDepthStart, ref cascadeViewport0);
						SunshineMath.ShadowCoordDataRayInRect (ref shadowCoordDepthRayZ, ref cascadeViewport0);
						SunshineMath.ShadowCoordDataRayInRect (ref shadowCoordDepthRayU, ref cascadeViewport0);
						SunshineMath.ShadowCoordDataRayInRect (ref shadowCoordDepthRayV, ref cascadeViewport0);
				}
		
				Shader.SetGlobalFloat ("sunshine_IsOrthographic", AttachedCamera.orthographic ? 1.0f : 0.0f);
				Shader.SetGlobalVector ("sunshine_ShadowCoordDepthStart", shadowCoordDepthStart);
				Shader.SetGlobalVector ("sunshine_ShadowCoordDepthRayZ", shadowCoordDepthRayZ);
				Shader.SetGlobalVector ("sunshine_ShadowCoordDepthRayU", shadowCoordDepthRayU);
				Shader.SetGlobalVector ("sunshine_ShadowCoordDepthRayV", shadowCoordDepthRayV);
		
				float shadowSizeXY = Sunshine.Instance.SunLightCamera.orthographicSize * 2.0f;
				Vector2 shadowSize2D = new Vector2 (shadowSizeXY, shadowSizeXY);
				//if(Sunshine.PRE_TRANSFORM_INTO_CASCADE0)
				{
						shadowSize2D.x /= cascadeViewport0.width;
						shadowSize2D.y /= cascadeViewport0.height;
				}
				Vector3 shadowToWorldScale = new Vector3 (shadowSize2D.x, shadowSize2D.y, Sunshine.Instance.SunLightCamera.farClipPlane) / AttachedCamera.farClipPlane;
				Shader.SetGlobalVector ("sunshine_ShadowToWorldScale", shadowToWorldScale);
		
		
				Matrix4x4 cascadeRanges = Matrix4x4.zero;
				Vector3 world00 = Sunshine.Instance.SunLightCamera.ViewportToWorldPoint (new Vector3 (0f, 0f, 0f));
				Vector3 world11 = Sunshine.Instance.SunLightCamera.ViewportToWorldPoint (new Vector3 (1f, 1f, 0f));
				for (int cID = 0; cID < Sunshine.Instance.CascadeCount; cID++) {
						Vector4 cascadeRange = new Vector4 (0f, 0f, 1f, 1f);
						if (cID > 0) { //Save some CPU on the first cascade. ;)
								Camera cascadeCamera = Sunshine.Instance.SunLightCameras [cID];
								Vector3 min = cascadeCamera.WorldToViewportPoint (world00);
								Vector3 max = cascadeCamera.WorldToViewportPoint (world11);
								cascadeRange = new Vector4 (min.x, min.y, max.x, max.y);
						}
						Rect cascadeViewport = Sunshine.Instance.CascadeRect (cID);
			
						cascadeRange.x = cascadeViewport.xMin + cascadeViewport.width * cascadeRange.x;
						cascadeRange.y = cascadeViewport.yMin + cascadeViewport.height * cascadeRange.y;
						cascadeRange.z = cascadeViewport.xMin + cascadeViewport.width * cascadeRange.z;
						cascadeRange.w = cascadeViewport.yMin + cascadeViewport.height * cascadeRange.w;
			
			
						cascadeRange.z = cascadeRange.z - cascadeRange.x; //Change to a Delta to avoid lerp() on gpu...
						cascadeRange.w = cascadeRange.w - cascadeRange.y; //Change to a Delta to avoid lerp() on gpu...
			
						cascadeRanges.SetRow (cID, cascadeRange); //SetRow, or SetColumn???
				}
		
				//if(Sunshine.PRE_TRANSFORM_INTO_CASCADE0)
				{
						Vector4 range0 = cascadeRanges.GetRow (0);
						for (int cID = 0; cID < Sunshine.Instance.CascadeCount; cID++) {
								Vector4 range = cascadeRanges.GetRow (cID);
								range.z /= range0.z;
								range.w /= range0.w;
								cascadeRanges.SetRow (cID, range);
						}
				}
				bool overcastEnabled = (Sunshine.Instance.OvercastTexture);
		
				ConfigureOvercast (overcastEnabled, overcastEnabled ? Sunshine.Instance.OvercastTexture : Sunshine.Instance.BlankOvercastTexture, Sunshine.Instance.OvercastScale, Sunshine.Instance.OvercastMovement, Sunshine.Instance.OvercastPlaneHeight);
		
				// Assuming the cascades are square, we can use .xx and .y can be the z-offset...
				// This allows staggared cascade rendering without adjusting the depth texture!
				Shader.SetGlobalMatrix ("sunshine_CascadeRanges", cascadeRanges);
				SunshineKeywords.SetCascadeCount (Sunshine.Instance.CascadeCount);
		}

		void Update ()
		{
				if (!Sunshine.Instance)
						return;
				bool postRequired = Sunshine.Instance.RequiresPostprocessing;
				if (sunshinePostprocess) {
						if (sunshinePostprocess.enabled != postRequired)
								sunshinePostprocess.enabled = postRequired;
				}
				if (StereoscopicMasterCamera != null) {
						if (StereoscopicMasterCamera.StereoscopicMasterCamera == this)
								StereoscopicMasterCamera = null; // No recursion allowed
			else
								AttachedCamera.depth = StereoscopicMasterCamera.AttachedCamera.depth + 1;
				}

		}
	
		#if UNITY_EDITOR
		void OnGUI ()
		{
				if (!Sunshine.Instance)
						return;
				if (Sunshine.Instance.DebugView == SunshineDebugViews.Status) {
						string depthModes = "";
						if ((AttachedCamera.depthTextureMode & DepthTextureMode.Depth) != 0)
								depthModes += " Depth";
						if ((AttachedCamera.depthTextureMode & DepthTextureMode.DepthNormals) != 0)
								depthModes += " DepthNormals";
			
						string debug = string.Format ("::Sunshine {0} Status::\n- - - - - - - - - - - - - - - - - - - -\n", Sunshine.Version) +
						               string.Format ("Sunshine Enabled: {0}\n", Sunshine.Instance.enabled) +
						               string.Format ("Sunshine GoodToGo: {0}\n", GoodToGo) +
						               string.Format ("Sunshine Cascades: {0}\n", Sunshine.Instance.CascadeCount) +
						               string.Format ("Use Custom Shadows: {0}\n", SunshineProjectPreferences.Instance.UseCustomShadows) +
						               string.Format ("Forward Shaders OK: {0}\n", ForwardShadersOK) +
						               string.Format ("Forward Shadows: {0}\n", ShadowsActive) +
						               string.Format ("Scatter Enabled: {0}\n", Sunshine.Instance.ScatterEnabled) +
						               string.Format ("Scatter Active: {0}\n", Sunshine.Instance.ScatterActive) +
						               string.Format ("Scatter Shader Exists: {0}\n", Sunshine.Instance.PostScatterShader != null) +
						               string.Format ("Scatter Material Exists: {0}\n", Sunshine.Instance.PostScatterMaterial != null) +
						               string.Format ("Scatter Shader Supported: {0}\n", Sunshine.Instance.PostScatterShader.isSupported) +
						               string.Format ("Scatter Noise Texture Exists: {0}\n", Sunshine.Instance.ScatterDitherTexture != null) +
						               string.Format ("Blur Shader Exists: {0}\n", Sunshine.Instance.PostBlurShader != null) +
						               string.Format ("Blur Material Exists: {0}\n", Sunshine.Instance.PostBlurMaterial != null) +
						               string.Format ("Blur Shader Supported: {0}\n", Sunshine.Instance.PostBlurShader.isSupported) +
						               string.Format ("Supports Depth: {0}\n", SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth)) +
						               string.Format ("Supports Image Effects: {0}\n", SystemInfo.supportsImageEffects) +
						               string.Format ("Shader Level: {0}\n", SystemInfo.graphicsShaderLevel) +
						               string.Format ("Camera.depthTextureMode:{0}", depthModes);
			
						GUILayout.TextArea (debug);
				}
		}
		#endif
	
		void OnPreCull ()
		{
				if (!GoodToGo) {
						SunshineKeywords.DisableShadows ();
						return;
				}
		
				RenderCascades ();
		}

		void OnPreRender ()
		{
				if (!GoodToGo)
						return;
				
				ConfigureShaders ();

				if (ShadowsActive) {
						_lightShadows = Sunshine.Instance.SunLight.shadows;
						_lightRenderMode = Sunshine.Instance.SunLight.renderMode;
						Sunshine.Instance.SunLight.shadows = LightShadows.None; //Disable built-in shadows
						Sunshine.Instance.SunLight.renderMode = LightRenderMode.ForcePixel; //Force per-pixel lighting
				}

				if (Sunshine.Instance.RequiresPostprocessing) {
						//Check for existing depth textures...
						bool usingDepth = (AttachedCamera.depthTextureMode & DepthTextureMode.Depth) != 0;
						if (!usingDepth) {
								if (SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth))
										AttachedCamera.depthTextureMode |= DepthTextureMode.Depth;
						}
				}
		}

		void OnPostRender ()
		{
				if (ShadowsActive) {
						Sunshine.Instance.SunLight.shadows = _lightShadows;
						Sunshine.Instance.SunLight.renderMode = _lightRenderMode;
				}
		
				//Turn off Sunshine in the Editor...
				SunshineKeywords.DisableShadows ();
		}

		private float scatterNoiseSeed = 0f;

		public void OnPostProcess (RenderTexture source, RenderTexture destination)
		{
				if (!GoodToGo) {
						Graphics.Blit (source, destination);
						return;
				}
	
				if (Sunshine.Instance.DebugView == SunshineDebugViews.Cascades) {
						SunshinePostprocess.Blit (source, destination, Sunshine.Instance.PostDebugMaterial, SunshinePostDebugPass.DebugCascades);
				} else {
						bool postProcessLightRays = Sunshine.Instance.ScatterActive;

						if (postProcessLightRays) {
								bool scatterBlur = Sunshine.Instance.ScatterBlur;	
								if (!Sunshine.Instance.PostBlurSupported)
										scatterBlur = false;
				
								RenderTexture rayRT = null;
				
								bool overcastEnabled = Sunshine.Instance.OvercastAffectsScatter && (Sunshine.Instance.OvercastTexture || Sunshine.Instance.ScatterOvercastTexture);
								bool customScatter = Sunshine.Instance.CustomScatterOvercast;
								Texture2D overcastTexture = overcastEnabled ? (customScatter ? Sunshine.Instance.ScatterOvercastTexture : Sunshine.Instance.OvercastTexture) : null;
								float overcastScale = customScatter ? Sunshine.Instance.ScatterOvercastScale : Sunshine.Instance.OvercastScale;
								Vector2 overcastMovement = customScatter ? Sunshine.Instance.ScatterOvercastMovement : Sunshine.Instance.OvercastMovement;
								float overcastPlaneHeight = customScatter ? Sunshine.Instance.ScatterOvercastPlaneHeight : Sunshine.Instance.OvercastPlaneHeight;
								ConfigureOvercast (overcastEnabled, overcastTexture, overcastScale, overcastMovement, overcastPlaneHeight);

								// View Dependance:
								var startRay = AttachedCamera.ViewportPointToRay (new Vector3 (0f, 0f, 0f)).direction;
								var rayU = AttachedCamera.ViewportPointToRay (new Vector3 (1f, 0f, 0f)).direction - startRay;
								var rayV = AttachedCamera.ViewportPointToRay (new Vector3 (0f, 1f, 0f)).direction - startRay;
								Sunshine.Instance.PostScatterMaterial.SetVector ("worldLightRay", Sunshine.Instance.SunLight.transform.forward); // Can't cache transform, SunLight can change...
								Sunshine.Instance.PostScatterMaterial.SetVector ("worldRay", startRay);
								Sunshine.Instance.PostScatterMaterial.SetVector ("worldRayU", rayU);
								Sunshine.Instance.PostScatterMaterial.SetVector ("worldRayV", rayV);
								Sunshine.Instance.PostScatterMaterial.SetTexture ("_ScatterRamp", Sunshine.Instance.ScatterRamp);
			
								SunshineKeywords.SetScatterQuality (Sunshine.Instance.ScatterSamplingQuality);
								Sunshine.Instance.PostScatterMaterial.SetVector ("ScatterColor", Sunshine.Instance.ScatterColor);
								if (Sunshine.Instance.ScatterAnimateNoise) {
										scatterNoiseSeed += Time.deltaTime * Sunshine.Instance.ScatterAnimateNoiseSpeed;
										scatterNoiseSeed -= Mathf.Floor (scatterNoiseSeed);
								}
								Sunshine.Instance.PostScatterMaterial.SetTexture ("ScatterDitherMap", Sunshine.Instance.ScatterDitherTexture);

								float scatterFade = 1f - Sunshine.Instance.ScatterExaggeration;
								float dustVolumeScale = 1f / ((Mathf.Clamp01 (scatterFade) * Sunshine.Instance.LightDistance) / AttachedCamera.farClipPlane);
								float skyTerm = Sunshine.Instance.ScatterSky * Sunshine.Instance.ScatterIntensity;
								Sunshine.Instance.PostScatterMaterial.SetVector ("ScatterIntensityVolumeSky", new Vector4 (Sunshine.Instance.ScatterIntensity, dustVolumeScale, skyTerm * 0.333f, skyTerm * 0.667f));
				
								bool blitScatterDirectlyToScreen = (Sunshine.Instance.ScatterResolution == SunshineRelativeResolutions.Full && !scatterBlur && Sunshine.Instance.DebugView != SunshineDebugViews.Scatter);
								if (!blitScatterDirectlyToScreen) {
										//Allocate Buffers
										int scatterDiv = SunshineMath.RelativeResolutionDivisor (Sunshine.Instance.ScatterResolution);
										int2 scatterResolution = new int2 (source.width, source.height) / scatterDiv;
										scatterResolution.x = Mathf.Max (scatterResolution.x, 1);
										scatterResolution.y = Mathf.Max (scatterResolution.y, 1);
										Sunshine.Instance.PostScatterMaterial.SetVector ("ScatterDitherData", new Vector3 ((float)scatterResolution.x / (float)Sunshine.Instance.ScatterDitherTexture.width, (float)scatterResolution.y / (float)Sunshine.Instance.ScatterDitherTexture.height, Sunshine.Instance.ScatterAnimateNoise ? scatterNoiseSeed : 0f));
										rayRT = RenderTexture.GetTemporary (scatterResolution.x, scatterResolution.y, 0, source.format, RenderTextureReadWrite.Default);
										if (rayRT) {
												//rayRT.useMipMap = false;
												rayRT.filterMode = FilterMode.Point;
												rayRT.wrapMode = TextureWrapMode.Clamp;

												//Draw Rays... Blend Later whether we blur or not, since it needs to be applied to the full resolution scene buffer...
												SunshinePostprocess.Blit (source, rayRT, Sunshine.Instance.PostScatterMaterial, SunshinePostScatterPass.DrawScatter);
	
												if (scatterBlur) {
														Sunshine.Instance.PostBlurMaterial.SetFloat ("BlurDepthTollerance", Sunshine.Instance.ScatterBlurDepthTollerance);
														RenderTexture rayRT_PingPong = RenderTexture.GetTemporary (rayRT.width, rayRT.height, 0, rayRT.format, RenderTextureReadWrite.Default);
														if (rayRT_PingPong) {
																//rayRT_PingPong.useMipMap = rayRT.useMipMap;
																rayRT_PingPong.filterMode = rayRT.filterMode;
																rayRT_PingPong.wrapMode = rayRT.wrapMode;
								
																//Blur Rays Horizontally
																Sunshine.Instance.PostBlurMaterial.SetVector ("BlurXY", new Vector2 (1f, 0f));
																SunshinePostprocess.Blit (rayRT, rayRT_PingPong, Sunshine.Instance.PostBlurMaterial, 0);
																rayRT.DiscardContents ();
								
																//Blur Rays Vertically
																Sunshine.Instance.PostBlurMaterial.SetVector ("BlurXY", new Vector2 (0f, 1f));
																SunshinePostprocess.Blit (rayRT_PingPong, rayRT, Sunshine.Instance.PostBlurMaterial, 0);
																RenderTexture.ReleaseTemporary (rayRT_PingPong);
														}
												}
	
												rayRT.filterMode = FilterMode.Bilinear;
						
												//Apply Rays
												if (Sunshine.Instance.DebugView == SunshineDebugViews.Scatter) {
														SunshinePostprocess.Blit (rayRT, destination, Sunshine.Instance.PostDebugMaterial, SunshinePostDebugPass.DebugAlpha); //Draw Rays...
												} else {
														Sunshine.Instance.PostScatterMaterial.SetTexture ("_ScatterTexture", rayRT);
														SunshinePostprocess.Blit (source, destination, Sunshine.Instance.PostScatterMaterial, SunshinePostScatterPass.ApplyScatter); //Apply Rays
												}
												RenderTexture.ReleaseTemporary (rayRT);
										} else {
												blitScatterDirectlyToScreen = true;
										}
								}
								if (blitScatterDirectlyToScreen) {
										Sunshine.Instance.PostScatterMaterial.SetVector ("ScatterDitherData", new Vector3 (AttachedCamera.pixelWidth / (float)Sunshine.Instance.ScatterDitherTexture.width, AttachedCamera.pixelHeight / (float)Sunshine.Instance.ScatterDitherTexture.height, Sunshine.Instance.ScatterAnimateNoise ? scatterNoiseSeed : 0f));
										SunshinePostprocess.Blit (source, destination, Sunshine.Instance.PostScatterMaterial, SunshinePostScatterPass.DrawScatter);
								}
						}
				}
		}

}
