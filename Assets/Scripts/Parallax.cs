using UnityEditor.ShortcutManagement;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField][Range(0f, 1f)] float parallax;

    float width;
    float height;

    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        width = sr.bounds.size.x;
        height = sr.bounds.size.y;
    }

    void LateUpdate()
    {
        Vector3 camPos = cam.position;

        float x = Mathf.Repeat(camPos.x * parallax, width);
        float y = Mathf.Repeat(camPos.y * parallax, height);

        transform.position = new Vector3(
            camPos.x - x,
            camPos.y - y,
            transform.position.z
        );
    }
}