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
        bool[] showActs;
        AtomMap atm;
        static int capacityRT;

        void OnEnable ()
        {
            atm = (AtomMap)target;
            capacityRT = (int)RequestType.Count;
            showActs = new bool[capacityRT+1];
        }

        public override void OnInspectorGUI ()
        {
            showActs [capacityRT] = EditorGUILayout.Foldout (showActs [capacityRT], "Acts");
            if (showActs [capacityRT]) {
                EditorGUI.indentLevel++;
                for (int i = 0; i < atm.acts.Length; i++) {
                    using (var h = new GUILayout.HorizontalScope ()) {
                        string id = Convert.ToString (i, 2).PadLeft (8, '0');
                        showActs [i] = EditorGUILayout.Foldout (showActs [i], "[" + id + "] " + RequestTypeName.names [i]);
                        if (showActs [i]) {
                            int rl = atm.acts [i].m_reaction.Length;
                            int rn = EditorGUILayout.DelayedIntField (rl, GUILayout.Width (30));

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
                    if (showActs [i]) {
                        for (int j = 0; j < atm.acts [i].m_reaction.Length; j++) {
                            atm.acts [i].m_reaction [j] = (RequestType)EditorGUILayout.EnumPopup (j.ToString (), atm.acts [i].m_reaction [j]);
                        }
                    }
                }
                if (GUILayout.Button ("Update"))
                    UpdateAtomMap ();
                EditorGUI.indentLevel--;
            }

            using (var v = new EditorGUILayout.VerticalScope ("helpbox")) {
                showDef = EditorGUILayout.Foldout (showDef, "Default Inspector");
                if (showDef)
                    DrawDefaultInspector ();
            }
        }

        void UpdateAtomMap ()
        {
            if (atm.acts.Length < capacityRT) {
                ActMap[] newActs = new ActMap[capacityRT];
                for (int i = 0; i < capacityRT; i++) {
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
