using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Goal : MonoBehaviour
{
    [SerializeField] private bool _isUpsideDown;

    private SpriteRenderer _sr;
    public bool goalCleared {get; private set;} = false;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if(player != null && player.isUpsideDown == _isUpsideDown)
        {
            var color = _sr.color;
            color.a = 1.0f;
            _sr.color = color;
            goalCleared = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if(player != null && player.isUpsideDown == _isUpsideDown)
        {
            var color = _sr.color;
            color.a = 0.5f;
            _sr.color = color;
            goalCleared = false;
        }
    }
}
