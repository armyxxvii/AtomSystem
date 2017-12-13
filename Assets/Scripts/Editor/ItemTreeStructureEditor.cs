using UnityEditor;
using UnityEngine;

namespace CreAtom
{
    [CustomEditor (typeof(ItemTreeStructure))]
    public class ItemTreeStructureEditor : Editor
    {
        ItemTreeStructure its;
        SerializedProperty p_partNodes;
        public bool sortByTree = true;

        void OnEnable ()
        {
            its = (ItemTreeStructure)target;
            p_partNodes = serializedObject.FindProperty ("partNodes");
        }

        public override void OnInspectorGUI ()
        {
            //reset button
            if (Application.isPlaying) {
                if (GUILayout.Button ("Reset"))
                    its.Reset ();
            }
            
            sortByTree = EditorGUILayout.Toggle ("Sort By Tree", sortByTree);
            if (!sortByTree) {
                //Draw Nodes by index
                for (int i = 0; i < its.partNodes.Count; i++)
                    DrawNode (i);
            } else {
                //Draw Nodes by tree
                DrawChildNodes (its.rootNode);
            }
            serializedObject.ApplyModifiedProperties ();
        }

        void DrawChildNodes (PartNode parentNode)
        {
            if (parentNode.childIds.Count > 0) {
                foreach (int i in parentNode.childIds) {
                    DrawNode (i);
                    if (its.partNodes [i].showChild) {
                        EditorGUI.indentLevel++;
                        DrawChildNodes (its.partNodes [i]);
                        EditorGUI.indentLevel--;
                    }
                }
            }
        }

        void DrawNode (int index)
        {
            EditorGUILayout.PropertyField (p_partNodes.GetArrayElementAtIndex (index));
            Rect r = GUILayoutUtility.GetLastRect ();
            EditorGUI.LabelField (r, index.ToString (), EditorStyles.boldLabel);
            if (its.partNodes [index].childIds.Count > 0)
                its.partNodes [index].showChild = EditorGUI.Foldout (r, its.partNodes [index].showChild, "");
        }

    }
}
