using UnityEngine;
using System.Collections.Generic;

namespace CreAtom
{
    public static class AtomHelper
    {
        public static RequestType[] Collision (Atom giver, Atom taker)
        {
            List<RequestType> rts = new List<RequestType> (8);
            for (int t = 0; t < taker.takes.Length; ++t) {
                int atom_t = taker.takes [t] & taker.tMasks [t];
                for (int g = 0; g < giver.gives.Length; ++g) {
                    int atom = giver.gives [g] & giver.gMasks [g] & atom_t;
                    if (AtomAgent.Instance != null) {
                        for (int i = 0; i < (int)RequestType.Count; i++) {
                            if ((atom & (1 << i)) > 0 && atom > 0)
                                rts.Add (AtomAgent.Instance.m_maps.m_reaction [i]);
                        }
                    }
                }
            }

            return rts.ToArray ();
        }

        public static bool IsAtom (GameObject a_object)
        {
            return a_object.GetComponent<Atom> () != null;
        }

        public static bool IsDeflected (Atom a_ip)
        {
            bool isDeflected = false;
            const int wd = (int)RequestType.DeflectRequest;

            for (int i = 0; i < a_ip.takes.Length; i++) {
                if ((a_ip.takes [i] & a_ip.tMasks [i]) == wd) {
                    isDeflected = true;
                    break;
                }
            }
            return a_ip != null && isDeflected;
        }
    }
}
