using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace CreAtom
{
    [RequireComponent (typeof(Collider))]
    [System.Serializable]
    public class ItemPart : MonoBehaviour
    {
        public ItemTreeStructure item;
        [System.NonSerialized]public int id;

        [Header ("造型")]
        public GameObject appearance;

        [Header ("範圍")]
        [Tooltip ("自動抓取\"第一個\"Trigger")]public Collider trigger;

        [Header ("成分")]
        public List<Atom> atoms;

        void Start ()
        {
            if (trigger == null) {
                trigger = GetComponent<Collider> ();
            }
            if (gameObject.activeInHierarchy) {
                RegisterAtoms ();
                if (isLifeDecrease) {
                    deadTime = defaultLife;
                    StartCoroutine (DecreaseLife ());
                }
            }
        }

        #region AtomManage

        void AddAtom (Atom _atom)
        {
            if (!atoms.Exists (atom => atom.name == _atom.name))
                atoms.Add (_atom);
            RegisterAtoms ();
        }

        void RemoveAtom (Atom _atom)
        {
            atoms.RemoveAll (atom => atom.name == _atom.name);
            RegisterAtoms ();
        }

        void RegisterAtoms ()
        {
            isCheckEmpty = false;
            isLifeDecrease = false;
            isSolid = false;

            foreach (Atom a in atoms) {
                isCheckLife |= a.family == AtomFamily.Life;
                isCheckEmpty |= a.family == AtomFamily.Life;
                isLifeDecrease |= a.family == AtomFamily.Life;
                isSolid |= a.family == AtomFamily.Solid;
            }
        }

        #endregion

        #region LifeEvent

        [System.NonSerialized]public bool isCheckLife;
        [System.NonSerialized]public bool isLifeDecrease;
        [System.NonSerialized]public bool isCheckEmpty;
        public float defaultLife = 5;
        float deadTime;

        public void CheckPartLife ()
        {
            if (isCheckEmpty) {
                bool isempty = true;
                foreach (int i in item.GetNode(id).childIds) {
                    if (item.GetNode (i).part_Instance)
                        isempty = false;
                }
                if (isempty) {
                    Debug.Log ("<" + GetInstanceID () + "><color=red>" + name + "</color> was destroyed because of no childs \n");
                    DestroySelf ();
                }
            }
        }

        IEnumerator DecreaseLife()
        {
            while (isLifeDecrease) {
                deadTime -= Time.deltaTime;
                if(deadTime<0){
                    Debug.Log ("<" + GetInstanceID () + "><color=red>" + name + "</color> was destroyed because of life end \n");
                    DestroySelfAndChild ();
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        void DestroySelf ()
        {
            if (item)
                item.DestroyPart (this);
            else
                Destroy (gameObject);
        }

        void DestroySelfAndChild ()
        {
            if (item)
                item.DestroyPart (this, true);
            else
                Destroy (gameObject);
        }

        #endregion

        #region HitEvent

        [System.NonSerialized]public bool isSolid;

        void OnCollisionEnter (Collision c)
        {
            ItemPart _hitPart = c.gameObject.GetComponent<ItemPart> ();
            if (_hitPart != null) {
                CheckHit (_hitPart);
                CheckPartLife ();
//                if (item != null)
//                    item.UpdateParts ();
            }
        }

        void OnTriggerEnter (Collider c)
        {
            ItemPart _hitPart = c.gameObject.GetComponent<ItemPart> ();
            if (_hitPart != null) {
                CheckHit (_hitPart);
                CheckPartLife ();
//                if (item != null)
//                    item.UpdateParts ();
            }
        }

        void CheckHit (ItemPart _hitPart)
        {
            if (!isSolid)
                return;
            bool destroyFlag = false;
            foreach (var atomA in atoms) {
                foreach (Atom atomB in _hitPart.atoms) {
                    destroyFlag |= atomA.Hit (atomB) == AtomResult.destroy;
                }
            }

            if (destroyFlag) {
                Debug.Log ("<" + GetInstanceID () + "><color=red>" + name + "</color> was destroyed by " +
                "<" + _hitPart.GetInstanceID () + "><color=orange>" + _hitPart.name + "</color>\n");
                DestroySelf ();
            }
        }

        #endregion

    }
}
