using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armed_Radar : MonoBehaviour
{
    private Armed_NPC _parent;

    // Start is called before the first frame update
    void Start()
    {
        _parent = transform.parent.GetComponent<Armed_NPC>();
        if (_parent == null)
        {
            Debug.LogError("Armed_Radar cannot communicate with Armed_NPC");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerDetector")
        {
            _parent.Interaction();
        }

        if (other.tag == "Player")
        {
            _parent.Guard();
        }
    }
}
