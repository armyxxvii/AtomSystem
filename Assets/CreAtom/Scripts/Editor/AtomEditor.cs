using UnityEditor;
using CreAtom;
using UnityEngine;
using System;

namespace CreAtom
{
    public enum Element
    {
        無效 = 0,
        風 = 1 << 5,
        地 = 2 << 5,
        火 = 3 << 5,
        水 = 4 << 5,
    }

    public enum ReactionType
    {
        //ContinuousSymptom
        持續狀態 = 0 << 3,
        //ContinuousDamage
        持續傷害 = 1 << 3,
        //InstantSymptom
        瞬間狀態 = 2 << 3,
        //InstantDamage
        瞬間傷害 = 3 << 3,
    }

    public enum ModifyType
    {
        無效,
        非補正,
        _解除,
        _散播,
        _範圍降低,
        _範圍提昇,
        _強度降低,
        _強度提昇,
    }

    [CustomEditor (typeof(Atom))]
    public class AtomEditor : Editor
    {
        SerializedProperty p_gives, p_takes;
        public bool showDef = true;

        void OnEnable ()
        {
            p_gives = serializedObject.FindProperty ("gives");
            p_takes = serializedObject.FindProperty ("takes");
        }

        public override void OnInspectorGUI ()
        {
            EditorGUIUtility.labelWidth = 70;

            showDef = EditorGUILayout.ToggleLeft ("Show Default Inspector",showDef);
            EditorGUILayout.Separator ();
            if (showDef)
                DrawDefaultInspector ();
            else {
                using (var cg = new EditorGUI.ChangeCheckScope ()) {
                    int gLength = EditorGUILayout.DelayedIntField ("GIVES :", p_gives.arraySize);
                    if (gLength != p_gives.arraySize)
                        ModifyArray (p_gives, gLength);
                }
                for (int gi = 0; gi < p_gives.arraySize; gi++)
                    DrawReaction ("Give (" + gi + ")", p_gives.GetArrayElementAtIndex (gi));

                EditorGUILayout.Separator ();

                using (var ct = new EditorGUI.ChangeCheckScope ()) {
                    int tLength = EditorGUILayout.DelayedIntField ("TAKES :", p_takes.arraySize);
                    if (tLength != p_takes.arraySize)
                        ModifyArray (p_takes, tLength);
                }
                for (int ti = 0; ti < p_takes.arraySize; ti++)
                    DrawReaction ("Take (" + ti + ")", p_takes.GetArrayElementAtIndex (ti));

                serializedObject.ApplyModifiedProperties ();
            }
        }

        static void ModifyArray (SerializedProperty Array, int newLength)
        {
            if (newLength == Array.arraySize)
                return;
            if (newLength > Array.arraySize) {
                for (int i = Array.arraySize; i < newLength; i++) {
                    if (i > -1)
                        Array.InsertArrayElementAtIndex (i);
                }
            } else if (newLength < Array.arraySize) {
                for (int i = Array.arraySize - 1; i > newLength - 1; i--) {
                    if (i > -1)
                        Array.DeleteArrayElementAtIndex (i);
                }
            }
        }

        static void DrawReaction (string _name, SerializedProperty _reaction)
        {
            Element enumE = (Element)(_reaction.intValue & 7 << 5);
            ReactionType enumR = (ReactionType)(_reaction.intValue & 3 << 3);
            ModifyType enumM = (ModifyType)(_reaction.intValue & 3);

            using (var v1 = new EditorGUILayout.VerticalScope ("helpbox")) {
                EditorGUILayout.LabelField (_name);
                using (var c = new EditorGUI.ChangeCheckScope ()) {
                    using (var h2 = new EditorGUILayout.HorizontalScope ()) {
                        using (var ve = new EditorGUILayout.VerticalScope ()) {
                            GUILayout.Label ("元素屬性", GUILayout.Width (50));
                            enumE = (Element)EditorGUILayout.EnumPopup (enumE, "DropDown");
                        }
                        using (var vr = new EditorGUILayout.VerticalScope ()) {
                            GUILayout.Label ("反應類型", GUILayout.Width (50));
                            enumR = (ReactionType)EditorGUILayout.EnumPopup (enumR, "DropDown");
                        }
                        using (var vm = new EditorGUILayout.VerticalScope ()) {
                            GUILayout.Label ("補正類型", GUILayout.Width (50));
                            enumM = (ModifyType)EditorGUILayout.EnumPopup (enumM, "DropDown");
                        }
                    }
                    if (c.changed)
                        _reaction.intValue = (int)enumE + (int)enumR + (int)enumM;
                }
                GUILayout.Space (3f);
                string requestName = _reaction.intValue == -1 ? "" : RequestTypeName.names [_reaction.intValue];
                requestName += " (" + Convert.ToString (_reaction.intValue, 2).PadLeft (8, '0');
                requestName += "_" + (RequestType)_reaction.intValue + ")";
                EditorGUILayout.TextField ("", requestName, "dockarea");
            }
        }

    }
}