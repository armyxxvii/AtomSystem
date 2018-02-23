using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreAtom
{
	public class AtomHitLog : MonoBehaviour
    {
        [Header ("範圍")]
        [Tooltip ("自動抓取\"第一個\"Trigger")]
        public Collider[] triggers;

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
            RequestType[] rts = AtomHelper.Collision (GetComponent<ItemPart>(), _hitPart);
            string log = "<b>" + name + " is hitted by " + _hitPart.name + " and get following reaction :</b>\n";
            foreach (RequestType r in rts)
                log += "[" + (int)r + "]" + r + " (" + RequestTypeName.names [(int)r] + ")\n";
            Debug.Log (log);
            SendMessage ("Request", rts, SendMessageOptions.DontRequireReceiver);
        }

        void Awake ()
        {
            if (triggers == null) {
                triggers = GetComponents<Collider> ();
            }
        }

    }
}
