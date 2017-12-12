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

        void Start ()
        {
            Backup ();
            rootNode.part_Instance = gameObject;
            ClearPartInstance ();
            GeneratePart (rootNode, rootNode);
            for (int i = 0; i < partNodes.Count; i++) {
                partNodes [i].part.id = i;
            }
        }

        public void Reset ()
        {
            ClearPartInstance ();
            Restore ();
            rootNode.part_Instance = gameObject;
            GeneratePart (rootNode, rootNode);
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

        public bool isGenerateComplate ()
        {
            bool check = true;
            foreach (var pn in partNodes)
                check &= pn.part_Instance != null;
            return check;
        }

        void ClearPartInstance ()
        {
            foreach (PartNode pn in partNodes) {
                if (pn.part_Instance)
                    GameObject.Destroy (pn.part_Instance);
            }
        }

        void GeneratePart (PartNode _partNode, PartNode _rootNode, bool _isHide = false)
        {
            //Generate _partNode
            if (_partNode.part) {
                Debug.Log ("Generate Part : " + _partNode.part.name+"\n");
                // Art inside Part
                _partNode.part_Instance = CreatePart (_partNode.Tpos, _partNode.Trot, _partNode.part.gameObject, _rootNode.part_Instance);
                _partNode.part_Instance.SetActive (!_isHide);
                _partNode.part = _partNode.part_Instance.GetComponent<ItemPart> ();
                _partNode.part.item = this;

                if (_partNode.part.appearance) {
                    _partNode.app_Instance = CreatePart (Vector3.zero, Vector3.zero, _partNode.part.appearance, _partNode.part_Instance);
                    _partNode.app_Instance.SetActive (!_isHide);
                }

                // Part inside Art
//                if (_partNode.part.appearance) {
//                    _partNode.app_Instance = Create (_partNode.part.appearance, _rootNode.app_Instance);
//                    _partNode.app_Instance.SetActive (!_rootNode.hideChild);
//                }
//                _partNode.part_Instance = Create (_partNode.part.gameObject, _partNode.app_Instance);
//                _partNode.part = _partNode.part_Instance.GetComponent<ItemPart> ();
//                _partNode.part_Instance.SetActive (!_rootNode.hideChild);
            }

            //Generate _partNode.Childs
            for (int i = 0; i < _partNode.childIds.Count; i++) {
                PartNode cNode = partNodes [_partNode.childIds[i]];
                bool cHide = _partNode.childHides [i];
                GeneratePart (cNode, _partNode, cHide);
            }
        }

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

        public void DestroyPart (ItemPart _part)
        {
            //get self partnode & id
            int id = partNodes.FindIndex (p => p.part == _part);
            PartNode selfNode = id < 0 ? rootNode : partNodes [id];

            //get parent partnode & id
            int pid = id < 0 ? -1 : selfNode.parentId;
            PartNode parentNode = pid < 0 ? rootNode : partNodes [pid];

            //relink child to parent
            foreach (int i in selfNode.childIds) {
                if (!partNodes [i].part_Instance) {
                    Debug.LogWarning (partNodes [i].part.name + "[" + i + "] missing!!!");
                    continue;
                }
                if (!parentNode.childIds.Contains (i))
                    parentNode.childIds.Add (i);
                partNodes [i].parentId = pid;
                partNodes [i].part_Instance.transform.parent = parentNode.part_Instance.transform;
                partNodes [i].app_Instance.SetActive (true);
                partNodes [i].part_Instance.SetActive (true);
            }

            // clear self
            parentNode.childIds.Remove (id);
            Destroy (selfNode.part_Instance);
            selfNode.part_Instance = null;
            selfNode.app_Instance = null;

            UpdateParts ();
        }

    }
}
