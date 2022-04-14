using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using Polyworks;

public class ItemInspector : UIController
{
    #region public members
    public static int INSPECTOR_LAYER = 15;

    public Camera MainCamera;
    public GameObject InspectionLight;
    public GameObject InspectionUI;
    public string[] InspectionLayers;
    public float Distance = 1f;
    public float DistanceMin = 0.5f;
    public float DistanceMax = 15f;
    public float XSpeed = 150.0f;
    public float YSpeed = 150.0f;
    public float RotationMultiplier = 4f;
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
    private int originalCullingMask;
    private HDAdditionalCameraData.ClearColorMode originalColorMode;

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

    #region singleton
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
    #endregion

    #region public methods
    public void OnInspectItem(bool isInspecting, string name)
    {
        if (!isInspecting)
        {
            return;
        }

        CollectableItem item = Game.Instance.GetPlayerInventory().GetItem(name);

        if (item == null)
        {
            return;
        }
        addTarget(item.transform, item.data.displayName, item.data.description);

        eventCenter.SetActiveInputTarget(InputController.SET_ACTIVE_INPUT_TARGET, this);
    }

    #endregion

    #region private methods
    private void addTarget(Transform target, string name, string description)
    {
        item = target;

        Utilities.Instance.ChangeLayers(item.gameObject, INSPECTOR_LAYER);

        Vector3 position = new Vector3(transform.position.x + Distance, transform.position.y, transform.position.z);
        item.position = position;
        // item.rotation = transform.rotation;
        // item.position = Vector3.zero;
        // item.SetParent(transform);

        InspectionUI.SetActive(true);

        ItemName.text = name;
        ItemDescription.text = description;

        MainCamera.cullingMask = LayerMask.GetMask(InspectionLayers);
        MainCamera.GetComponent<HDAdditionalCameraData>().clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
        MainCamera.transform.LookAt(item);

        InspectionLight.SetActive(true);
        InspectionLight.gameObject.transform.position = new Vector3(position.x, position.y + 1, position.z);

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

    private void setZoom()
    {
        if (isZoomIn)
        {
            isZoomIn = false;

            if (currentZoom >= MaxZoom)
            {
                return;
            }
            MainCamera.fieldOfView += ZoomAmount;
            currentZoom++;
            return;
        }

        if (!isZoomOut)
        {
            return;
        }

        isZoomOut = false;

        if (currentZoom <= MinZoom)
        {
            return;
        }
        MainCamera.fieldOfView -= ZoomAmount;
        currentZoom--;
    }

    private void removeTargetAndReset()
    {
        cancel = false;

        InspectionUI.SetActive(false);
        InspectionLight.SetActive(false);

        item.parent = previousParent;
        item.position = previousPosition;

        Utilities.Instance.ChangeLayers(item.gameObject, previousLayer);

        Destroy(item.gameObject);

        MainCamera.fieldOfView = initialFieldOfView;
        MainCamera.cullingMask = originalCullingMask;
        MainCamera.GetComponent<HDAdditionalCameraData>().clearColorMode = originalColorMode;

        currentZoom = 0;

        ItemName.text = "";
        ItemDescription.text = "";

        rotationYAxis = 0.0f;
        rotationXAxis = 0.0f;

        velocityX = 0.0f;
        velocityY = 0.0f;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        eventCenter.CloseItemInspector();
    }
    #endregion

    #region unity methods
    private void Awake()
    {
        if (MainCamera == null)
        {
            return;
        }

        InspectionUI.SetActive(false);
        InspectionLight.SetActive(false);

        originalCullingMask = MainCamera.cullingMask;
        initialFieldOfView = MainCamera.fieldOfView;
        originalColorMode = MainCamera.GetComponent<HDAdditionalCameraData>().clearColorMode;

        MainCamera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = Color.black;

        initialRotation = transform.rotation;
        initialPosition = transform.position;

        ItemName.text = "";
        ItemDescription.text = "";

        eventCenter = EventCenter.Instance;
        eventCenter.OnInspectItem += OnInspectItem;
    }

    private void LateUpdate()
    {
        // based on: http://answers.unity3d.com/questions/463704/smooth-orbit-round-object-with-adjustable-orbit-ra.html
        if (item == null)
        {
            return;
        }

        if (cancel)
        {
            cancel = false;
            eventCenter.InspectItem(false, item.name);
            return;
        }

        setZoom();

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
    #endregion
}
