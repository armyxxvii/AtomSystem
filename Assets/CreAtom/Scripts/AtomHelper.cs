using UnityEngine;
using System.Collections.Generic;
using CreAtom;

public class AtomHelper
{
    public static RequestType[] Collision(ItemPart a_1, ItemPart a_2)
    {
        List<RequestType> rts = new List<RequestType>(8);
        AtomType a1;
        AtomType a2;
        for (int i = 0; i < a_1.atoms.Count; ++i)
        {
            for (int j = 0; j < a_2.atoms.Count; ++j)
            {
                a1 = a_1.atoms[i].type;
                a2 = a_2.atoms[j].type;

                if (AtomAgent.Instance != null)
                {
                    rts.Add(AtomAgent.Instance.m_maps.acts[(int)a1].m_reaction[(int)a2]);
                }
            }
        }

        return rts.ToArray();
    }

    public static bool IsAtom(GameObject a_object)
    {
        return a_object.GetComponent<ItemPart>() != null;
    }

    public static bool IsDeflected(ItemPart a_ip)
    {
        return a_ip != null && a_ip.atoms[0].type == AtomType.WallDeflect;
    }
}
