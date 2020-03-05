using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] public float _speed;
    [SerializeField] public float _destroyAt = 2000f;
    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(5f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x > 1500f)
        {
            Destroy(this.gameObject);

        }
    }
}
