using UnityEngine;
using System.Collections.Generic;
using CreAtom;

public static class AtomHelper
{
    public static RequestType[] Collision (ItemPart taker, ItemPart giver)
    {
        List<RequestType> rts = new List<RequestType> (8);
        for (int i = 0; i < giver.atom.gives.Length; ++i) {
            Atom.Reaction give = giver.atom.gives [i];
//            int requestCode = give.element << 4;
            for (int j = 0; j < taker.atom.takes.Length; ++j) {
                Atom.Reaction take = taker.atom.takes [j];

                if (AtomAgent.Instance != null && take.element == give.element) {
                    int reactionCode = take.code & give.code;
                    if (reactionCode == 0 && take.code != 0 && give.code != 0)
                        reactionCode = -1;
                    if (reactionCode == give.code)
                        rts.Add ((RequestType)reactionCode);
                        
//                    requestCode += take.type | give.type;
                }
            }
//            rts.Add ((RequestType)requestCode);
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
        for (int i = 0; i < a_ip.atom.takes.Length; i++)
            if (a_ip.atom.takes [i].code == (int)AtomType.WallDeflect)
                return true;
        return false;
    }
}
