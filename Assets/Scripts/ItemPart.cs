using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PartNode
{
    public ItemPart part;
    public GameObject part_Instance;
    public GameObject app_Instance;
    //    public int parentId;
    public bool hideChild;
    public List<int> childIds;
}

[RequireComponent (typeof(Collider))]
public class ItemPart : MonoBehaviour
{
    public ItemTreeStructure item;

    [Header ("造型")]
    public GameObject appearance;

    [Header ("範圍")]
    [Tooltip("自動抓取\"第一個\"Trigger")]public Collider trigger;

    [Header ("硬度")]
    [Range (0, 99)] public int hardness;

    [Header ("成分")]
    public Atom atom;
    public List<Atom> atoms;

}
