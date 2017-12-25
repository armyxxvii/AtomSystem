using UnityEngine;
using System.Collections.Generic;

namespace CreAtom
{
    [RequireComponent (typeof(Collider))]
    [System.Serializable]
    public class ItemPart : MonoBehaviour
    {
        public ItemTreeStructure item;

        [Header ("造型")]
        public GameObject appearance;

        [Header ("範圍")]
        [Tooltip ("自動抓取\"第一個\"Trigger")]public Collider trigger;

        [Header ("成分")]
        public List<Atom> atoms;

        void Start ()
        {
            if (trigger == null) {
                trigger = GetComponent<Collider> ();
            }
        }


        public void OnTriggerEnter (Collider c)
        {
            ItemPart part_B = c.gameObject.GetComponent<ItemPart> ();
            if (part_B == null)
                return;
            
                bool destroyFlag = false;
                foreach (Atom atomA in atoms) {
                    foreach (Atom atomB in part_B.atoms) {
                        int dCheck = Atom.AtomCheck (atomA, atomB);
                        if (dCheck % 10 > 0)
                            destroyFlag = true;
                    }
                }
                if (destroyFlag) {
                Debug.Log ("<" + GetInstanceID () + "><color=red>" + name + "</color> was destroyed by " +
                "<" + part_B.GetInstanceID () + "><color=orange>" + part_B.name + "</color>\n");
                    DestroySelf ();
                }
            }

        public void DestroySelf ()
        {
            if (item)
                item.DestroyPart (this);
            else
                Destroy (gameObject);
        }

    }
}
