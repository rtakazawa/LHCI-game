using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Civil_NPC : MonoBehaviour
{
    [System.Serializable]
    public enum _NPCtype
    {
        Rhino = 1,
        Panther = 2,
    };

    public _NPCtype _npcType;
    private bool _active;
    private int _interactionID;
    private Player _player;
    private Karma _karma;
    private Text _txt;
    private int _dialogueID;
    

    private void Awake()
    {
        _txt = transform.Find("NPC_canvas/Text").GetComponent<Text>();
        if (_txt == null)
        {
            Debug.LogError("NPC cannot communicate with its Text");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("NPC cannot communicate with PLAYER");
        }

        _karma = GameObject.Find("Player").GetComponent<Karma>();
        if (_karma == null)
        {
            Debug.LogError("NPC cannot communicate with Player's KARMA");
        }
    }

    public void InteractionID()
    {
        switch(_npcType)
        {
            case _NPCtype.Panther:
                var _attitude1 = _karma._panther;
                var _divide1 = 25;
                var _value1 = _attitude1 / _divide1;
                _interactionID = _value1;
                break;

            case _NPCtype.Rhino:
                var _attitude2 = _karma._rhino;
                var _divide2 = 25;
                var _value2 = _attitude2 / _divide2;
                _interactionID = _value2;
                break;
        }
        Debug.Log("Civil_NPC interaction ID value is: " + _interactionID);
    }


    public void Interaction()
    {
        InteractionID();
        _active = true;

        Debug.Log("Interacted with NPC, case:" + _interactionID);
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
        while (_active)
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
        while (_active)
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
        while (_active)
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
        while (_active)
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
        while (_active)
        {
            Debug.Log("Hostile message");

            _player.FriendlyInteraction();

            yield return new WaitForSeconds(1.5f);
            // *salute animation*
            _txt.text = "Run bitch";
            yield return new WaitForSeconds(4.0f);
            _txt.text = "";
            _active = false;
        }

    }
}
