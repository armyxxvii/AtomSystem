using UnityEngine;
using System.Collections.Generic;

namespace CreAtom
{

    [System.Serializable]
    public class ItemTreeStructure : MonoBehaviour
    {
        [Header ("本體")]
        public PartNode rootNode;
        [System.NonSerialized]PartNode rootNode_backup;
        [Header ("零件結構")]
        public List<PartNode> partNodes;
        [System.NonSerialized]List<PartNode> partNodes_backup;

        public PartNode GetNode(int index)
        {
            if (index < 0)
                return rootNode;
            if (partNodes.Count == 0) {
                partNodes.Add (new PartNode ());
                rootNode.childIds.Add (0);
                rootNode.childHides.Add (true);
            }
            if (index > partNodes.Count - 1)
                return null;
            return partNodes [index];
        }

        bool isInit;
        bool isGenerateComplate;

        public bool IsGenerateComplate {
            get {
                return isGenerateComplate;
            }
        }

        void Start ()
        {
            Restart ();
        }

        public void Restart ()
        {
            if (!isInit) {
                Backup ();
                rootNode.part_Instance = gameObject;
                ClearPartInstance ();
                isInit = true;
            } else {
                ClearPartInstance ();
                Restore ();
                rootNode.part_Instance = gameObject;
            }
            GenerateTree (rootNode, rootNode);
            for (int i = 0; i < partNodes.Count; i++) {
                partNodes [i].part.id = i;
            }
            partNodes [0].part_Instance.SetActive (true);
        }

        void Backup ()
        {
            rootNode_backup = rootNode.Clone ();
            partNodes_backup = new List<PartNode> ();
            foreach (PartNode pn in partNodes)
                partNodes_backup.Add (pn.Clone ());
        }

        void Restore ()
        {
            rootNode = rootNode_backup.Clone ();
            partNodes = new List<PartNode> ();
            foreach (PartNode pn in partNodes_backup)
                partNodes.Add (pn.Clone ());
        }

        void ClearPartInstance ()
        {
            foreach (PartNode pn in partNodes) {
                if (pn.part_Instance)
                    GameObject.Destroy (pn.part_Instance);
            }
        }

        void GenerateTree (PartNode _partNode, PartNode _rootNode, bool _isHide = false)
        {
            //Generate _partNode
            if (_partNode.part) {
                Debug.Log ("Generate Part : " + _partNode.part.name + "\n");
                // Art inside Part
                _partNode.part_Instance = CreatePart (_partNode.Tpos, _partNode.Trot, _partNode.part.gameObject, _rootNode.part_Instance);
                _partNode.part_Instance.SetActive (!_isHide);
                _partNode.part = _partNode.part_Instance.GetComponent<ItemPart> ();
                _partNode.part.item = this;

                if (_partNode.part.appearance) {
                    _partNode.app_Instance = CreatePart (Vector3.zero, Vector3.zero, _partNode.part.appearance, _partNode.part_Instance);
                    _partNode.app_Instance.SetActive (!_isHide);
                }
            }

            //Generate _partNode.Childs
            for (int i = 0; i < _partNode.childIds.Count; i++) {
                PartNode cNode = GetNode(_partNode.childIds [i]);
                bool cHide = _partNode.childHides [i];
                GenerateTree (cNode, _partNode, cHide);
            }
        }

        #region Part
        static GameObject CreatePart (Vector3 _position, Vector3 _rotation, GameObject _source, GameObject _root = null)
        {
            GameObject _instance = GameObject.Instantiate (_source);
            if (_root != null)
                _instance.transform.parent = _root.transform;
            _instance.transform.localPosition = _position;
            _instance.transform.localEulerAngles = _rotation;
            _instance.transform.localScale = Vector3.one;
            return _instance;
        }

        public void DestroyPart (ItemPart _part,bool _destroyChild = false)
        {
            //get self partnode & id
            int id = partNodes.FindIndex (p => p.part == _part);
            PartNode selfNode = GetNode (id);

            //get parent partnode & id
            int pId = id < 0 ? -1 : selfNode.parentId;
            PartNode parentNode = GetNode (pId);

            if (_destroyChild) {
                for (int i = selfNode.childIds.Count - 1; i > -1; i--) {
                    int cId = selfNode.childIds [i];
                    PartNode childNode = GetNode (cId);
                    Destroy (childNode.part_Instance);
                }
            } else {
                //relink child to parent
                foreach (var cId in selfNode.childIds) {
                    PartNode childNode = GetNode (cId);
                    if (childNode == null || !childNode.part_Instance) {
                        Debug.LogWarning ("Node[" + cId + "] is missing!!!");
                        continue;
                    }
                    if (!parentNode.childIds.Contains (cId)) {
                        parentNode.childIds.Add (cId);
                        parentNode.childHides.Add (false);
                    }
                    childNode.parentId = pId;
                    childNode.part_Instance.transform.parent = parentNode.part_Instance.transform;
                    childNode.app_Instance.SetActive (true);
                    childNode.part_Instance.SetActive (true);
                }
            }

            // clear self
            int index = parentNode.childIds.FindIndex (val => val == id);
            if (index > -1) {
                parentNode.childIds.RemoveAt (index);
                parentNode.childHides.RemoveAt (index);
            }
            Destroy (selfNode.part_Instance);
            selfNode.part_Instance = null;
            selfNode.app_Instance = null;
        }

//        public void UpdateParts ()
//        {
//            for (int i = 0; i < partNodes.Count; i++) {
//                GameObject pi = partNodes [i].part_Instance;
//                if (pi != null)
//                    partNodes [i].part.CheckPartLife ();
//            }
//        }
        #endregion
    }
}
