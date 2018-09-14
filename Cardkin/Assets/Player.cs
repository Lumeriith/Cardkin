using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float cameraShake;
    public bool isDead;

    public float movementSpeedModifier = 1f;
    public bool isStunned = false;

    public float health = 30f;
    public float maxHealth = 30f;

    private int stunCount = 0;

    public void ApplySlow(float amount, float duration)
    {
        StartCoroutine(Slow(amount, duration));
    }

    public void ApplyStun(float duration)
    {
        StartCoroutine(Stun(duration));
    }

    public void ApplyCameraShake(float amount)
    {
        cameraShake += amount;
    }

    IEnumerator Slow(float amount, float duration)
    {
        movementSpeedModifier *= (1 - amount);
        yield return new WaitForSeconds(duration);
        movementSpeedModifier /= (1 - amount);
    }

    IEnumerator Stun(float duration)
    {
        stunCount++;
        isStunned = true;
        yield return new WaitForSeconds(duration);
        stunCount--;
        if (stunCount == 0)
        {
            isStunned = false;
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ApplySlow(0.3f, 1f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ApplyStun(1f);
        }

    }
    private void FixedUpdate()
    {
        if (stunCount <= 0)
        {
            stunCount = 0;
            isStunned = false;
        }
        else
        {
            isStunned = true;
        }

        if (cameraShake >= 0)
        {
            cameraShake *= 0.94f;
        }
        else if (cameraShake < 0.2)
        {
            cameraShake = 0;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if ((health <= 0) && !isDead)
        {
            isDead = true;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            CapsuleCollider cc = gameObject.AddComponent<CapsuleCollider>();
            rb.AddForce(Random.insideUnitSphere * 200 + transform.up * 400);
            rb.AddTorque(Random.insideUnitSphere * 100);
            GetComponentInChildren<PlayerCameraRig>().gameObject.transform.parent = null;
        }
    }
    

}
