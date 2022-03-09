using UnityEngine;
using Polyworks;

public class CameraZoom : MonoBehaviour
{

    public float Zoom = 10;
    public GameObject Cursor;

    private Camera target;
    private float normal;
    private bool isZoomed = false;

    public void Execute()
    {
        zoomCamera();
    }

    public void OnMainCameraEnabled()
    {
        target = Camera.main;
        normal = target.fieldOfView;
    }

    private void Awake()
    {
        EventCenter.Instance.OnMainCameraEnabled += OnMainCameraEnabled;
    }

    private void zoomCamera()
    {
        target.fieldOfView = (isZoomed) ? normal : Zoom;
        isZoomed = !isZoomed;
        Cursor.SetActive(!isZoomed);
    }

    private void OnDestroy()
    {
        EventCenter ec = EventCenter.Instance;
        if (ec != null)
        {
            ec.OnMainCameraEnabled -= OnMainCameraEnabled;
        }
    }
}
