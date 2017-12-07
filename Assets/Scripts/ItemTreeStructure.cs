using UnityEngine;
using System.Collections.Generic;

namespace AtomSystem
{

    [System.Serializable]
    public class ItemTreeStructure : MonoBehaviour
    {
        [Header ("本體")]
        public PartNode rootNode;
        [Header ("零件結構")]
        public List<PartNode> partNodes;
        [System.NonSerialized]public bool isGenerateComplate;

        void Start ()
        {
            foreach (PartNode pn in partNodes) {
                if (pn.part_Instance)
                    GameObject.Destroy (pn.part_Instance);
            }
            rootNode.part_Instance = gameObject;
            rootNode.app_Instance = gameObject;
            isGenerateComplate = GeneratePart (rootNode, rootNode);
        }

        bool GeneratePart (PartNode _partNode, PartNode _rootNode)
        {
            if (_partNode.part) {
                // Art in Part
                _partNode.part_Instance = CreatePart (_partNode.part.gameObject, _rootNode.part_Instance);
                _partNode.part_Instance.SetActive (!_rootNode.hideChild);

                _partNode.part = _partNode.part_Instance.GetComponent<ItemPart> ();

                if (_partNode.part.appearance) {
                    _partNode.app_Instance = CreatePart (_partNode.part.appearance, _partNode.part_Instance);
                    _partNode.app_Instance.SetActive (!_rootNode.hideChild);
                }

                // Part in Art
//                if (_partNode.part.appearance) {
//                    _partNode.app_Instance = Create (_partNode.part.appearance, _rootNode.app_Instance);
//                    _partNode.app_Instance.SetActive (!_rootNode.hideChild);
//                }
//                _partNode.part_Instance = Create (_partNode.part.gameObject, _partNode.app_Instance);
//                _partNode.part = _partNode.part_Instance.GetComponent<ItemPart> ();
//                _partNode.part_Instance.SetActive (!_rootNode.hideChild);
            }
            //Generate Childs
            foreach (int cId in _partNode.childIds) {
                PartNode c = partNodes [cId];
                GeneratePart (c, _partNode);
            }
            return true;
        }

        GameObject CreatePart (GameObject _source, GameObject _root = null)
        {
            GameObject _instance = GameObject.Instantiate (_source);
            if (_root != null)
                _instance.transform.parent = _root.transform;
            _instance.transform.localPosition = Vector3.zero;
            _instance.transform.localEulerAngles = Vector3.zero;
            _instance.transform.localScale = Vector3.one;
            ItemPart _part = _instance.GetComponent<ItemPart> ();
            if (_part != null)
                _part.item = this;
            return _instance;
        }

        public void DestroyPart(ItemPart _part)
        {
            int id = partNodes.FindIndex (p => p.part == _part);
            PartNode selfNode = id < 0 ? rootNode : partNodes [id];
            int pid = (id < 0 ? rootNode : selfNode).parentId;
            PartNode parentNode = pid < 0 ? rootNode : partNodes [pid];
            parentNode.childIds.Remove (id);
            foreach (int i in selfNode.childIds) {
                if (!parentNode.childIds.Contains (i))
                    parentNode.childIds.Add (i);
                partNodes [i].parentId = pid;
                partNodes [i].part_Instance.transform.parent = parentNode.part_Instance.transform;
                partNodes [i].app_Instance.SetActive (true);
                partNodes [i].part_Instance.SetActive (true);
            }
            Destroy (selfNode.part_Instance);
//            Destroy (selfNode.app_Instance);
        }

    }
}
