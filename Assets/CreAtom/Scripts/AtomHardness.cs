using UnityEngine;

namespace CreAtom
{
    public enum HardnessType
    {
        none,
        劣瓷,
        瓷,
        肉,
        劣木,
        木,
        劣石,
        石,
        劣鐵,
        鐵,
        鑽石,
        Count
    }

    [CreateAssetMenu (menuName = "CreAtom/Hardness")]
    [System.Serializable]
    public class Hardness: ScriptableObject
    {
        public struct isDestroy
        {
            public bool[] giver;
        }
        public static isDestroy [] taker;
    }
}