using UnityEngine;
using System.Collections.Generic;

namespace CreAtom
{
    [RequireComponent (typeof(Collider))]
    [System.Serializable]
    public class ItemPart : MonoBehaviour
    {
        [System.NonSerialized]public int id;

        [Header ("造型")]
        public GameObject appearance;

        [Header ("範圍")]
        [Tooltip ("自動抓取\"第一個\"Trigger")]
        public Collider[] triggers;

        [Header ("成分")]
        public List<Atom> atoms;

        void Start ()
        {
            if (triggers == null) {
                triggers = GetComponents<Collider>();
            }
        }

        #region HitEvent

        [System.NonSerialized]public bool isSolid;

        void OnCollisionEnter (Collision c)
        {
            ItemPart _hitPart = c.gameObject.GetComponent<ItemPart> ();
            if (_hitPart != null) {
                CheckHit (_hitPart);
            }
        }

        void OnTriggerEnter (Collider c)
        {
            ItemPart _hitPart = c.gameObject.GetComponent<ItemPart> ();
            if (_hitPart != null) {
                CheckHit (_hitPart);
            }
        }

        void CheckHit (ItemPart _hitPart)
        {
            RequestType[] rts = AtomHelper.Collision (this, _hitPart);
            string log = "<b>" + name + " is hitted by " + _hitPart.name + " and get following reaction :</b>\n";
            foreach (RequestType r in rts)
                log +="[" + (int)r + "]"+ r + " ("+RequestTypeName.names[(int)r] + ")\n";
            Debug.Log (log);
            SendMessage ("Request", rts, SendMessageOptions.DontRequireReceiver);
        }

        #endregion

    }
}
