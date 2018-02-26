using UnityEngine;

namespace CreAtom
{
    [CreateAssetMenu (menuName = "CreAtom/AtomMap")]
    public class AtomMap : ScriptableObject
    {
        public RequestType[] m_reaction;
    }
}