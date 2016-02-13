using UnityEngine;
using System.Collections;


/// <summary>
/// Internally used for most of Sunshine's math operations...
/// </summary>
public static class SunshineMath
{
		/// <summary>
		/// Estimates the Shadow Resolution that Unity internally calculates
		/// </summary>
		/// <returns>
		/// The resolution that Unity is estimated to use...
		/// </returns>
		/// <param name='resolution'>
		/// An approximate Resolution quality...
		/// </param>
		public static int UnityStyleLightmapResolution (SunshineLightResolutions resolution)
		{
				if (resolution == SunshineLightResolutions.Custom)
						return 0;
				//Excerpt from the Unity Documentation:
				/*
			Directional lights: NextPowerOfTwo( pixel size * 1.9 ), but no more than 2048.
			When graphics card has 512MB or more video memory, the upper shadow map limits are increased (4096 for Directional, 2048 for Spot, 1024 for Point lights).
			At "Medium" shadow resolution, shadow map size is 2X smaller than at "High" resolution. And at "Low" resolution, it's 4X smaller than at "High" resolution.
		*/
				int pixelSize = Mathf.Max (Screen.width, Screen.height);
				int result = Mathf.NextPowerOfTwo ((int)((float)pixelSize * 1.9f));
				int dimensionLimit = 2048;
				if (SystemInfo.graphicsMemorySize >= 512)
						dimensionLimit = 4096;
		
				result = Mathf.Min (dimensionLimit, result); //Yes, both Min()'s appear to be required to simulate Unity Sizing...
				switch (resolution) {
				case SunshineLightResolutions.LowResolution:
						result /= 4;
						break;
				case SunshineLightResolutions.MediumResolution:
						result /= 2;
						break;
				case SunshineLightResolutions.VeryHighResolution:
						result *= 2;
						break;
				}
				result = Mathf.Min (dimensionLimit, result); //Yes, both Min()'s appear to be required to simulate Unity Sizing...
		
				//TODO: Reduce until Lightmap fits into...
				//(TotalVideoMemory - ScreenMemory - RenderTextureMemory) / 3.
			
				return result;
		}

		/// <summary>
		/// Computes the size of a shadow texel in world space
		/// </summary>
		/// <returns>
		/// The texel size in world units
		/// </returns>
		/// <param name='resolution'>
		/// The texture Resolution
		/// </param>
		/// <param name='orthographicSize'>
		/// Orthographic size of the camera.
		/// </param>
		public static float ShadowTexelWorldSize (float resolution, float orthographicSize)
		{
				return (orthographicSize * 2.0f) / resolution;
		}

		/// <summary>
		/// Returns a Matrix4x4 that will convert a projection matrix from [-1,1] space into [0,1] space
		/// </summary>
		public static readonly Matrix4x4 ToTextureSpaceProjection = Matrix4x4.TRS (new Vector3 (0.5f, 0.5f, 0f), Quaternion.identity, new Vector3 (0.5f, 0.5f, 1f));

	
		/// <summary>
		/// Returns a Matrix4x4 that will convert a projection matrix from [-1,1] space into rect space
		/// </summary>
		public static Matrix4x4 ToRectSpaceProjection (Rect rect)
		{
				return Matrix4x4.TRS (new Vector3 (rect.width * 0.5f + rect.x, rect.height * 0.5f + rect.y, 0f), Quaternion.identity, new Vector3 (rect.width * 0.5f, rect.height * 0.5f, 1f));
		}

		/// <summary>
		/// Modifies a projection matrix to preserve linear depth in [0,1] space
		/// </summary>
		/// <param name='projection'>
		/// Projection matrix.
		/// </param>
		/// <param name='farClip'>
		/// Far clip.
		/// </param>
		public static void SetLinearDepthProjection (ref Matrix4x4 projection, float farClip)
		{
				projection.SetRow (2, new Vector4 (0f, 0f, -1f / farClip, 0f));
		}

