using UnityEngine;
using System.Collections.Generic;

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
            if (_partNode.part.appearance) {
                _partNode.app_Instance = Create (_partNode.part.appearance, _rootNode.app_Instance);
                _partNode.app_Instance.SetActive (!_rootNode.hideChild);
            }
            _partNode.part_Instance = Create (_partNode.part.gameObject, _partNode.app_Instance);
            _partNode.part_Instance.SetActive (!_rootNode.hideChild);
        }
        foreach (int cId in _partNode.childIds) {
            PartNode c = partNodes [cId];
            GeneratePart (c, _partNode);
        }
        return true;
    }

    GameObject Create (GameObject _source, GameObject _root = null)
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
}
