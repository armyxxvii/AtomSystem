using UnityEditor;
using CreAtom;
using UnityEngine;

namespace CreAtom
{
    public enum Element
    {
        無效 = -1,
        /* Ice */
        水 = 0,
        /* Wind */
        風 = 1,
        /* Earth */
        地 = 2,
        /* Fire */
        火 = 3
    }

    [System.Flags] public enum ReactionType
    {
        持續狀態 = 1 << 0,
        //ContinuousSymptom
        持續傷害 = 1 << 1,
        //ContinuousDamage
        瞬間狀態 = 1 << 2,
        //InstantSymptom
        瞬間傷害 = 1 << 3,
        //InstantDamage
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
            using (var cg = new EditorGUI.ChangeCheckScope ()) {
                int gLength = EditorGUILayout.DelayedIntField ("GIVES :", p_gives.arraySize);
                for (int gi = 0; gi < p_gives.arraySize; gi++)
                    DrawReaction ("Give (" + gi + ")", p_gives.GetArrayElementAtIndex (gi));
                if (cg.changed)
                    ModifyArray (p_gives, gLength);
            }

            EditorGUILayout.Separator ();

            using (var ct = new EditorGUI.ChangeCheckScope ()) {
                int tLength = EditorGUILayout.DelayedIntField ("TAKES :", p_takes.arraySize);
                for (int ti = 0; ti < p_takes.arraySize; ti++)
                    DrawReaction ("Take (" + ti + ")", p_takes.GetArrayElementAtIndex (ti));
                if (ct.changed)
                    ModifyArray (p_takes, tLength);
            }

            serializedObject.ApplyModifiedProperties ();

            using (var v = new EditorGUILayout.VerticalScope ("helpbox")) {
                showDef = EditorGUILayout.Foldout (showDef, "Default Inspector");
                if (showDef)
                    DrawDefaultInspector ();
            }
        }

        static void ModifyArray (SerializedProperty Array, int newLength)
        {
            if (newLength > Array.arraySize) {
                
            } else if (newLength < Array.arraySize) {
                
            }
        }

        static void DrawReaction (string _name, SerializedProperty _reaction)
        {
            SerializedProperty _code = _reaction.FindPropertyRelative ("code");
            SerializedProperty _element = _reaction.FindPropertyRelative ("element");
            SerializedProperty _type = _reaction.FindPropertyRelative ("type");
            SerializedProperty _typeMask = _reaction.FindPropertyRelative ("typeMask");
            bool[] rs = new bool[4];
            Element enum_element = (Element)_element.intValue;
            int int_reactionType = 0;

            using (var v1 = new EditorGUILayout.VerticalScope ("helpbox")) {
                EditorGUILayout.LabelField (_name);
                using (var h2 = new EditorGUILayout.HorizontalScope ()) {
                    using (var c = new EditorGUI.ChangeCheckScope ()) {
                        using (var v3 = new EditorGUILayout.VerticalScope ()) {
                            GUILayout.Label ("元素屬性", GUILayout.Width (66));
                            enum_element = (Element)EditorGUILayout.EnumPopup (enum_element, "LargeButton", GUILayout.Height (49));
                            _element.intValue = (int)enum_element;
                        }

                        using (var v3 = new EditorGUILayout.VerticalScope ()) {
                            GUILayout.Label ("反應類型", GUILayout.Width (66));
                            using (var h4 = new EditorGUILayout.HorizontalScope ()) {
                                for (int i = 0; i < 2; i++) {
                                    int flag_r = 1 << i;
                                    rs [i] = ((_type.intValue & flag_r) > 0);
                                    rs [i] = GUILayout.Toggle (rs [i], ((ReactionType)(1 << i)).ToString (), "largebuttonMid");
                                    int_reactionType += rs [i] ? flag_r : 0;
                                }
                            }
                            using (var h4 = new EditorGUILayout.HorizontalScope ()) {
                                for (int i = 2; i < 4; i++) {
                                    int flag_r = 1 << i;
                                    rs [i] = ((_type.intValue & flag_r) > 0);
                                    rs [i] = GUILayout.Toggle (rs [i], ((ReactionType)(1 << i)).ToString (), "largebuttonMid");
                                    int_reactionType += rs [i] ? flag_r : 0;
                                }
                            }
                        }
                        if (c.changed) {
                            _type.intValue = int_reactionType;
                            _code.intValue = _element.intValue == -1 ? -1 : _element.intValue << 4 | int_reactionType;
                        }
                    }
                }
            
                string requestName = _element.intValue == -1 ? "" : RequestTypeName.names [int_reactionType + _element.intValue * 16];
                EditorGUILayout.LabelField (/*"反應"*/"", requestName, "tl selectionbutton");
            }
        }

    }
}