		/// <summary>
		/// Returns a projection matrix that preserves linear depth in [0,1] space
		/// </summary>
		/// <param name='projection'>
		/// Projection matrix.
		/// </param>
		/// <param name='farClip'>
		/// Far clip.
		/// </param>
		public static Matrix4x4 SetLinearDepthProjection (Matrix4x4 projection, float farClip)
		{
				SetLinearDepthProjection (ref projection, farClip);
				return projection;
		}

		/// <summary>
		/// All possible cascade viewport arrangements...
		/// </summary>
		public static readonly Rect[][] CascadeViewportArrangements = new Rect[][] {
				new Rect[] {	// 1
						new Rect (0f, 0f, 1f, 1f)
				},
				new Rect[] {	// 2 Stacking Vertically is better for PredicatedTiling
						new Rect (0f, 0f, 1f, 0.5f),
						new Rect (0f, 0.5f, 1f, 0.5f),
				},
				/*
		new Rect[]	// 2 (Wide Version, Worse for Resolution, Better for PredicatedTiling)
		{
			new Rect(0f, 0f, 1f, 0.5f),
			new Rect(0f, 0.5f, 1f, 0.5f),
		},
		//*/
				new Rect[] {	// 3 (Tall Version, Better for Resolution, Worse for PredicatedTiling)
						new Rect (0f, 0f, 0.5f, 1f),
						new Rect (0.5f, 0f, 0.5f, 0.5f),
						new Rect (0.5f, 0.5f, 0.5f, 0.5f),
				},
				/*
		new Rect[]	// 3 (Wide Version, Worse for Resolution, Better for PredicatedTiling)
		{
			new Rect(0f, 0f, 0.5f, 1f),
			new Rect(0.5f, 0f, 0.5f, 0.5f),
			new Rect(0.5f, 0.5f, 0.5f, 0.5f),
		},
		//*/
				new Rect[] {	// 4
						new Rect (0f, 0f, 0.5f, 0.5f),
						new Rect (0f, 0.5f, 0.5f, 0.5f),
						new Rect (0.5f, 0f, 0.5f, 0.5f),
						new Rect (0.5f, 0.5f, 0.5f, 0.5f),
				},
		};

		/// <summary>
		/// Shader-style swizzling.  Same as float.xy
		/// </summary>
		public static Vector2 xy (Vector3 self)
		{
				return new Vector2 (self.x, self.y);
		}

		/// <summary>
		/// Shader-style swizzling.  Same as float.xy
		/// </summary>
		public static Vector2 xy (Vector4 self)
		{
				return new Vector2 (self.x, self.y);
		}

		/// <summary>
		/// Shader-style swizzling.  Same as float.xyz
		/// </summary>
		public static Vector3 xyz (Vector4 self)
		{
				return new Vector3 (self.x, self.y, self.z);
		}

