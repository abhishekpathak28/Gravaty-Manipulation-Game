using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] private float rayLenght = 1.3f;
    [SerializeField] LayerMask groundLayerMask;
    public bool isGrounded { get; private set; }
    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position + transform.up * 0.01f, -transform.up, rayLenght, groundLayerMask);
        Debug.DrawRay(transform.position + transform.up * 0.01f, -transform.up * rayLenght, isGrounded ? Color.green : Color.red);
    }
}
