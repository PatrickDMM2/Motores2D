using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDisappearPlatform : MonoBehaviour
{
    public float _timetoWait = 5;
    public float _secondstoWait = 1;
    public float _elapseTime = 0;
    public SpriteRenderer _spriteRenderer;
    public Color _color1;
    public Color _color2;
    bool Appear = false;

    public float _timeToTogglePlatfform = 5;
    public float _currentTime = 0;
    public bool enabled = true;
    void Start()
    {
        Appear = enabled;
        StartCoroutine(WaitTime());
    }
    IEnumerator WaitTime()
    {
        float _currentTime = 0;
        var color = _spriteRenderer.color = _color1;
        color.a = 1;
        if (Appear)
        {
            TogglePlatform(true);
        }

        while (_currentTime < _timetoWait)
        {
            if (!Appear)
            {
                color.a = Mathf.Lerp(1, 0, _currentTime / _timetoWait); 
            }
            else
            {
                color.a = Mathf.Lerp(0, 1, _currentTime / _timetoWait); 
            }
            _currentTime += Time.deltaTime;
            _spriteRenderer.color = color;
            yield return null;
        }
        _spriteRenderer.color = _color1;
        TogglePlatform(Appear);
        Appear = !Appear;
        Debug.Log("Se acabo el tiempo");
        yield return new WaitForSeconds(_secondstoWait);
        StartCoroutine(WaitTime());
    }


    void TogglePlatform(bool enabled)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(enabled);
        }
    }
}

