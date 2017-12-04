using UnityEngine;
using System.Collections.Generic;

namespace AtomSystem
{
    [RequireComponent (typeof(Collider))]
    public class ItemPart : MonoBehaviour
    {
        public ItemTreeStructure item;

        [Header ("造型")]
        public GameObject appearance;
        public List<Collider> cols;

        [Header ("範圍")]
        [Tooltip ("自動抓取\"第一個\"Trigger")]public Collider trigger;

        [Header ("成分")]
        public List<Atom> atoms;
        public List<string> setting;
        public List<ItemPart> childs;

        void Start ()
        {
            if (trigger == null) {
                trigger = GetComponent<Collider> ();
            }
        }

//        static List<int[]> reactions = new List<int[]> ();
        public void OnTriggerEnter (Collider c)
        {
            ItemPart part_B = c.gameObject.GetComponent<ItemPart> ();
            if (part_B == null)
                return;
//            if (reactions.Exists (r => (r [0] == GetInstanceID () && r [1] == part_B.GetInstanceID ()))) {
//                Debug.Log ("<color=green>Hold  " + name + "</color>\n" + part_B.name);
//                reactions.Remove (new [] {GetInstanceID (),part_B.GetInstanceID ()});
//                return;
//            }
//            reactions.Add (new []{ GetInstanceID (), part_B.GetInstanceID () });
            if (part_B != null) {
                bool destroyFlag = false;
                foreach (Atom atomA in atoms) {
                    foreach (Atom atomB in part_B.atoms) {
                        int dCheck = Atom.AtomCheck (atomA, atomB);
                        if (dCheck % 10 > 0)
                            destroyFlag = true;
                    }
                }
                if (destroyFlag) {
                    Debug.Log ("<color=orange>Break " + name + "</color>\n" + part_B.name);
                    item.DestroyPart (this);
                }
            }
        }

    }
}
