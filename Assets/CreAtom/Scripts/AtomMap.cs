using UnityEngine;

namespace CreAtom
{
    [System.Serializable]
    public struct ActMap
    {
        public RequestType[] m_reaction;
    }

    [CreateAssetMenu(menuName = "CreAtom/AtomMap")]
    public class AtomMap : ScriptableObject
    {
        public ActMap[] acts;
    }
}