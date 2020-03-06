using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float _mountedSpeed = 15f;
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 2)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private LayerMask[] _whatIsInteractuable;            // 0 = NPC, 1 = Object

    


    private Companion _companion;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Transform _interactionCheck;
    [SerializeField] const float k_NPCinteractionRadius = 1f;
    [SerializeField] const float k_WOinteractionRadius = 3f;
    public bool _NPCinteraction;
    public bool _WOinteraction;
    private GameObject _detector;
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    [SerializeField] private GameObject _MeleeWeapon;
    [SerializeField] private GameObject _RangedWeapon;
    [SerializeField] private GameObject _MountedRangedWeapon;

    [SerializeField] private GameObject _FunBall;

    [SerializeField] public float _MeleeCooldown = 2.0f;
    [SerializeField] public float _RangedCooldown = 1.5f;
    [SerializeField] public float _InteractionCooldown = 3.0f;

    private bool _attack = false;
    private bool _aiming = false;
    private bool _cooldown = false;
    private bool _interacting = false;
    private bool _mounted = false;



    private void Awake()
    {
        // Setting up references.
        _interactionCheck = transform.Find("Checkers/InteractionCheck");
        m_GroundCheck = transform.Find("Checkers/GroundCheck");
        m_CeilingCheck = transform.Find("Checkers/CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        _companion = GameObject.Find("Companion").GetComponent<Companion>();
        if (_companion == null)
        {
            Debug.LogError("The Companion is NULL");
        }

        _detector = GameObject.Find("Player/Checkers/Detector");
        if (_detector == null)
        {
            Debug.LogError("The Detector is NULL");
        }
            


        
    }

    private void FixedUpdate()
    {


        Collider[] NPCdetector = Physics.OverlapSphere(_interactionCheck.position, k_NPCinteractionRadius, _whatIsInteractuable[0]);
        for (int i = 0; i < NPCdetector.Length; i++)
        {
            if (NPCdetector[i].gameObject != gameObject)
            {
                _NPCinteraction = true;
            }
        }

        Collider[] WOdetector = Physics.OverlapSphere(_interactionCheck.position, k_WOinteractionRadius, _whatIsInteractuable[1]);
        for (int i = 0; i < WOdetector.Length; i++)
        {
            if (WOdetector[i].gameObject != gameObject)
            {
                _WOinteraction = true;
                Debug.Log("World Object detected");
            }
        }

        m_Grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
          if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

        m_Anim.SetBool("Mounted", _mounted);

        if (Input.GetKey(KeyCode.P))
        {
            Instantiate(_FunBall, transform.position, Quaternion.identity);
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch") && !_mounted)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded && !_mounted && !_aiming && !_cooldown || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move*m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    public void Karma()
    {
        
    }

    public void Interact(bool interact)
    {
        if (_NPCinteraction && interact)
        {
            _detector.SetActive(true);
            _cooldown = true;
            StartCoroutine(InteractCooldownRoutine());
        }
    }

    IEnumerator InteractCooldownRoutine()
    {
        while (_cooldown)
        {
            yield return new WaitForSeconds(_InteractionCooldown);
            _detector.SetActive(false);
            _cooldown = false;
        }
    }

    public void FriendlyInteraction()
    {
        _interacting = true;
        StartCoroutine(FriendlyInteractionRoutine());
    }

    IEnumerator FriendlyInteractionRoutine()
    {
        while (_interacting)
        {
            yield return new WaitForSeconds(0.2f);
            m_Anim.SetBool("HoodOff", true);

            // *interaction event*

            yield return new WaitForSeconds(2.0f);
            m_Anim.SetBool("HoodOff", false);
            _interacting = false;
        }
    }


    public void Attack(bool attack, bool aim, bool noAim)
    {

        // Melee attack
        if (!_mounted && m_Grounded && attack && !_cooldown && !_attack && !_aiming)
        {
            _attack = true;
            StartCoroutine(MeleeRoutine());
            m_Anim.SetTrigger("Attack");
        }       

        // Aiming bow
        if (!_mounted && m_Grounded && aim && !_cooldown && !_cooldown && !_attack && !_aiming)
        {
            _aiming = true;
            
            m_Anim.SetBool("Aim", true);

            Debug.Log("Player is aiming");
        }

        // Shoot arrow
        if (!_mounted && !_cooldown && _aiming && attack)
        {
            m_Anim.SetTrigger("Attack");
            _attack = true;
            StartCoroutine(RangedRoutine());
            
        }

        // Stop aiming bow
        if (!_mounted && !_cooldown && noAim)
        {
            m_Anim.SetBool("Aim", false);
            _cooldown = true;
            StartCoroutine(RangedCooldownCoroutine());
            _aiming = false;

            Debug.Log("Player is NOT aiming");
        }
    }

    IEnumerator MeleeRoutine()
    {      
        while (_attack)
        {         
            _cooldown = true;
            StartCoroutine(CooldownCoroutine());

            yield return new WaitForSeconds(0.5f);
            Instantiate(_MeleeWeapon, transform.position, Quaternion.identity, gameObject.transform);

            _attack = false;
        }
    }

    IEnumerator RangedRoutine()
    {

        while (_attack)
        {
            _cooldown = true;
            StartCoroutine(RangedCooldownCoroutine());

            yield return new WaitForSeconds(0.4f);
            Instantiate(_RangedWeapon, transform.position, Quaternion.identity);

            _attack = false;
        }
    }
  
    IEnumerator CooldownCoroutine()
    {
        while (_cooldown)
        {          
            yield return new WaitForSeconds(_MeleeCooldown);
            m_Anim.ResetTrigger("Attack");
            _cooldown = false;
        }
    }

    IEnumerator RangedCooldownCoroutine()
    {
        while (_cooldown)
        {
            yield return new WaitForSeconds(_RangedCooldown);
            m_Anim.ResetTrigger("Attack");
            _cooldown = false;
        }
    }


    public void Mount(bool mount, float m_h, bool m_ranged)
    {
        if (m_Grounded && mount && !_mounted)
        {
            if (_companion != null)
            {
                _companion.Mount();
            }
            
        }

        if (_mounted && mount)
        {
            StartCoroutine(UnmountingRoutine());
            
            _companion.Unmount();
        }

        if (_mounted)
        {
            m_Rigidbody2D.velocity = new Vector2(m_h * _mountedSpeed, m_Rigidbody2D.velocity.y);
            
        }

        if (_mounted && m_ranged && !_cooldown && !_attack)
        {
            m_Anim.SetTrigger("Attack");
            _attack = true;
            StartCoroutine(MountedRangedRoutine());
        }
    }

    public void Mounted()
    {
        
        StartCoroutine(MountingRoutine());
        m_Anim.SetTrigger("Mounting");
    }

    IEnumerator MountingRoutine()
    {
        while (!_mounted)
        {
            m_AirControl = true;

            yield return new WaitForSeconds(0.4f);
            m_Anim.SetBool("Mounted", true);
            _mounted = true;
            m_Anim.ResetTrigger("Mounting");
            Debug.Log("Player is mounted");
        }
    }

    IEnumerator UnmountingRoutine()
    {
        while (_mounted)
        {
            
            m_Anim.SetTrigger("Unmounting");
            yield return new WaitForSeconds(0.4f);
            
            m_Anim.SetBool("Mounted", false);
            _mounted = false;

            m_AirControl = false;

            m_Anim.ResetTrigger("Unmounting");
            Debug.Log("Player is unmounted");

        }
    }

    IEnumerator MountedRangedRoutine()
    {
        
        while (_attack)
        {
            _cooldown = true;
            StartCoroutine(RangedCooldownCoroutine());

            yield return new WaitForSeconds(0.6f);
            var _posY = transform.position.y;
            var _sumY = _posY + 1.7f;
            var _posXY = new Vector3(transform.position.x, _sumY);
            Instantiate(_MountedRangedWeapon, _posXY, Quaternion.identity);

            _attack = false;
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

