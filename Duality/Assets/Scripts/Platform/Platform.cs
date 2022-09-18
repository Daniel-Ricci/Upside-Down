using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform _pos1;
    [SerializeField] private Transform _pos2;
    [SerializeField] private float _speed;

    private Vector3 _targetPos;

    private void Start()
    {
        _targetPos = _pos1.position;
    }

    void Update()
    {
        if(transform.position == _pos1.position)
        {
            _targetPos = _pos2.position;
        }
        else if(transform.position == _pos2.position)
        {
            _targetPos = _pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);
    }
}
