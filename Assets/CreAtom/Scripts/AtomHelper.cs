using UnityEngine;
using System.Collections.Generic;

namespace CreAtom
{
    public static class AtomHelper
    {
        public static RequestType[] Collision (Atom taker, Atom giver)
        {
            List<RequestType> rts = new List<RequestType> (8);
            for (int i = 0; i < taker.takes.Length; ++i) {
                int atom_t = taker.takes [i] & taker.tMasks [i];
                for (int j = 0; j < giver.gives.Length; ++j) {
                    int atom_g = giver.gives [j] & giver.gMasks [j];
                    if (AtomAgent.Instance != null) {
                        rts.Add (AtomAgent.Instance.m_maps.m_reaction [atom_t & atom_g]);
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
