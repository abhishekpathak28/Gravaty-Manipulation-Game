using UnityEngine;

public class GravityController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravityMagnitude = 15f;
    [SerializeField] private float rotationSpeed = 5f;
    [Header("References")]
    [SerializeField] private Transform hologramAchor;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject hologramObject;
    [SerializeField] private PlayerGroundCheck groundCheckScript;
    private Vector3 offset;

    public Vector3 CurrentGravityDir { get; private set; } = Vector3.down;
    private Vector3 proposedGravityDir;
    private bool isGravityChangePending;
    private Rigidbody rb;

    private float currentfallTimer = 0f;
    private float maxFallTime = 3f;
    private bool isInputActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        if (hologramObject) hologramObject.SetActive(false);
        if (groundCheckScript == null) Debug.LogError("GroundCheckScript missing");
    }
    void Update()
    {
        if (!isInputActive) return;
        HandleGravityInput();
        CheckFallStatus();
    }
    void FixedUpdate()
    {
        if (!isInputActive) return;
        ApplyGravity();
        AlignPlayerToGravity();
    }
    void HandleGravityInput()
    {
        Vector3 inputDir = Vector3.zero;
        Vector3 playerUp = -CurrentGravityDir;
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, playerUp).normalized;

        Vector3 camRight = Vector3.ProjectOnPlane(cameraTransform.right, playerUp).normalized;

        if (Input.GetKeyDown(KeyCode.UpArrow)) inputDir = camForward;

        else if (Input.GetKeyDown(KeyCode.DownArrow)) inputDir = -camForward;

        else if (Input.GetKeyDown(KeyCode.RightArrow)) inputDir = camRight;

        else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputDir = -camRight;


        if (inputDir != Vector3.zero)
        {
            // Vector3 playerRelDir = transform.TransformDirection(inputDir);
            proposedGravityDir = SnapToCardinalDirection(inputDir);
            ShowHologram(proposedGravityDir);
            isGravityChangePending = true;
        }
        if (isGravityChangePending && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGravity(proposedGravityDir);
            ShowHologram(proposedGravityDir);
            if (hologramObject != null) hologramObject.SetActive(false);
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
        hologramObject.transform.SetParent(hologramAchor);
        hologramObject.transform.localPosition = offset;
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
    void CheckFallStatus()
    {
        if (groundCheckScript == null) return;
        if (groundCheckScript.isGrounded)
        {
            currentfallTimer = 0f;
        }
        else
        {
            currentfallTimer += Time.deltaTime;
        }
        if (currentfallTimer > maxFallTime)
        {

            isInputActive = false;
            GameManager.instance.GameOver();
        }
    }
}
