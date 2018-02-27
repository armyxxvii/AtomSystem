using UnityEngine;

namespace CreAtom
{
    [System.Serializable]
    public struct AtomCode
    {
        public int[] gives;
        public int[] gMasks;
        public int[] takes;
        public int[] tMasks;
    }

    [System.Serializable]
    public class Atom : MonoBehaviour
    {
        public AtomCode atomCode;
    }
}
