using UnityEngine;
using System.Collections;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField]
    public AudioClip _audioClip;
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player detected");
            _audioSource.clip = _audioClip;
            _audioSource.Play();
        }
    }
}
