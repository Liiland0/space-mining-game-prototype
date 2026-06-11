using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlrMovementController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource thrusterAudioSource;
    [SerializeField] private AudioClip thrusterSound;

    [Header("Movement Settings")]
    [SerializeField] private float steeringPower = 100f;
    [SerializeField] private float thrustPower = 10f;
    [SerializeField] private float maxSpeed = 5f;

    private float thrust;
    private bool wasThrusting;
    private Vector2 mousePos;

    private VisualEffect thrustEffect;
    private Rigidbody2D rb;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        thrustEffect = GetComponentInChildren<VisualEffect>();
        thrustEffect.Stop();

        thrusterAudioSource.clip = thrusterSound;
        thrusterAudioSource.loop = true;

        cam = Camera.main;
    }

    private float targetAngle;

    void Update()
    {
        Vector3 worldMouse = cam.ScreenToWorldPoint(mousePos);
        worldMouse.z = 0f;

        Vector2 direction = (worldMouse - transform.position).normalized;
        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    void FixedUpdate()
    {
        float newAngle = Mathf.LerpAngle(rb.rotation, targetAngle, steeringPower * Time.fixedDeltaTime);
        rb.MoveRotation(newAngle);

        bool isThrusting = thrust != 0;

        if (isThrusting)
        {
            rb.AddForce(transform.up * thrust * thrustPower);
        }

        if (isThrusting && !wasThrusting)
        {
            thrustEffect.Play();
        }
        else if (!isThrusting && wasThrusting)
        {
            thrustEffect.Stop();
        }

        wasThrusting = isThrusting;

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Station")
        {
            Debug.Log("Docked at station");
            other.GetComponent<SpaceStation>().CheckQuests();
        }
    }

    public void OnThrust(InputAction.CallbackContext context)
    {
        thrust = context.ReadValue<float>();

        if (thrust != 0 && !thrusterAudioSource.isPlaying)
            thrusterAudioSource.Play();
        else if (thrust == 0 && thrusterAudioSource.isPlaying)
            thrusterAudioSource.Stop();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }
}
