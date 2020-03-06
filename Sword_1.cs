using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_1 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 50.0f;
    
    Vector3 currentEulerAngles;

    private void Awake()
    {
        currentEulerAngles = new Vector3(0, 0, 0);
    }
    public void Update()
    {
        currentEulerAngles += new Vector3(0, 0, 2) * Time.deltaTime * _speed;
        transform.eulerAngles = currentEulerAngles;


        if (currentEulerAngles.z >= 240)
        {
            Destroy(this.gameObject);
        }
    }

}
