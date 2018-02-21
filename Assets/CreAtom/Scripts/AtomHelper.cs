using UnityEngine;
using System.Collections.Generic;
using CreAtom;

public static class AtomHelper
{
    public static RequestType[] Collision (ItemPart taker, ItemPart giver)
    {
        List<RequestType> rts = new List<RequestType> (8);
        for (int g = 0; g < giver.atoms.Count; ++g) {
            foreach (var give in giver.atoms [g].gives) {
                for (int t = 0; t < taker.atoms.Count; ++t) {
                    foreach (var take in taker.atoms [t].takes) {
                        if (AtomAgent.Instance != null && take.element == give.element) {
                            int reactionCode = take.code & give.code;
                            if (reactionCode == 0 && take.code != 0 && give.code != 0)
                                reactionCode = -1;
                            if (reactionCode == give.code)
                                rts.Add ((RequestType)reactionCode);
                        }
                    }
                }
            }
        }

        return rts.ToArray ();
    }

    public static bool IsAtom (GameObject a_object)
    {
        return a_object.GetComponent<ItemPart> () != null;
    }

    public static bool IsDeflected (ItemPart a_ip)
    {
        if (a_ip == null)
            return false;
        const int wallDeflect = (int)AtomType.WallDeflect;
        for (int i = 0; i < a_ip.atoms.Count; i++)
            for (int t = 0; t < a_ip.atoms [i].takes.Length; t++)
                if (a_ip.atoms [i].takes [t].code == wallDeflect)
                    return true;
        return false;
    }
}
