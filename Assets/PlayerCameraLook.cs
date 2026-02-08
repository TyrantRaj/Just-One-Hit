using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraLook : MonoBehaviour
{
    public float mouseSensitivity = 120f;
    public float stickSensitivity = 220f;

    public float minPitch = -40f;
    public float maxPitch = 65f;

    Vector2 lookInput;
    float yaw;
    float pitch;

    Transform player;

    void Awake()
    {
        player = transform.root;
        Vector3 euler = transform.eulerAngles;
        yaw = euler.y;
        pitch = euler.x;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        if (lookInput == Vector2.zero) return;

        bool usingMouse = Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero;

        float sens = usingMouse ? mouseSensitivity : stickSensitivity;

        yaw += lookInput.x * sens * Time.deltaTime;
        pitch -= lookInput.y * sens * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
        player.rotation = Quaternion.Euler(0, yaw, 0);
    }
}
