using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "AsteroidType", menuName = "Scriptable Objects/ItemType/AsteroidType")]
public class AsteroidType : ItemType
{
    [Header("Asteroid Properties")]
    public TileBase tileBase;
    public Color minimapColor;
    public int durability;
}
