using System.Collections.Generic;
using UnityEngine;

namespace AtomSystem
{
    [System.Serializable]
    public class PartNode
    {
        public ItemPart part;
        public GameObject part_Instance;
        public GameObject app_Instance;
        public int parentId;
        public bool hideChild;
        public List<int> childIds;
    }
}
