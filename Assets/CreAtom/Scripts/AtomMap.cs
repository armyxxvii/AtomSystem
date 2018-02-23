using UnityEngine;

namespace CreAtom
{
    [System.Serializable]
    public struct ActMap
    {
        public RequestType[] m_reaction;
    }

    [System.Serializable]
    public struct HardGiver
    {
        public bool[] isDestroy;
    }

    [CreateAssetMenu(menuName = "CreAtom/AtomMap")]
    public class AtomMap : ScriptableObject
    {
        public ActMap[] acts;
        public HardGiver [] Hards;
    }
}