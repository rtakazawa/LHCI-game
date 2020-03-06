using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] public Transform _target;
    [SerializeField] public float _walkSpeed = 3.0f;  
    [SerializeField] public float _walkDistance = 3.0f;
    [SerializeField] public float _runSpeed = 9.0f;
    [SerializeField] public float _runDistance = 10.0f;

    private bool _returning = false;
    private bool _invisible;
    private float _distance;
    private Vector3 _heading;
    private Vector3 _direction;

    private bool m_FacingRight = true;
    private Rigidbody2D _rigidbody2D;

    private Animator _anim;
    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animation is NULL");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody2D == null)
        {
            Debug.LogError("The Rigidbody2D is NULL");
        }
    }

    private void Update()
    {
        Movement();
        _heading = _target.position - this.gameObject.transform.position;
        _distance = _heading.magnitude;
        _direction = _heading / _distance;

        if (_invisible)
        {
            transform.position = new Vector3(0, -50, 0);
        }
    }

    // Update is called once per frame
    void Movement()
    {
        
        // Walk towards player
        if (_distance > _walkDistance && _distance < _runDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _walkSpeed * Time.deltaTime);
            
            _anim.SetFloat("Speed", _walkSpeed);
         
        }
        
        // Run towards player
        else if (_distance > _runDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _runSpeed * Time.deltaTime);
            _anim.SetFloat("Speed", _runSpeed);
        }

        // Idle if near player
        else if(_distance <= _walkDistance)
        {

            _anim.SetFloat("Speed", 0);
        }

        // Check direction, flip
        if (_direction.x > 0 && !m_FacingRight)
        {    
            Flip();
        }

        else if (_direction.x < 0 && m_FacingRight)
        {            
            Flip();
        }

    }

    public void Mount()
    {
        // Check proximity, mount the player
        if (_distance < 1.0f)
        {
            _player.Mounted();
            StartCoroutine(MountRoutine());
        }

        // Move towards player
        else
        {
            transform.position = Vector2.MoveTowards(_target.transform.position, _target.transform.position, _walkSpeed * Time.deltaTime);
            _anim.SetFloat("Speed", _walkSpeed);
        }
    }

    public void Unmount()
    {
        _returning = true;
        StartCoroutine(UnmountRoutine());
    }

    IEnumerator MountRoutine()
    {
        // Hide game object
        while (!_invisible)
        {    
            yield return new WaitForSeconds(0.3f);
            _invisible = true;
        }
    }

    IEnumerator UnmountRoutine()
    {
        // Bring back game object
        while (_returning)
        {
            _invisible = false;
            yield return new WaitForSeconds(1.0f);

            // Calculate respawn position
            var _posY = _player.transform.position.y;
            var _sumY = _posY + 1.5f;
            transform.position = new Vector3(_player.transform.position.x, _sumY, 0);
            yield return new WaitForSeconds(0.3f);

            _returning = false;
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
