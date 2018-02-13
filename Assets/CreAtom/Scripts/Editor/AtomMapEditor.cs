using UnityEngine;
using UnityEditor;
using CreAtom;
using System;

namespace CreAtom
{
    [CustomEditor (typeof(AtomMap))]
    public class AtomMapEditor : Editor
    {
        public bool showDef = true;
        bool[] show_1;
        AtomMap atm;
        static int capacity;

        void OnEnable ()
        {
            atm = (AtomMap)target;
            capacity = (int)RequestType.Count;
            show_1 = new bool[capacity];
        }

        public override void OnInspectorGUI ()
        {
            for (int i = 0; i < atm.acts.Length; i++) {
                using (var h = new GUILayout.HorizontalScope ()) {
                    string id = Convert.ToString (i, 2).PadLeft (6, '0');
                    show_1 [i] = EditorGUILayout.Foldout (show_1 [i], "[" + id + "] " + RequestTypeName.names [i]);
                    if (show_1 [i]) {
                        int rl = atm.acts [i].m_reaction.Length;
                        int rn = EditorGUILayout.DelayedIntField (rl,GUILayout.Width(30));

                        if (rn > rl) {
                            RequestType[] newRt = new RequestType[rn];
                            for (int r = 0; r < rn; r++)
                                newRt [r] = RequestType.None;
                            atm.acts [i].m_reaction.CopyTo (newRt, 0);
                            atm.acts [i].m_reaction = newRt;
                        } else if (rn < rl) {
                            RequestType[] newRt = new RequestType[rn];
                            for (int r = 0; r < rn; r++)
                                newRt [r] = atm.acts [i].m_reaction [r];
                            atm.acts [i].m_reaction = newRt;
                        }

                    }
                }
                if (show_1 [i]) {
                    for (int j = 0; j < atm.acts [i].m_reaction.Length; j++) {
                        atm.acts [i].m_reaction [j] = (RequestType)EditorGUILayout.EnumPopup (j.ToString (), atm.acts [i].m_reaction [j]);
                    }
                }
            }

            if (GUILayout.Button ("Update"))
                UpdateAtomMap ();
            using (var v = new EditorGUILayout.VerticalScope ("helpbox")) {
                showDef = EditorGUILayout.Foldout (showDef, "Default Inspector");
                if (showDef)
                    DrawDefaultInspector ();
            }
        }

        void UpdateAtomMap ()
        {
            if (atm.acts.Length < capacity) {
                ActMap[] newActs = new ActMap[capacity];
                for (int i = 0; i < capacity; i++) {
                    if (i < atm.acts.Length) {
                        newActs [i] = atm.acts [i];
                    } else {
                        newActs [i] = new ActMap();
                        newActs [i].m_reaction = new []{(RequestType)i};
                    }
                }
                atm.acts = newActs;
            }
        }
    }
}
