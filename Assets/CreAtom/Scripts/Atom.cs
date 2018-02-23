using UnityEngine;
using System;

namespace CreAtom
{
    [CreateAssetMenu (menuName = "CreAtom/Atom")]
    public class Atom : ScriptableObject
    {
        public int[] gives;
        public int[] takes;
    }
}
