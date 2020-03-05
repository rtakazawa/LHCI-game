using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mounted_arrow_1 : MonoBehaviour
{

    [SerializeField] private float _forceX = 800.0f;
    [SerializeField] private float _forceY = 30.0f;
    private bool _onScreen = true;
    private bool _onFlight = true;
    [SerializeField] public float _timeOnScreen = 10.0f;
    private Rigidbody2D m_Rigidbody2D;

    private Quaternion _orto;


    private void Awake()
    {
        StartCoroutine(ArrowFlight());
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.AddForce(new Vector2(_forceX, _forceY));

    }

    void Update()
    {
        transform.rotation = _orto * Quaternion.Euler(0, 90f, 0);
        _orto = Quaternion.LookRotation(m_Rigidbody2D.velocity);

    }


    IEnumerator ArrowFlight()
    {
        while (_onScreen == true)
        {
            yield return new WaitForSeconds(_timeOnScreen);
            _onScreen = false;
            Destroy(this.gameObject);
        }
    }
}
