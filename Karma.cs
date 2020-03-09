using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karma : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] public int _attribute1;        // Example 1
    [Range(0, 100)] [SerializeField] public int _attribute2;        // Example 2
    [Range(0, 100)] [SerializeField] public int _attribute3;        // Example 3

    public int _rhino;
    public int _panther;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        _rhino = _attribute1 - _attribute2;
        _panther = _attribute3 - _attribute2;
    }

}
