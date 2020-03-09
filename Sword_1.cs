using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_1 : MonoBehaviour
{

    private void Awake()
    {
        StartCoroutine(AliveRoutine());
    }

    IEnumerator AliveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }
    }
}
