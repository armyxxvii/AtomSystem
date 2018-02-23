using UnityEngine;
using System.Collections.Generic;

namespace CreAtom
{
    [System.Serializable]
    public class ItemPart : MonoBehaviour
    {
        [Header ("硬度")]
        public HardnessType hardness;

        [Header ("原子")]
        public List<Atom> atoms;

    }
}
