using UnityEngine;

public class RadiusInteract : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Door")
            collision.GetComponent<Animator>().SetBool("Opened", true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door")
            collision.GetComponent<Animator>().SetBool("Opened", false);
    }
}