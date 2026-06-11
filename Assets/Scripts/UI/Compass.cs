using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] private Transform pointTo;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, pointTo.position);
    }
}