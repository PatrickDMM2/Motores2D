using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteWalk : MonoBehaviour
{
    public List<Sprite> _anim1;
    public float _timePerFrame = 1;
    int _currentFrame = 0;
    float _currentTime = 0;
    SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _anim1[0];
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _timePerFrame) 
        {
            _currentTime = 0;
            _currentFrame++;
            if (_currentFrame >= _anim1.Count) 
            { 
                _currentFrame = 0;
            }
        }
        _spriteRenderer.sprite = _anim1[_currentFrame];
    }
}
