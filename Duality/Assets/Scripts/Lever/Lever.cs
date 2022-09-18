using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] List<GameObject> _targets;
    [SerializeField] Sprite _leverUpSprite;
    [SerializeField] Sprite _leverDownSprite;
    
    private bool _isLeverUp = true;

    private SpriteRenderer _sr;
    private AudioSource _audioSource;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if(_isLeverUp)
        {
            _isLeverUp = false;
            _sr.sprite = _leverDownSprite;
        }
        else
        {
            _isLeverUp = true;
            _sr.sprite = _leverUpSprite;
        }

        foreach(var go in _targets)
        {
            go.SetActive(!go.activeSelf);
        }

        _audioSource.Play();
    }
}
