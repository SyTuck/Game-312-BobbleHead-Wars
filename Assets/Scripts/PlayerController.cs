﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody head;
    public Rigidbody marineBody;
    public LayerMask layerMask;
    public Animator bodyAnimator;
    public float[] hitForce;
    public float timeBetweenHits = 2.5f;
    public float moveSpeed = 50.0f;

    private Vector3 currentLookTarget = Vector3.zero;
    private CharacterController characterController;
    private DeathParticles deathParticles;
    private float timeSinceHit = 0.0f;
    private int hitNumber = -1;
 //   private bool isDead = false;
    private bool isHit = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        deathParticles = gameObject.GetComponentInChildren<DeathParticles>();

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

        if (isHit)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > timeBetweenHits)
            {
                isHit = false;
                timeSinceHit = 0;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDirection == Vector3.zero)
        {
            bodyAnimator.SetBool("IsMoving", false);
        }
        else
        {
            head.AddForce(transform.right * 150, ForceMode.Acceleration);
            bodyAnimator.SetBool("IsMoving", true);
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.green);
        if (Physics.Raycast(ray, out hit, 1000.0f, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }
            Vector3 targetPosition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);

            Quaternion rotation = Quaternion.LookRotation(targetPosition - this.transform.position);

            this.transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Alien alien = other.gameObject.GetComponent<Alien>();
        if (alien != null)
        {
            if (!isHit)
            {
                hitNumber += 1;
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (hitNumber < hitForce.Length)
                {
                    cameraShake.intensity = hitForce[hitNumber];
                    cameraShake.Shake();
                }
                else
                {
                    Die();
                }
                isHit = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }

    public void Die()
    {
        bodyAnimator.SetBool("IsMoving", false);

        marineBody.transform.parent = null;
        marineBody.isKinematic = false;
        marineBody.useGravity = true;
        marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        marineBody.gameObject.GetComponent<Gun>().enabled = false;
        marineBody.gameObject.GetComponent<Gun>().isDead = true;

        Destroy(head.gameObject.GetComponent<HingeJoint>());
        head.transform.parent = null;
        head.useGravity = true;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);

        deathParticles.Activate();

        Destroy(gameObject);

    }
}
