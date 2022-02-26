using UnityEngine;

public class FogManager : MonoBehaviour
{


    public Color _normalColor;
    public float _normalStartDistance;

    public Color _underwaterColor;
    public float _underwaterStartDistance = 2f;

    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        OnUnderWater(false);
    }

    public void OnUnderWater(bool under)
    {
        if (under)
        {
            RenderSettings.fogColor = _underwaterColor;
            return;
        }
        RenderSettings.fogColor = _normalColor;
    }
}
