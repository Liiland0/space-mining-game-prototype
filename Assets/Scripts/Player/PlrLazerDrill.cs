using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class PlrLazerDrill : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private LineRenderer lineRenderer;
    private Transform firePoint;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip drillSound;

    [Header("Lazer Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 100f;
    [SerializeField] private float cooldown = 1f / 3f;
    [SerializeField] private float thickness = 0.05f;

    private bool firing = false;
    private float timer;

    private Vector3 endPoint;

    void Awake()
    {
        firePoint = transform;
        timer = cooldown;

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.clip = drillSound;
        audioSource.loop = true;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (firing)
            Shoot();
        else
            lineRenderer.enabled = false;
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, range, hitLayers);

        if (hit.collider != null)
        {
            endPoint = hit.point;
            AsteroidType type = hit.collider.GetComponent<AsteroidGenerator>()?.asteroidType;

            if (timer >= cooldown && type != null && damage >= type.durability)
            {
                Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                if (tilemap == null)
                    tilemap = hit.collider.GetComponentInChildren<Tilemap>();

                if (tilemap != null)
                {
                    Vector3 hitPos = hit.point - hit.normal * 0.05f;
                    Vector3Int cell = tilemap.WorldToCell(hitPos);

                    if (!tilemap.HasTile(cell))
                    {
                        Vector3Int[] neighbors = { cell, cell + Vector3Int.right, cell + Vector3Int.up, cell + new Vector3Int(1, 1, 0) };
                        foreach (var c in neighbors)
                        {
                            if (tilemap.HasTile(c))
                            {
                                cell = c;
                                break;
                            }
                        }
                    }

                    if (tilemap.HasTile(cell))
                    {
                        tilemap.SetTile(cell, null);
                        AsteroidGenerator generator = tilemap.GetComponentInParent<AsteroidGenerator>();

                        // Todo: make destroying a tile a method in generator
                        DropItem(generator.asteroidType, tilemap.GetCellCenterWorld(cell));
                        generator.CopyTilemapToMiniMap();

                        if (generator != null)
                            generator.RecalculateMassAndCenter();
                    }
                }
                timer = 0f;
            }
        }
        else
            endPoint = firePoint.position + firePoint.up * range;

        DrawLaser(endPoint);
    }

    private void DropItem(ItemType type, Vector3 position)
    {
        GameObject droppedItem = Instantiate(droppedItemPrefab, position, Quaternion.identity);
        droppedItem.GetComponent<DroppedItem>().Initialize(type);
    }

    private void DrawLaser(Vector3 target)
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target);
        lineRenderer.enabled = true;
    }

    public void OnFire1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            firing = true;

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (context.canceled)
        {
            firing = false;
            lineRenderer.enabled = false;

            audioSource.Stop();
        }
    }

    public void UpgradeDrillPower(int amount)
    {
        damage += amount;
    }

    void OnEnable()
    {
        if (firing && audioSource != null)
            audioSource.Play();
    }

    void OnDisable()
    {
        if (audioSource != null) audioSource.Stop();
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint != null)
            Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.up * range);
    }
}