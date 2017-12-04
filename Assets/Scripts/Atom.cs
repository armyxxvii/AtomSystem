using UnityEngine;
using System.Collections.Generic;

namespace AtomSystem
{
    public enum AtomFamily
    {
        unknown = 0,
        Solid = 99
    }

    [CreateAssetMenu (menuName = "AtomSystem/Atom")]
    public class Atom : ScriptableObject
    {
        public AtomFamily family;

        public static int AtomCheck (Atom _a, Atom _b)
        {
            if (_a.family == AtomFamily.Solid && _b.family == AtomFamily.Solid) {
                return HardnessCheck (_a, _b);
//            } else {
//
            }
            return 0;
        }

        static int HardnessCheck (Atom _a, Atom _b)
        {
            int result = 0;
            if (_a.name.Contains ("玻璃"))
                result += 1;
            if (_b.name.Contains ("玻璃"))
                result += 10;
            return result;
        }
    }
}
