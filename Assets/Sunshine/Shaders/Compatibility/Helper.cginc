#undef UnityMajorVersion

#if defined(UNITY_PI) || defined(UNITY_FOG_LERP_COLOR)
	#define UnityMajorVersion 5
#elif defined(CBUFFER_START)
	#define UnityMajorVersion 4
#else
	#define UnityMajorVersion 3
#endif
