using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armed_NPC : MonoBehaviour
{

    [System.Serializable]
    public enum _NPCtype
    {
        Rhino = 1,
        Panther = 2,
    };

    public _NPCtype _npcType;

    [SerializeField] private int _hp;
    [Range (1, 100)] [SerializeField] private float _blockChance;
    [SerializeField] private float _speed;

    private bool m_FacingRight;
    private bool _active;
    private bool _guarded;
    private bool _attack;
    private bool _attackCooldown;
    private bool _damageCooldown;
    private bool _dying;
    private int _interactionID;
    private Animator _animator;
    private Player _player;
    private Karma _karma;
    private Text _txt;
    private Dialogue _dialogue;
    private int _dialogueID;

    private float _distance;
    private Vector3 _heading;
    private Vector3 _direction;

    private void Awake()
    {
        _active = false;
        _guarded = false;
        _attack = false;

        _txt = transform.Find("NPC_canvas/Text").GetComponent<Text>();
        if (_txt == null)
        {
            Debug.LogError("ARMED NPC cannot communicate with its Text");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("ARMED NPC cannot communicate with PLAYER");
        }

        _karma = GameObject.Find("Player").GetComponent<Karma>();
        if (_karma == null)
        {
            Debug.LogError("ARMED NPC cannot communicate with Player's KARMA");
        }

        _animator = transform.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("ARMED NPC cannot communicate with its ANIMATOR");
        }
    }

    public void Update()
    {
        if (_guarded)
        {
            _animator.SetBool("Guarded", true);
        }

        if (_attack)
        {
            Attack();
        }

        _heading = _player.transform.position - transform.position;
        _distance = _heading.magnitude;
        _direction = _heading / _distance;

        // Check direction, flip
      
    }

    public void InteractionID()
    {
        
        switch (_npcType)
        {
            case _NPCtype.Panther:
                var _attitude1 = _karma._panther;
                var _divide1 = 25;
                var _value1 = _attitude1 / _divide1;
                _interactionID = Mathf.RoundToInt(_value1);
                break;

            case _NPCtype.Rhino:
                var _attitude2 = _karma._rhino;
                var _divide2 = 25;
                var _value2 = _attitude2 / _divide2;
                _interactionID = Mathf.RoundToInt(_value2);
                break;
        }
        Debug.Log("Armed_NPC interaction ID result is: " + _interactionID);
    }


    public void Interaction()
    {
        InteractionID();
        _active = true;

        switch (_interactionID)
        {
            default:
                break;

            case 0:
                StartCoroutine(EnemyRoutine());
                break;

            case 1:
                StartCoroutine(CautiousRoutine());
                break;

            case 2:
                StartCoroutine(NeutralRoutine());
                break;

            case 3:
                StartCoroutine(FriendlyRoutine());
                break;

            case 4:
                StartCoroutine(AllyRoutine());
                break;

        }
    }

    IEnumerator AllyRoutine()
    {
        while (_active && !_attack)
        {
            _player.FriendlyInteraction();
            yield return new WaitForSeconds(1.5f);
            // *salute animation*
            _dialogueID = Random.Range(0, 2);
            switch (_dialogueID)
            {
                default:
                    break;

                case 0:
                    _txt.text = "You rock";
                    break;

                case 1:
                    _txt.text = "We love u";
                    break;
            }


            yield return new WaitForSeconds(4.0f);
            _txt.text = "";
            _active = false;
        }

    }

    IEnumerator FriendlyRoutine()
    {
        while (_active && !_attack)
        {
            _player.FriendlyInteraction();

            yield return new WaitForSeconds(1.5f);
            // *salute animation*
            _dialogueID = Random.Range(0, 2);
            switch (_dialogueID)
            {
                default:
                    break;

                case 0:
                    _txt.text = "Hello handsome boi";
                    break;

                case 1:
                    _txt.text = "How u doin?";
                    break;
            }
            yield return new WaitForSeconds(4.0f);
            _txt.text = "";
            _active = false;
        }

    }

    IEnumerator NeutralRoutine()
    {
        while (_active && !_attack)
        {
            _player.FriendlyInteraction();

            yield return new WaitForSeconds(1.5f);
            // *salute animation*
            _txt.text = "Nice sword";
            yield return new WaitForSeconds(4.0f);
            _txt.text = "";
            _active = false;
        }

    }

    IEnumerator CautiousRoutine()
    {
        while (_active && !_attack)
        {
            _player.FriendlyInteraction();

            yield return new WaitForSeconds(1.5f);
            // *salute animation*
            _txt.text = "There are some m'fackas looking for u";
            yield return new WaitForSeconds(4.0f);
            _txt.text = "";
            _active = false;
        }

    }

    IEnumerator EnemyRoutine()
    {
        while (_active && !_attack)
        {

            _player.FriendlyInteraction();

            yield return new WaitForSeconds(1.5f);
            // *salute animation*
            _txt.text = "Run bitch";
            yield return new WaitForSeconds(4.0f);
            _txt.text = "";
            _active = false;
        }

    }

    public void Guard()
    {
        InteractionID();

        switch (_interactionID)
        {
            default:
                break;

            case 0:
                _guarded = true;
                _attack = true;
                break;

            case 1:
                _guarded = true;
                _attack = true;
                break;

            case 2:
                _guarded = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _animator.ResetTrigger("Block");
        _animator.ResetTrigger("Hit");

        if (other.tag == "Attack" && !_dying)
        {
            var chance = -0.1f * _blockChance + 10f;
            int chanceInt = Mathf.RoundToInt(chance); 
            var result = Random.Range(0, chanceInt);

            if (result < 1f )
            {
                Debug.Log("ARMED_NPC blocked attack");
                _animator.SetTrigger("Block");
            }

            if (result >= 1f && !_damageCooldown)
            {
                Damage();
                _animator.SetTrigger("Hit");
                _damageCooldown = true;
                StartCoroutine(DamageCooldownRoutine());
                Debug.Log("Armed_NPC hitted with " + other.transform.name);
            }
        }
    }

 

    public void Attack()
    {
        if (_distance > 1f && !_attackCooldown && !_damageCooldown && !_dying && _attack)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
            _animator.SetFloat("Speed", _speed);
        }

        if (_distance <= 1f && !_attackCooldown && !_damageCooldown && !_dying && _attack)
        {
            _animator.SetTrigger("Attack");
            _attackCooldown = true;
            StartCoroutine(CooldownRoutine());
        }

        if (_direction.x > 0 && !m_FacingRight && !_dying)
        {
            Flip();
        }

        else if (_direction.x < 0 && m_FacingRight && !_dying)
        {
            Flip();
        }
    }

    private void Damage()
    {
        _hp--;

        if (_hp < 1)
        {
            _dying = true;
            StartCoroutine(DeathRoutine());
        }

        if (!_attack)
        {
            _attack = true;
        }
    }

    IEnumerator CooldownRoutine()
    {
        while(_attackCooldown)
        {
            yield return new WaitForSeconds(1.4f);
            _animator.ResetTrigger("Attack");
            _attackCooldown = false;
        }
    }

    IEnumerator DamageCooldownRoutine()
    {
        while (_damageCooldown)
        {
            yield return new WaitForSeconds(1f);
            _damageCooldown = false;
        }
    }
    IEnumerator DeathRoutine()
    {
        while (true)
        {
            _animator.SetTrigger("Dead");
            yield return new WaitForSeconds(5f);
            Destroy(this.gameObject);
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