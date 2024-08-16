using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public float _speed = 20f;
    public int _damage = 10;
    public Rigidbody2D _rb;

    void Start()
    {
        _rb.velocity = transform.right * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
