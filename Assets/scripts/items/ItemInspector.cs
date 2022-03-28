using UnityEngine;
using UnityEngine.UI;
using Polyworks;

public class ItemInspector : MonoBehaviour, IInputControllable
{
    #region public members
    public static int INSPECTOR_LAYER = 15;

    public Camera MainCamera;
    public string[] InspectionLayers;
    public float Distance = 2.0f;
    public float DistanceMin = .5f;
    public float DistanceMax = 15f;
    public float XSpeed = 150.0f;
    public float YSpeed = 150.0f;
    public float RotationMultiplier = 0.01f;
    public float YMinLimit = -361f;
    public float YMaxLimit = 361f;
    public float SmoothTime = 2f;
    public float ZoomAmount = 15f;
    public float MaxZoom = 4f;
    public float MinZoom = -4f;
    public Text ItemName;
    public Text ItemDescription;
    #endregion

    #region private members
    private EventCenter eventCenter;
    private int originalCullingMask;

    private float horizontal = 0;
    private float vertical = 0;

    private bool cancel = false;
    private bool zoomIn = false;
    private bool zoomOut = false;

    private float rotationYAxis = 0.0f;
    private float rotationXAxis = 0.0f;

    private float velocityX = 0.0f;
    private float velocityY = 0.0f;

    private Transform item;
    private Transform previousParent;
    private Vector3 previousPosition;
    private int previousLayer;
    private float initialFieldOfView;
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    private int currentZoom;
    #endregion

    private static ItemInspector instance;

    private ItemInspector() { }

    public static ItemInspector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(ItemInspector)) as ItemInspector;
            }
            return instance;
        }
    }

    public void OnInspectItem(bool isInspecting, string name)
    {
        if (!isInspecting)
        {
            removeTargetAndReset();
            return;
        }
        CollectableItem item = Game.Instance.GetPlayerInventory().GetItem(name);

        if (item == null)
        {
            return;
        }
        item.transform.rotation = transform.rotation;
        AddTarget(item.transform, item.data.displayName, item.data.description);
    }

    public void SetInput(InputObject input)
    {

    }

    public void SetHorizontal(float horizontal)
    {
        this.horizontal = horizontal;
    }

    public void SetVertical(float vertical)
    {
        this.vertical = vertical;
    }

    public void SetZoomIn(bool zoomIn)
    {
        this.zoomIn = zoomIn;
    }

    public void SetZoomOut(bool zoomOut)
    {
        this.zoomOut = zoomOut;
    }

    public void SetCancel(bool cancel)
    {
        this.cancel = cancel;
    }

    public void AddTarget(Transform item, string name, string description)
    {
        this.item = item;

        Utilities.Instance.ChangeLayers(item.gameObject, INSPECTOR_LAYER);

        Vector3 position = new Vector3(transform.position.x + Distance, transform.position.y, transform.position.z);
        item.transform.position = position;
        ItemName.text = name;
        ItemDescription.text = description;

        MainCamera.cullingMask = LayerMask.GetMask(InspectionLayers);

        ItemInspectionScale[] entries = Game.Instance.GetItemInspectionScales();
        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].name == item.name)
            {
                Vector3 itemScale = new Vector3(entries[i].scale.x, entries[i].scale.y, entries[i].scale.z);
                item.transform.localScale = itemScale;
            }
        }
    }

    private void removeTargetAndReset()
    {
        cancel = false;

        item.parent = previousParent;
        item.position = previousPosition;

        Utilities.Instance.ChangeLayers(item.gameObject, previousLayer);

        item = null;

        MainCamera.fieldOfView = initialFieldOfView;
        MainCamera.cullingMask = originalCullingMask;
        currentZoom = 0;

        ItemName.text = "";
        ItemDescription.text = "";

        rotationYAxis = 0.0f;
        rotationXAxis = 0.0f;

        velocityX = 0.0f;
        velocityY = 0.0f;

        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    void Awake()
    {
        if (MainCamera == null)
        {
            return;
        }

        originalCullingMask = MainCamera.cullingMask;
        initialFieldOfView = MainCamera.fieldOfView;
        initialRotation = transform.rotation;
        initialPosition = transform.position;

        ItemName.text = "";
        ItemDescription.text = "";

        eventCenter = EventCenter.Instance;
        eventCenter.OnInspectItem += OnInspectItem;
    }

    void LateUpdate()
    {
        // based on: http://answers.unity3d.com/questions/463704/smooth-orbit-round-object-with-adjustable-orbit-ra.html
        if (item == null)
        {
            return;
        }

        // Debug.Log("LateUpdate, horizontal = " + horizontal + ", vertical = " + vertical);
        if (cancel)
        {
            eventCenter.InspectItem(false, item.name);
            return;
        }

        if (zoomIn)
        {
            zoomIn = false;
            // Debug.Log("zoomIn, currentZoom = " + currentZoom + ", MaxZoom = " + MaxZoom);
            if (currentZoom >= MaxZoom)
            {
                return;
            }
            MainCamera.fieldOfView += ZoomAmount;
            currentZoom++;
            return;
        }

        if (zoomOut)
        {
            zoomOut = false;
            // Debug.Log("zoomOut, currentZoom = " + currentZoom + ", MinZoom = " + MinZoom);
            if (currentZoom <= MinZoom)
            {
                return;
            }
            MainCamera.fieldOfView -= ZoomAmount;
            currentZoom--;
            return;
        }

        velocityX = XSpeed * horizontal * RotationMultiplier;
        velocityY = YSpeed * vertical * RotationMultiplier;

        rotationYAxis += velocityX;
        rotationXAxis -= velocityY;
        rotationXAxis = Polyworks.Utils.ClampAngle(rotationXAxis, YMinLimit, YMaxLimit);

        Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
        Quaternion rotation = toRotation;

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -Distance);
        Vector3 position = rotation * negDistance + item.position;

        transform.rotation = rotation;
        transform.position = position;

        velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * SmoothTime);
        velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * SmoothTime);
    }

    private void OnDestroy()
    {
        if (eventCenter == null)
        {
            return;
        }
        eventCenter.OnInspectItem -= OnInspectItem;
    }
}
