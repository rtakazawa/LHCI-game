using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


    [RequireComponent(typeof(Player))]
public class User_control : MonoBehaviour
{
    private Player m_Character;
    private bool m_Jump;


    private void Awake()
    {
        m_Character = GetComponent<Player>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }
    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        bool attack = Input.GetKeyDown(KeyCode.Mouse0);
        bool aim = Input.GetKeyDown(KeyCode.Mouse1);
        bool noAim = Input.GetKeyUp(KeyCode.Mouse1);

        float m_h = CrossPlatformInputManager.GetAxis("Horizontal");
        bool m_ranged = Input.GetKeyDown(KeyCode.Mouse0);
        bool mount = Input.GetKeyDown(KeyCode.F);
        bool interact = Input.GetKeyDown(KeyCode.E);


        // Pass all parameters to the character control script.
        m_Character.Interact(interact);
        m_Character.Move(h, crouch, m_Jump);
        m_Character.Attack(attack, aim, noAim);
        m_Character.Mount(mount, m_h, m_ranged);
        m_Jump = false;
    }
}

