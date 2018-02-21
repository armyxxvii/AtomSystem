using UnityEngine;
using System;

namespace CreAtom
{
    [CreateAssetMenu (menuName = "CreAtom/Atom")]
    public class Atom : ScriptableObject
    {

        [Serializable]
        public struct Reaction{
            public int code;
            public int element;
            public int type;
        }

        public Reaction[] gives;
        public Reaction[] takes;

        public static int GetReactionCode (Reaction _reaction)
        {
            return _reaction.element << 4 + _reaction.type;
        }
    }
}
