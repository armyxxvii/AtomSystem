using UnityEditor;
using CreAtom;
using UnityEngine;
using System;

namespace CreAtom
{
    public enum ReactionType
    {
        旗標0_Hit,
        旗標1_Deflect,
        旗標2_ItemHit,
        旗標3_SoftFlash,

        Count
    }

    [CustomEditor (typeof(Atom))]
    public class AtomEditor : Editor
    {
        SerializedProperty p_gives, p_gMasks, p_takes, p_tMasks;
        public bool showDef = false;

        void OnEnable ()
        {
            p_gives = serializedObject.FindProperty ("gives");
            p_gMasks = serializedObject.FindProperty ("gMasks");
            p_takes = serializedObject.FindProperty ("takes");
            p_tMasks = serializedObject.FindProperty ("tMasks");
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
                    DrawReaction ("Give (" + gi + ")", p_gives.GetArrayElementAtIndex (gi),p_gMasks.GetArrayElementAtIndex (gi));

                EditorGUILayout.Separator ();

                using (var ct = new EditorGUI.ChangeCheckScope ()) {
                    int tLength = EditorGUILayout.DelayedIntField ("TAKES :", p_takes.arraySize);
                    if (tLength != p_takes.arraySize)
                        ModifyArray (p_takes, tLength);
                }
                for (int ti = 0; ti < p_takes.arraySize; ti++)
                    DrawReaction ("Take (" + ti + ")", p_takes.GetArrayElementAtIndex (ti),p_tMasks.GetArrayElementAtIndex (ti));

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

        static bool[] SplitInt(int _value,int _bit)
        {
            bool[] _result = new bool[_bit];
            for (int i = 0; i < _bit; i++) {
                _result [i] = ((_value >> i) & 1) > 0;
            }
            return _result;
        }

        static int CombineInt(bool[] _flags)
        {
            int _result = 0;
            for (int i = 0; i < _flags.Length; i++) {
                _result += _flags [i] ? 1 << i : 0;
            }
            return _result;
        }

        static void DrawReaction (string _name, SerializedProperty _reaction, SerializedProperty _mask)
        {
            const int bCount = (int)ReactionType.Count;
            bool[] a_reaction = SplitInt(_reaction.intValue,bCount);
            bool[] a_mask = SplitInt(_mask.intValue,bCount);

            using (var v1 = new EditorGUILayout.VerticalScope ("helpbox")) {
                EditorGUILayout.LabelField (_name);
                using (var c = new EditorGUI.ChangeCheckScope ()) {
                    for (int i = 0; i < (int)ReactionType.Count; i++) {
                        using (var h2 = new EditorGUILayout.HorizontalScope ()) {
                            a_mask [i] = EditorGUILayout.ToggleLeft ("Mask",a_mask [i],GUILayout.Width(60));
                            a_reaction [i] = EditorGUILayout.ToggleLeft (((ReactionType)i).ToString(),a_reaction [i]);
                        }
                    }
                    if (c.changed){
                        _reaction.intValue = CombineInt(a_reaction);
                        _mask.intValue = CombineInt (a_mask);
                    }
                }
                GUILayout.Space (3f);
                using (var h2 = new EditorGUILayout.HorizontalScope ()) {
                    EditorGUILayout.TextField ("", _mask.intValue.ToString(), "dockarea", GUILayout.Width (60));
                    EditorGUILayout.TextField ("", _reaction.intValue.ToString(), "dockarea");
                }
            }
        }

    }
}