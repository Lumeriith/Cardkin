using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private new Rigidbody rigidbody;
    public float shootForce = 200f;
    public float stunDuration = 2f;
    public float lifetime = 10f;
    public GameObject destroyEffect;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(0, 0, shootForce);
        StartCoroutine("Lifetick");
    }

    IEnumerator Lifetick()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
        if (destroyEffect != null)
        {
            ParticleSystem particle = Instantiate(destroyEffect, transform.position, transform.rotation).GetComponent<ParticleSystem>();
            DestroyObject(particle.gameObject, particle.main.duration);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Player victim = other.GetComponent<Player>();
        if (victim != null)
        {
            victim.ApplyStun(stunDuration);
            victim.health -= 20;
            victim.ApplyCameraShake(0.5f);
        }
        StopCoroutine("Lifetick");
        Destroy(gameObject);
        if (destroyEffect != null)
        {
            ParticleSystem particle = Instantiate(destroyEffect, transform.position, transform.rotation).GetComponent<ParticleSystem>();
            DestroyObject(particle.gameObject, particle.main.duration);
        }
    }
}
