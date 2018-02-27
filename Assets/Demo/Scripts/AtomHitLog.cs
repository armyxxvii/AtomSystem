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
            Atom _hitPart = c.gameObject.GetComponent<Atom> ();
            if (_hitPart != null) {
                CheckHit (_hitPart);
            }
        }

        void OnTriggerEnter (Collider c)
        {
            Atom _hitPart = c.gameObject.GetComponent<Atom> ();
            if (_hitPart != null) {
                CheckHit (_hitPart);
            }
        }

        void CheckHit (Atom _hitPart)
        {
            RequestType[] rts = AtomHelper.Collision (_hitPart, GetComponent<Atom> ());
            string log = "<b>" + name + " is hitted by " + _hitPart.name + " and get following reaction :</b>\n";
            foreach (RequestType r in rts)
                log += "[" + (int)r + "]" + r + /*" (" + (RequestTypeName)r + ")" + */"\n";
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