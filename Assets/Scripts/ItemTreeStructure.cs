using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PartNode
{
    public ItemPart part;
    [System.NonSerialized] public GameObject partInstance;
    public int parentId;
    public bool hideChild;
    public List<int> childIds;
}

public class ItemTreeStructure : MonoBehaviour
{
    public PartNode rootNode;
    public List<PartNode> partNodes;
    [System.NonSerialized]public bool isGenerateComplate;

    void Start ()
    {
        foreach (PartNode pn in partNodes) {
            if (pn.partInstance)
                GameObject.Destroy (pn.partInstance);
        }
        rootNode.partInstance = gameObject;
        isGenerateComplate = GeneratePart (rootNode, rootNode);
    }

    bool GeneratePart (PartNode _partNode, PartNode _rootNode)
    {
        if (_partNode.part) {
            _partNode.partInstance = GameObject.Instantiate (_partNode.part.gameObject);
            _partNode.partInstance.transform.parent = _rootNode.partInstance.transform;
            _partNode.partInstance.SetActive (!_rootNode.hideChild);
            if (_partNode.part.appearance) {
                GameObject _app = GameObject.Instantiate (_partNode.part.appearance);
                _app.transform.parent = _partNode.partInstance.transform;
            }
        }
        foreach (int cId in _partNode.childIds) {
            GeneratePart (partNodes [cId], _partNode);
        }
        return true;
    }
}