		/// <summary>
		/// Same as new Vector3(x, y, 0)
		/// </summary>
		public static Vector3 xy0 (Vector2 self)
		{
				return new Vector3 (self.x, self.y, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, 0)
		/// </summary>
		public static Vector3 xy0 (Vector3 self)
		{
				return new Vector3 (self.x, self.y, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, 0)
		/// </summary>
		public static Vector3 xy0 (Vector4 self)
		{
				return new Vector3 (self.x, self.y, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, 0, 0)
		/// </summary>
		public static Vector4 xy00 (Vector2 self)
		{
				return new Vector4 (self.x, self.y, 0f, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, 0, 0)
		/// </summary>
		public static Vector4 xy00 (Vector3 self)
		{
				return new Vector4 (self.x, self.y, 0f, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, 0, 0)
		/// </summary>
		public static Vector4 xy00 (Vector4 self)
		{
				return new Vector4 (self.x, self.y, 0f, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, z, 0)
		/// </summary>
		public static Vector4 xyz0 (Vector3 self)
		{
				return new Vector4 (self.x, self.y, self.z, 0f);
		}

		/// <summary>
		/// Same as new Vector3(x, y, z, 0)
		/// </summary>
		public static Vector4 xyz0 (Vector4 self)
		{
				return new Vector4 (self.x, self.y, self.z, 0f);
		}

		/// <summary>
		/// Gets the integer divisor for a relative resolution (ie. Half = 2, Third = 3)
		/// </summary>
		/// <returns>
		/// The resolution divisor.
		/// </returns>
		/// <param name='resolution'>
		/// Relative resolution.
		/// </param>
		public static int RelativeResolutionDivisor (SunshineRelativeResolutions resolution)
		{
				switch (resolution) {
				case SunshineRelativeResolutions.Full:
						return 1;
				case SunshineRelativeResolutions.Half:
						return 2;
				case SunshineRelativeResolutions.Third:
						return 3;
				case SunshineRelativeResolutions.Quarter:
						return 4;
				case SunshineRelativeResolutions.Fifth:
						return 5;
				case SunshineRelativeResolutions.Sixth:
						return 6;
				case SunshineRelativeResolutions.Seventh:
						return 7;
				case SunshineRelativeResolutions.Eighth:
						return 8;
				}
				return 1;
		}

		/// <summary>
		/// Calculates the diagonal radius of a shadow filter kernel.
		/// </summary>
		/// <returns>
		/// The kernel radius.
		/// </returns>
		/// <param name='filter'>
		/// Filter.
		/// </param>
		public static float ShadowKernelRadius (SunshineShadowFilters filter)
		{
				// Sqrt(size^2 + size^2) / 2 
				switch (filter) {
				case SunshineShadowFilters.PCF2x2:
						return 1.414214f;
				case SunshineShadowFilters.PCF3x3:
						return 2.12132f;
				case SunshineShadowFilters.PCF4x4:
						return 2.828427f;
				//case ShadowFilters.PCF5x5:		return 3.535534f;
				//case ShadowFilters.PCF6x6:		return 4.24264f;
				//case ShadowFilters.PCF7x7:		return 4.949748f;
				//case ShadowFilters.PCF8x8:		return 5.656854f;
				//case ShadowFilters.PCF9x9:		return 6.363961f;
				case SunshineShadowFilters.Hard:
				default:
						return 0.7071068f;
				}
		}

		/// <summary>
		/// Quantizes a value.
		/// </summary>
		/// <returns>
		/// The quantized value.
		/// </returns>
		/// <param name='number'>
		/// Number to quantize
		/// </param>
		/// <param name='step'>
		/// Step size to quantize to.
		/// </param>
		public static float QuantizeValue (float number, float step)
		{
				return Mathf.Floor (number / step + 0.5f) * step;
		}

		/// <summary>
		/// Quantizes a value, avoiding flicker due to floating point error.
		/// </summary>
		/// <returns>
		/// The quantized value.
		/// </returns>
		/// <param name='number'>
		/// Number to quantize
		/// </param>
		/// <param name='step'>
		/// Step size to quantize to.
		/// </param>
		/// <param name='lastResult'>
		/// The last result this method returned
		/// </param>
		public static float QuantizeValueWithoutFlicker (float number, float step, float lastResult)
		{
				float result = SunshineMath.QuantizeValue (number, step);

				//If the new result is significantly better than the last result, return the new result
				if (Mathf.Abs (result - number) * 4f < Mathf.Abs (lastResult - number))
						return result;
		
				return lastResult;
		}

		/// <summary>
		/// Quantizes a value.
		/// </summary>
		/// <returns>
		/// The quantized value.
		/// </returns>
		/// <param name='number'>
		/// Number to quantize
		/// </param>
		/// <param name='resolution'>
		/// The number of steps per unit.
		/// </param>		
		public static float QuantizeValue (float number, int resolution)
		{
				return QuantizeValue (number, 1f / (float)resolution);
		}

		/// <summary>
		/// Quantizes a value.
		/// </summary>
		/// <returns>
		/// The quantized value.
		/// </returns>
		/// <param name='number'>
		/// Number to quantize
		/// </param>
		/// <param name='resolution'>
		/// The number of steps per unit.
		/// </param>		
		/// <param name='lastResult'>
		/// The last result this method returned
		/// </param>
		public static float QuantizeValueWithoutFlicker (float number, int resolution, float lastResult)
		{
				return QuantizeValueWithoutFlicker (number, 1f / (float)resolution, lastResult);
		}

		/// <summary>
		/// Calculates the ratio between far clip at the corner of the view frustum, and at the center, assuming a radial clipping plane.
		/// </summary>
		/// <returns>
		/// The corner far clip ratio
		/// </returns>
		/// <param name='cam'>
		/// A camera.
		/// </param>
		public static float RadialClipCornerRatio (Camera cam)
		{
				//*
				var cornerRay = cam.ViewportPointToRay (new Vector3 (0f, 0f, 0f));
				var localForward = cam.transform.InverseTransformDirection (cornerRay.direction);
				return localForward.z;
				//*/
		
				//The following should be better, since it's perfectly stable!
				//But... the math just isn't cooperating! >:/
				/*
		float vFOV = cam.fieldOfView;
		float radVFOV = vFOV * Mathf.Deg2Rad;
		float radHFOV = 2f * Mathf.Atan(Mathf.Tan(radVFOV / 2f) * cam.aspect);
		float hFOV = Mathf.Rad2Deg * radHFOV;
		return (Quaternion.Euler(vFOV * 0.5f, hFOV * -0.5f, 0f) * Vector3.forward).z;
		//*/
		
		}

		/// <summary>
		/// A simple bounding sphere struct.
		/// </summary>
		public struct BoundingSphere
		{
				public Vector3 origin;
				public float radius;
		}

		/// <summary>
		/// Finds the minimum radius^2 required to encompass all points[] from the origin.
		/// </summary>
		/// <returns>
		/// The radius^2.
		/// </returns>
		/// <param name='origin'>
		/// Point of origin.
		/// </param>
		/// <param name='points'>
		/// Points to test.
		/// </param>
		public static float MinRadiusSq (Vector3 origin, Vector3[] points)
		{
				float maxRadiusSq = 0f;
				for (int i = 0; i < points.Length; i++) {
						float radSq = (points [i] - origin).sqrMagnitude;
						if (radSq > maxRadiusSq)
								maxRadiusSq = radSq;
				}
				return maxRadiusSq;
		}

		private static Vector3[] _frustumTestPoints = new Vector3[2];

		/// <summary>
		/// Calculates the minimum bounding sphere for a section of the view frustum.
		/// </summary>
		/// <returns>
		/// The minimum bounding sphere.
		/// </returns>
		/// <param name='camera'>
		/// Camera.
		/// </param>
		/// <param name='nearClip'>
		/// Near clip of the section.
		/// </param>
		/// <param name='farClip'>
		/// Far clip of the section.
		/// </param>
		/// <param name='radial'>
		/// True if the section is radial.
		/// </param>
		/// <param name='maxError'>
		/// Max error allowed.
		/// </param>
		/// <param name='maxSteps'>
		/// Max iterations allowed.
		/// </param>
		public static BoundingSphere FrustumBoundingSphereBinarySearch (Camera camera, float nearClip, float farClip, bool radial, float radialPadding, float maxError = 0.01f, int maxSteps = 100)
		{
				//NearClip must be shrunk, since the outter part of a circle is closer on Z...
				//FarClip does not, since it bulges outward in the middle
				float radialCornerRatio = RadialClipCornerRatio (camera);
		
				float eyeRadialNearClip = radial ? nearClip * radialCornerRatio : nearClip;
				float eyeRadialFarClip = radial ? farClip * radialCornerRatio : farClip;

				Vector3 worldEyeNear = camera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, nearClip));
				Vector3 worldEyeRadialFar = camera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, eyeRadialFarClip));
				Vector3 worldEyeRadialNear00 = camera.ViewportToWorldPoint (new Vector3 (0f, 0f, eyeRadialNearClip));
				Vector3 worldEyeFar11 = camera.ViewportToWorldPoint (new Vector3 (1f, 1f, farClip));
				Vector3 worldEyeRadialFar11 = radial ? camera.ViewportToWorldPoint (new Vector3 (1f, 1f, eyeRadialFarClip)) : worldEyeFar11;
		
				//No need to test all corners!  These are the same distance from the center ray...
				_frustumTestPoints [0] = worldEyeRadialNear00;
				_frustumTestPoints [1] = worldEyeRadialFar11;
		
				//In tests, the bulged out center is NEVER a limiting point... Save the calculations :)
				//_frustumTestPoints[2] = worldEyeFar;

				float minRadSq = float.MaxValue;
				Vector3 minOrigin = Vector3.zero;
		
				float searchPosition = 0f;
				float searchStep = 0.2f;
				for (int i = 0; i < maxSteps; i++) {
						Vector3 origin = Vector3.Lerp (worldEyeNear, worldEyeRadialFar, searchPosition);
						float radSq = MinRadiusSq (origin, _frustumTestPoints);
						if (radSq < minRadSq) {
								minRadSq = radSq;
								minOrigin = origin;
						} else {
								searchStep = searchStep * -0.5f;
								if (Mathf.Abs (searchStep) < maxError)
										break;
						}
						searchPosition += searchStep;
				}

				return new BoundingSphere () { origin = minOrigin, radius = Mathf.Sqrt (minRadSq) + radialPadding };
		}

		public struct ShadowCameraTemporalData
		{
				public float boundingRadius;
				public Vector3 lightWorldOrigin;
		}

		/// <summary>
		/// Prepares a shadow cascade camera.
		/// </summary>
		/// <param name='light'>
		/// Shadow casting light.
		/// </param>
		/// <param name='lightCamera'>
		/// Light camera.
		/// </param>
		/// <param name='eyeCamera'>
		/// Eye camera.
		/// </param>
		/// <param name='eyeNearClip'>
		/// Eye near clip for this cascade.
		/// </param>
		/// <param name='eyeFarClip'>
		/// Eye far clip for this cascade.
		/// </param>
		/// <param name='snapResolution'>
		/// Cascade resolution for quantizing.
		/// </param>
		/// <param name='totalShadowBounds'>
		/// Bounding sphere for the entire Lightmap.  Used for positioning the camera on Z.
		/// </param>
		public static void SetupShadowCamera (Light light, Camera lightCamera, Camera eyeCamera, float eyeNearClip, float eyeFarClip, float paddingZ, float paddingRadius, int snapResolution, ref BoundingSphere totalShadowBounds, ref ShadowCameraTemporalData temporalData)
		{
				var lightCameraTransform = lightCamera.transform;
		
				BoundingSphere bounds = new BoundingSphere ();
				if (Sunshine.Instance.UsingCustomBounds)
						bounds = Sunshine.Instance.CustomBounds;
				else
						bounds = FrustumBoundingSphereBinarySearch (eyeCamera, eyeNearClip, eyeFarClip, true, paddingRadius, 0.01f, 20);
		
				float maxRadius = QuantizeValueWithoutFlicker (bounds.radius, 100, temporalData.boundingRadius);
				temporalData.boundingRadius = maxRadius;
				float maxDimension2D = maxRadius * 2f;

				//Set Camera Properties
				lightCamera.aspect = 1.0f;
				lightCamera.orthographic = true;
				lightCamera.nearClipPlane = eyeCamera.nearClipPlane;
				lightCamera.farClipPlane = (totalShadowBounds.radius + paddingZ + lightCamera.nearClipPlane) * 2f;
				lightCamera.orthographicSize = maxDimension2D * 0.5f;

				lightCameraTransform.rotation = Quaternion.LookRotation (light.transform.forward);
		
				lightCameraTransform.position = bounds.origin;
		
				//Quantize:
				Vector3 lightWorldOrigin = lightCameraTransform.InverseTransformPoint (Vector3.zero);
	
				float camUnits = maxDimension2D / (float)snapResolution;
				lightWorldOrigin.x = QuantizeValueWithoutFlicker (lightWorldOrigin.x, camUnits, temporalData.lightWorldOrigin.x);
				lightWorldOrigin.y = QuantizeValueWithoutFlicker (lightWorldOrigin.y, camUnits, temporalData.lightWorldOrigin.y);
				//lightWorldOrigin.z = QuantizeValueWithoutFlicker(lightWorldOrigin.z, camUnits, temporalData.lightWorldOrigin.z);
				temporalData.lightWorldOrigin = lightWorldOrigin;
		
				lightCameraTransform.position -= lightCameraTransform.TransformPoint (lightWorldOrigin);

				// Make the lightCamera's Z Position uniform between all Cascades!
				// This may not be required in the future, when we impliment staggered cascade rendering...
				Vector3 shadowCenterInLight = lightCameraTransform.InverseTransformPoint (totalShadowBounds.origin);
				lightCameraTransform.position += lightCameraTransform.forward * (shadowCenterInLight.z - (totalShadowBounds.radius + lightCamera.nearClipPlane + paddingZ));
		}

		/// <summary>
		/// Remaps the shadow coordinate from a ShadowCoordData vector.
		/// </summary>
		/// <param name='shadowCoordData'>
		/// Shadow coordinate data.
		/// </param>
		/// <param name='rect'>
		/// Rect to remap to.
		/// </param>
		public static void ShadowCoordDataInRect (ref Vector4 shadowCoordData, ref Rect rect)
		{
				shadowCoordData.x = Mathf.Lerp (rect.xMin, rect.xMax, shadowCoordData.x);
				shadowCoordData.y = Mathf.Lerp (rect.yMin, rect.yMax, shadowCoordData.y);
		}

		/// <summary>
		/// Remaps the shadow coordinate from a ShadowCoordDataRay vector.
		/// </summary>
		/// <param name='shadowCoordDataRay'>
		/// Shadow coordinate data.
		/// </param>
		/// <param name='rect'>
		/// Rect to remap to.
		/// </param>
		public static void ShadowCoordDataRayInRect (ref Vector4 shadowCoordDataRay, ref Rect rect)
		{
				shadowCoordDataRay.x *= rect.width;
				shadowCoordDataRay.y *= rect.height;
		}

		public static LayerMask SubtractMask (LayerMask mask, LayerMask subtract)
		{
				return mask & (~subtract);
		}
}

/// <summary>
/// An integer version of Vector2
/// </summary>
public struct int2
{
		public int x;
		public int y;

		public int2 (int x, int y)
		{
				this.x = x;
				this.y = y;
		}

		public static int2 operator+ (int2 a, int2 b)
		{
				return new int2 (a.x + b.x, a.y + b.y);
		}

		public static int2 operator- (int2 a, int2 b)
		{
				return new int2 (a.x - b.x, a.y - b.y);
		}

		public static int2 operator* (int2 a, int2 b)
		{
				return new int2 (a.x * b.x, a.y * b.y);
		}

		public static int2 operator/ (int2 a, int2 b)
		{
				return new int2 (a.x / b.x, a.y / b.y);
		}

		public static int2 operator+ (int2 a, int b)
		{
				return new int2 (a.x + b, a.y + b);
		}

		public static int2 operator- (int2 a, int b)
		{
				return new int2 (a.x - b, a.y - b);
		}

		public static int2 operator* (int2 a, int b)
		{
				return new int2 (a.x * b, a.y * b);
		}

		public static int2 operator/ (int2 a, int b)
		{
				return new int2 (a.x / b, a.y / b);
		}
}
