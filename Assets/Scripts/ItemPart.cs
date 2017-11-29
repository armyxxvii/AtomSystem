using UnityEngine;

public class ItemPart : MonoBehaviour
{
    [Header ("造型")]
    public GameObject appearance;

    [Header ("範圍")]
    [Tooltip ("碰撞體")]
    public Collider trigger;

    [Header ("硬度")]
    [Tooltip ("硬度"),Range (0, 99)]
    public int hardness;

    [Header ("原子")]
    [Tooltip ("原子")]
    public Atom atom;

}
