using System;
using UnityEngine;

public class Arrow : Removal
{
    [NonSerialized] public bool isEnemyShooted = false;
    [NonSerialized] public float damage;
    [NonSerialized] public float speed;

    private Vector3 _velocity;

    public void UpdateVelocity(Vector3 velocity) => _velocity = velocity;

    private void Update()
    {
        _velocity.x -= _velocity.x * Time.deltaTime * 2;
        transform.Translate(speed * Time.deltaTime * (isEnemyShooted ? Vector3.down : Vector3.up) + _velocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isEnemyShooted)
        {
            if (collision.CompareTag("Player"))
            {
                DataManager.Damage(8);
                Destroy(gameObject);
            }
        } else
        { 
            if (collision.CompareTag("Enemy"))
            {
                var entity = collision.GetComponent<Entity>();
                entity.Hit();
                entity.RemoveHealth(35);
                Destroy(gameObject);
            }
        }
    }
}
