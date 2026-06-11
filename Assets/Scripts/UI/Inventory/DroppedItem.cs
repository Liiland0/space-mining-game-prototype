using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DroppedItem : MonoBehaviour
{
    public ItemType itemType;

    [SerializeField] private float pickupRadius = 2.5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float collectRadius = .55f;


    private GameObject player;
    private SpriteRenderer sr;
    private InventoryManager inventory;

    private float pickupRadiusSqr;
    private float collectRadiusSqr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        pickupRadiusSqr = pickupRadius * pickupRadius;
        collectRadiusSqr = collectRadius * collectRadius;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (player == null) return;

        Vector2 diff = (Vector2)(player.transform.position - transform.position);
        float distSqr = diff.sqrMagnitude;

        if (distSqr > pickupRadiusSqr) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.transform.position,
            moveSpeed * Time.deltaTime
        );

        if (distSqr < collectRadiusSqr)
        {
            if (inventory.PickUp(itemType, 1))
            {
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(ItemType type)
    {
        itemType = type;
        sr.sprite = itemType.invIcon;
    }
}