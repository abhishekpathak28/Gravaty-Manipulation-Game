using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravityMagnitude = 15f;
    [SerializeField] private float rotationSpeed = 5f;
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject hologramObject;

    public Vector3 CurrentGravityDir { get; private set; } = Vector3.down;
    private Vector3 proposedGravityDir;
    private bool isGravityChangePending;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        if (hologramObject) hologramObject.SetActive(false);
        // Physics.gravity = CurrentGravityDir * gravityMagnitude;
    }
    void Update()
    {
        HandleGravityInput();
    }
    void FixedUpdate()
    {
        ApplyGravity();
        AlignPlayerToGravity();
    }
    void HandleGravityInput()
    {
        Vector3 inputDir = Vector3.zero;
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        if (Input.GetKeyDown(KeyCode.UpArrow)) inputDir = camForward;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) inputDir = -camForward;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) inputDir = camRight;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputDir = -camRight;


        if (inputDir != Vector3.zero)
        {
            proposedGravityDir = SnapToCardinalDirection(inputDir);
            ShowHologram(proposedGravityDir);
            isGravityChangePending = true;
        }
        if (isGravityChangePending && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGravity(proposedGravityDir);
            ShowHologram(inputDir);
            if (hologramObject) hologramObject.SetActive(false);
        }

    }
    void ChangeGravity(Vector3 newDir)
    {
        CurrentGravityDir = newDir.normalized;
    }
    void ApplyGravity()
    {
        rb.AddForce(CurrentGravityDir * gravityMagnitude, ForceMode.Acceleration);
    }
    void AlignPlayerToGravity()
    {
        Vector3 targetUp = -CurrentGravityDir;
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, targetUp) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
    }
    void ShowHologram(Vector3 gravityDir)
    {
        if (hologramObject == null) return;
        hologramObject.SetActive(true);
        Quaternion targetRot = Quaternion.FromToRotation(Vector3.down, gravityDir);
        hologramObject.transform.rotation = targetRot;
    }
    Vector3 SnapToCardinalDirection(Vector3 input)
    {
        float x = Mathf.Abs(input.x);
        float y = Mathf.Abs(input.y);
        float z = Mathf.Abs(input.z);

        if (x > y && x > z) return new Vector3(Mathf.Sign(input.x), 0, 0);
        if (y > x && y > z) return new Vector3(0, Mathf.Sign(input.y), 0);
        return new Vector3(0, 0, Mathf.Sign(input.z));
    }
}
