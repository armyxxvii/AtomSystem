using System.Collections.Generic;
using UnityEngine;

namespace CreAtom
{
    [System.Serializable]
    public class PartNode
    {
        public Vector3 Tpos;
        public Vector3 Trot;
        public ItemPart part;
        public GameObject part_Instance;
        public GameObject app_Instance;
        public int parentId;
        public List<bool> childHides = new List<bool>();
        public List<int> childIds = new List<int>();

        #if UNITY_EDITOR
        [System.NonSerialized]public bool showChild = true;
        #endif

        public PartNode()
        {
            Tpos = Vector3.zero;
            Trot = Vector3.zero;
            part = null;
            part_Instance = null;
            app_Instance = null;
            parentId = -1;
            childHides = new List<bool> ();
            childIds = new List<int> ();
        }

        public PartNode Clone()
        {
            PartNode clone = new PartNode ();
            clone.Tpos = Tpos;
            clone.Trot = Trot;
            clone.part = part;
            clone.parentId = parentId;
            foreach (bool h in childHides)
                clone.childHides.Add (h);
            foreach (int i in childIds)
                clone.childIds.Add (i);
            return clone;
        }
    }
}
