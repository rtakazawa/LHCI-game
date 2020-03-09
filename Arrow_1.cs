using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_1 : MonoBehaviour
{
    private bool _onScreen = true;
    [SerializeField] public float _timeOnScreen = 4.0f;
    private Rigidbody2D m_Rigidbody2D;

    private Quaternion _orto;
    private Vector2 _mPos;


    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_Rigidbody2D.velocity.x > 1f && m_Rigidbody2D.velocity.y > 1f)
        {
            transform.rotation = _orto * Quaternion.Euler(0, 90f, 0);
            _orto = Quaternion.LookRotation(m_Rigidbody2D.velocity);
        }

        else;
        {
            StartCoroutine(ArrowTime());
        }

    }

  
    IEnumerator ArrowTime()
    {
        while (_onScreen == true)
        {
            yield return new WaitForSeconds(_timeOnScreen);
            _onScreen = false;
            Destroy(this.gameObject);           
        }
    }
}