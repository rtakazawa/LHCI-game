using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civil_Radar : MonoBehaviour
{
    private Civil_NPC _parent;

    // Start is called before the first frame update
    void Start()
    {
        _parent = transform.parent.GetComponent<Civil_NPC>();
        if (_parent == null)
        {
            Debug.LogError("Civil_Radar cannot communicate with Civil_NPC");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerDetector")
        {
            _parent.Interaction();
        }
    }
}
