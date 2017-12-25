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
        public bool simple;

        void OnEnable ()
        {
            its = (ItemTreeStructure)target;
            p_partNodes = serializedObject.FindProperty ("partNodes");
        }

        public override void OnInspectorGUI ()
        {
            EditorGUIUtility.labelWidth = 60;
            //reset button
            if (Application.isPlaying) {
                if (GUILayout.Button ("ReStart", GUILayout.Height (30)))
                    its.Restart ();
            }
            //function
            using (var h = new EditorGUILayout.HorizontalScope ("helpbox")) {
                sortByTree = EditorGUILayout.ToggleLeft ("Sort By Tree", sortByTree);
                simple = EditorGUILayout.ToggleLeft ("Simple Node", simple);
            }
            //function.RootChilds
            using (var h = new EditorGUILayout.HorizontalScope ("helpbox")) {
                EditorGUILayout.LabelField ("RootChilds", GUILayout.Width (EditorGUIUtility.labelWidth));
                for (int i = 0; i < its.rootNode.childIds.Count; i++) {
                    using (var v2 = new EditorGUILayout.VerticalScope ("textfield", GUILayout.Width (16))) {
                        its.rootNode.childIds [i] = EditorGUILayout.DelayedIntField (its.rootNode.childIds [i], "label", GUILayout.Width (18));
                        its.rootNode.childHides [i] = EditorGUILayout.Toggle (its.rootNode.childHides [i]);
                    }
                }
                EditorGUILayout.Space ();
            }
            //node list
            if (sortByTree) {
                //Draw Nodes by tree
                DrawTree (its.rootNode);
            } else {
                //Draw Nodes by index
                for (int i = 0; i < its.partNodes.Count; i++)
                    DrawNode (i);
            }
            serializedObject.ApplyModifiedProperties ();

            if (!Application.isPlaying) {
                TreeSizeTool ();
                SetParentTool ();
            }
        }

        #region TreeSizeTool
        int count;
        bool countChange;

        void TreeSizeTool ()
        {
            using (var h = new EditorGUILayout.HorizontalScope ("helpbox")) {
                using (var c = new EditorGUI.ChangeCheckScope ()) {
                    count = EditorGUILayout.DelayedIntField ("Tree Size", p_partNodes.arraySize);
                    countChange = c.changed;
                }
                if (countChange) {
                    if (its.partNodes.Count > count) {
                        //ChangeArraySizeDirectly (= remove nodes)
                        p_partNodes.arraySize = count;
                        serializedObject.ApplyModifiedProperties ();
                        //Search all PartNodes's childIds and delete removed Ids
                        CleanChildId (its.rootNode);
                        foreach (PartNode pn in its.partNodes)
                            CleanChildId (pn);
                    } else {
                        var p_I = serializedObject.FindProperty ("rootNode.childIds");
                        var p_H = serializedObject.FindProperty ("rootNode.childHides");
                        while (count > p_partNodes.arraySize) {
                            p_partNodes.InsertArrayElementAtIndex (p_partNodes.arraySize - 1);
                            p_partNodes.FindPropertyRelative ("Array.data[" + (p_partNodes.arraySize - 1) + "].parentId").intValue = -1;
                            p_I.InsertArrayElementAtIndex (p_I.arraySize - 1);
                            p_I.GetArrayElementAtIndex (p_I.arraySize - 1).intValue = p_partNodes.arraySize - 1;
                            p_H.InsertArrayElementAtIndex (p_H.arraySize - 1);
                            p_H.GetArrayElementAtIndex (p_H.arraySize - 1).boolValue = true;
                        }
                    }
                    serializedObject.ApplyModifiedProperties ();
                }
            }
        }

        void CleanChildId (PartNode pn)
        {
            for (int i = pn.childIds.Count - 1; i >= 0; i--) {
                if (pn.childIds [i] >= p_partNodes.arraySize) {
                    pn.childIds.RemoveAt (i);
                    pn.childHides.RemoveAt (i);
                }
            }
        }
        #endregion

        #region SetParentTool
        int spsId = -1;

        public int setParentSelfId {
            get {
                spsId = Mathf.Clamp (spsId, 0, its.partNodes.Count - 1);
                return spsId;
            }
            set {
                spsId = Mathf.Clamp (value, 0, its.partNodes.Count - 1);
            }
        }

        int sppId = -1;

        public int setParentNewId {
            get {
                sppId = Mathf.Clamp (sppId, -1, its.partNodes.Count - 1);
                if (sppId == spsId)
                    sppId = -1;
                return sppId;
            }
            set {
                sppId = Mathf.Clamp (value, -1, its.partNodes.Count - 1);
                if (sppId == spsId)
                    sppId = -1;
            }
        }

        void SetParentTool ()
        {
            int oldPid = its.GetNode (setParentSelfId).parentId;
            using (var h = new EditorGUILayout.HorizontalScope ("helpbox")) {
                GUILayout.Label ("Child Part", GUILayout.ExpandWidth (false));
                setParentSelfId = EditorGUILayout.IntField (setParentSelfId, GUILayout.Width (20));
                GUILayout.Label (": Parent [" + oldPid + "]  ->", GUILayout.ExpandWidth (false));
                setParentNewId = EditorGUILayout.IntField (setParentNewId, GUILayout.Width (20));
                if (GUILayout.Button ("Go"))
                    RelinkParent (setParentSelfId, setParentNewId);
            }
        }

        void RelinkParent (int selfId, int newParentId)
        {
            //GetNode
            PartNode selfNode = its.GetNode (selfId);
            PartNode oldPNode = its.GetNode (selfNode.parentId);
            PartNode newPNode = its.GetNode (newParentId);
            //old parent Node remove selfId
            for (int i = oldPNode.childIds.Count - 1; i >= 0; i--) {
                if (oldPNode.childIds [i] == selfId) {
                    oldPNode.childIds.RemoveAt (i);
                    oldPNode.childHides.RemoveAt (i);
                }
            }
            //new parent Node add selfId
            if (!newPNode.childIds.Contains (selfId)) {
                newPNode.childIds.Add (selfId);
                newPNode.childHides.Add (true);
            }
            //fix self Node
            selfNode.parentId = newParentId;
        }
        #endregion

        void DrawTree (PartNode parentNode)
        {
            if (parentNode.childIds.Count > 0) {
                foreach (int i in parentNode.childIds) {
                    DrawNode (i);
                    if (i < its.partNodes.Count && its.partNodes [i].showChild) {
                        EditorGUI.indentLevel++;
                        DrawTree (its.partNodes [i]);
                        EditorGUI.indentLevel--;
                    }
                }
            }
        }

        void DrawNode (int index)
        {
            PartNode n = its.GetNode (index);
            if (n == null)
                return;
            if (simple) {
                EditorGUI.indentLevel += 2;
                EditorGUILayout.LabelField (n.part != null ? n.part.name : "None", GUILayout.Width (200));
                EditorGUI.indentLevel -= 2;
                Rect r = GUILayoutUtility.GetLastRect ();
                EditorGUI.LabelField (r, index.ToString (), EditorStyles.boldLabel);
                if (n.childIds.Count > 0)
                    n.showChild = EditorGUI.Foldout (r, n.showChild, "");
                bool pI_Show = n.part_Instance != null && n.part_Instance.activeInHierarchy;
                r = EditorGUI.IndentedRect (r);
                Rect ri = new Rect (r.x + r.height + 2, r.y + 2, r.height - 3, r.height - 3);
                GUI.Box (ri, "", pI_Show ? "WinBtnMinMac" : "WinBtnInactiveMac");
            } else {
                EditorGUILayout.PropertyField (p_partNodes.GetArrayElementAtIndex (index));
                Rect r = GUILayoutUtility.GetLastRect ();
                EditorGUI.LabelField (r, index.ToString (), EditorStyles.boldLabel);
                if (n.childIds.Count > 0)
                    n.showChild = EditorGUI.Foldout (r, n.showChild, "");
            }
        }

    }
}
