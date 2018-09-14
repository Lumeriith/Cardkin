using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLance : MonoBehaviour {
    private new Rigidbody rigidbody;
    public float shootForce = 200f;
    public float slowAmount = 0.1f;
    public float slowDuration = 4f;
    public float lifetime = 4f;
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
            victim.ApplySlow(slowAmount, slowDuration);
            victim.health -= 10;
            victim.ApplyCameraShake(0.2f);
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
