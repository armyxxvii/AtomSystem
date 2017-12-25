using UnityEngine;
using System.Collections.Generic;
using System;

namespace CreAtom
{
    public enum AtomFamily
    {
        unknown = 0,
        Movement = 1,
        Life = 2,
        Solid = 99
    }

    [Flags] public enum  AtomResult
    {
        nothing = 0,
        destroy = 1 << 0,
        checkempty = 1 << 1,
        degrade = 1 << 2,
    }

    [CreateAssetMenu (menuName = "CreAtom/Atom")]
    public class Atom : ScriptableObject
    {
        [System.Serializable]
        public struct Reaction
        {
            public Atom hitAtom;
            public AtomResult result;
            public int resultInt;
        }

        public AtomFamily family;
        public List<Reaction> reactions;

        public AtomResult Hit (Atom _hitAtom)
        {
            return reactions.Find (atom => atom.hitAtom.name == _hitAtom.name).result;
        }
    }
}
