using UnityEngine;
using UnityEditor;

namespace CreAtom
{
    [CustomPropertyDrawer (typeof(PartNode))]
    public class PartNodeDrawer:PropertyDrawer
    {
        const float row = 16;
        const float row2 = 19;
        const float height = 95;
        const float widthRA = 0.35f;
        const float indent = 20;

        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            Rect _rect = EditorGUI.IndentedRect (position);
            _rect.width = 245;
            EditorGUIUtility.wideMode = true;
            var defIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var p_Tpos = property.FindPropertyRelative ("Tpos");
            var p_Trot = property.FindPropertyRelative ("Trot");
            var p_part = property.FindPropertyRelative ("part");
            var p_parentId = property.FindPropertyRelative ("parentId");
            var p_childHides = property.FindPropertyRelative ("childHides");
            var p_childIds = property.FindPropertyRelative ("childIds");
            var p_part_Instance = property.FindPropertyRelative ("part_Instance");

            EditorGUI.BeginProperty (_rect, label, property);

            //Title
            _rect.y -= 2;
            EditorGUI.DrawRect (_rect, new Color (0.1f, 0.5f, 0.5f, 0.2f));
            _rect.y += 2;

            _rect.height = row;
            _rect.x += indent;
            _rect.width -= indent;
            EditorGUI.PropertyField (_rect, p_part, new GUIContent (""));

            Rect _rectI = new Rect (
                              _rect.x + _rect.width - row,
                              _rect.y,
                              row,
                              _rect.height
                          );
            GameObject pI_obj = p_part_Instance.objectReferenceValue as GameObject;
            bool pI_Show = pI_obj != null && pI_obj.activeSelf;
            GUIContent pI_content = new GUIContent ("", pI_Show ? "Show" : "Hide");
            GUI.Box (_rectI, pI_content, pI_Show ? "WinBtnMinMac" : "WinBtnInactiveMac");

            _rect.x -= indent;
            _rect.width += indent;

            //Transform
            Rect _rectColT = new Rect (
                                 _rect.x + _rect.width * widthRA,
                                 _rect.y + row2,
                                 _rect.width - 3 - _rect.width * widthRA,
                                 row + row2
                             );
            EditorGUI.DrawRect (_rectColT, new Color (0.1f, 0.5f, 0.5f, 0.5f));
            EditorGUIUtility.labelWidth = 25f;
            _rectColT.height = row;
            EditorGUI.PropertyField (_rectColT, p_Tpos, new GUIContent ("Pos"), true);
            _rectColT.y += row2;
            EditorGUI.PropertyField (_rectColT, p_Trot, new GUIContent ("Rot"), true);

            //IDs
            int childCount = p_childIds.arraySize;
            Rect _rectColI = new Rect (
                                 _rect.x,
                                 _rect.y + row2,
                                 _rect.width * widthRA - 5,
                                 row
                             );
            EditorGUIUtility.labelWidth = 58f;
            EditorGUI.PropertyField (_rectColI, p_parentId, new GUIContent ("ParentID"));
            _rectColI.y += row2;
            childCount = EditorGUI.DelayedIntField (_rectColI, "ChildsCnt", childCount);

            //Child Ids
            float childSpace = (_rect.width - EditorGUIUtility.labelWidth) / childCount;
            Rect _rectColC = new Rect (
                                 _rect.x + 10,
                                 _rectColI.y + row2,
                                 EditorGUIUtility.labelWidth,
                                 _rectColI.height
                             );
            EditorGUI.LabelField (_rectColC, "ID");
            _rectColC.y += row;
            EditorGUI.LabelField (_rectColC, "Hide");
            _rectColC.x += 20;
            _rectColC.width = 22;
            _rectColC.y -= row;
            _rectColC.x += EditorGUIUtility.labelWidth - 30;
            for (int i = 0; i < p_childIds.arraySize; i++) {
                _rectColC.height += row2;
                GUI.Box (_rectColC, "", "textfield");
                _rectColC.x += 3;
                _rectColC.height -= row2;
                SerializedProperty _int = p_childIds.GetArrayElementAtIndex (i);
                _int.intValue = EditorGUI.DelayedIntField (_rectColC, _int.intValue, "label");
                _rectColC.y += row;
                if (i < p_childHides.arraySize) {
                    SerializedProperty _bool = p_childHides.GetArrayElementAtIndex (i);
                    _bool.boolValue = EditorGUI.Toggle (_rectColC, _bool.boolValue);
                }
                _rectColC.x += childSpace - 3;
                _rectColC.y -= row;
            }

            if (childCount != p_childIds.arraySize) {
                p_childIds.arraySize = childCount;
                p_childHides.arraySize = childCount;
            }
            EditorGUI.EndProperty ();
            EditorGUI.indentLevel = defIndent;
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            return height;
        }
    }
}
