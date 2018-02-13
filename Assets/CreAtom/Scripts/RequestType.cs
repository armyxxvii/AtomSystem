﻿namespace CreAtom
{
    public static class RequestTypeName
    {
        public static readonly string[] names = {
            "可  凍",
            "緩  慢",
            "中  毒",
            "水持續UP",
            "冰  凍",
            "水狀態UP",
            "???",
            "緩慢UP",
            "凍  傷",
            "???",
            "水傷害UP",
            "中毒UP",
            "水瞬間UP",
            "冰凍UP",
            "凍傷UP",
            "持續狀態UP",

            "導  電",
            "麻  痺",
            "帶  電",
            "風持續UP",
            "瞬風態",
            "風狀態UP",
            "???",
            "麻痺UP",
            "雷  電",
            "???",
            "風傷害UP",
            "帶電UP",
            "風瞬間UP",
            "瞬風態UP",
            "雷電UP",
            "持續傷害UP",

            "受  擊",
            "暈  眩",
            "出  血",
            "地持續UP",
            "反  彈",
            "地狀態UP",
            "???",
            "暈眩UP",
            "打  擊",
            "???",
            "地傷害UP",
            "出血UP",
            "地瞬間UP",
            "反彈UP",
            "打擊UP",
            "瞬間狀態UP",

            "可  燃",
            "失  明",
            "燃  燒",
            "火持續UP",
            "爆  炸",
            "火狀態UP",
            "???",
            "失明UP",
            "灼  傷",
            "???",
            "火傷害UP",
            "燃燒UP",
            "火瞬間UP",
            "爆炸UP",
            "灼傷UP",
            "瞬間傷害UP"
        };
    }

    public enum RequestType
    {
        None = -1,

        Freezable = 0,
        Slow = 1,
        Poison = 2,
        _Ice_CntAny_Up = 3,
        Freeze = 4,
        _Ice_AnySym_Up = 5,
        _000110 = 6,
        _Ice_CntSym_Up = 7,
        Frostbite = 8,
        _001001 = 9,
        _Ice_AnyDmg_Up = 10,
        _Ice_CntDmg_Up = 11,
        _Ice_InsAny_Up = 12,
        _Ice_InsSym_Up = 13,
        _Ice_InsDmg_Up = 14,
        _All_CntSym_Up = 15,
        Conductive = 16,
        Paralysis = 17,
        續風傷 = 18,
        _Wind_CntAny_Up = 19,
        瞬風態 = 20,
        _Wind_AnySym_Up = 21,
        _010110 = 22,
        _Wind_CntSym_Up = 23,
        Lightning = 24,
        _011001 = 25,
        _Wind_AnyDmg_Up = 26,
        _Wind_CntDmg_Up = 27,
        _Wind_InsAny_Up = 28,
        _Wind_InsSym_Up = 29,
        _Wind_InsDmg_Up = 30,
        _All_CntDmg_Up = 31,
        Defender = 32,
        Dizzy = 33,
        Bleeding = 34,
        _Earth_CntAny_Up = 35,
        WallDeflect = 36,
        _Earth_AnySym_Up = 37,
        _100110 = 38,
        _Earth_CntSym_Up = 39,
        Attacker = 40,
        _101001 = 41,
        _Earth_AnyDmg_Up = 42,
        _Earth_CntDmg_Up = 43,
        _Earth_InsAny_Up = 44,
        _Earth_InsSym_Up = 45,
        _Earth_InsDmg_Up = 46,
        _All_InsSym_Up = 47,
        Flammable = 48,
        Blind = 49,
        Burn = 50,
        _Fire_CntAny_Up = 51,
        Explosion = 52,
        _Fire_AnySym_Up = 53,
        _110110 = 54,
        _Fire_CntSym_Up = 55,
        Scald = 56,
        _111001 = 57,
        _Fire_AnyDmg_Up = 58,
        _Fire_CntDmg_Up = 59,
        _Fire_InsAny_Up = 60,
        _Fire_InsSym_Up = 61,
        _Fire_InsDmg_Up = 62,
        _All_InsDmg_Up = 63,

        Count,
    }
}