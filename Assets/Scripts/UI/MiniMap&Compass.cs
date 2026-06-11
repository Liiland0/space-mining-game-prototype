using UnityEngine;

public class MiniMapAndCompass : MonoBehaviour
{
    [SerializeField] private Transform miniCamTransform;

    void LateUpdate()
    {
        miniCamTransform.rotation = Quaternion.identity;
    }
}
