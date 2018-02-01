using UnityEngine;
namespace CreAtom
{
    public class AtomAgent : MonoBehaviour
    {
        public AtomMap m_maps;
        private static AtomAgent m_instance;
        public static AtomAgent Instance
        {
            get
            {
                return m_instance;
            }
        }

        private void Awake()
        {
            m_instance = this;
        }
    }
}
