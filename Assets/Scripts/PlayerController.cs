using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isOponent;

    public Rigidbody rigBod;
    public Transform orientation;
    public PlayerController oponent;

    public float rotationSpeed;
    public float moveSpeed;
    public float maxSpeed;

    public Animator leftHand, rightHand, bodyAnim;

    public Renderer render;

    float startSpeed;
    public float horizontalInput, verticalInput;

    public bool jumping, doJump, doLSlap, doRSlap, doShove;

    public Coroutine jmp, lSlap, rSlap, shov;

    public bool swingingRHand, swingingLHand, shoving, dizzy;

    Quaternion rot;

    public GameManager gm;

    public AudioClip slap1, slap2, slap3, slap4;
    public AudioSource sudio;

    public AudioSource hadSourceL;
    public AudioSource hadSourceR;

    private void Start()
    {
        startSpeed = moveSpeed;
        if (!isOponent) render.materials[0].color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (!isOponent && !jumping && !dizzy) Inputs();

        Vector3 dir = oponent.transform.position - transform.position;
        dir.y = 0; 
        rot = Quaternion.LookRotation(dir);
    }

    void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && !jumping && rigBod.velocity.magnitude > 0)
        {
            doJump = true;
        }

        if (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1) && !shoving)
        {
            shoving = true;
            doShove = true;
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !swingingLHand)
        {
            swingingLHand = true;
            doLSlap = true;
        }

        if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0) && !swingingRHand)
        {
            swingingRHand = true;
            doRSlap = true;
        }
    }

    private IEnumerator LSlap()
    {
        hadSourceL.Play();
        yield return new WaitForSeconds(1);
        swingingLHand = false;
        leftHand.Play("HandRIdle");
        leftHand.gameObject.tag = "Untagged";
    }

    private IEnumerator RSlap()
    {
        hadSourceR.Play();
        yield return new WaitForSeconds(1);
        swingingRHand = false;
        rightHand.Play("HandRIdle");
        rightHand.gameObject.tag = "Untagged";
    }

    private IEnumerator Shove()
    {
        yield return new WaitForSeconds(.3f);
        if (jmp != null) StopCoroutine(jmp);
        jumping = true;
        var moveDirection = orientation.forward * 1;

        rigBod.velocity = Vector3.zero;
        rigBod.AddForce(orientation.up * 150, ForceMode.Force);
        rigBod.AddForce(moveDirection.normalized * moveSpeed * 6 * 10f, ForceMode.Force);

        jmp = StartCoroutine(Jump());

        yield return new WaitForSeconds(1);
        shoving = false;
        bodyAnim.Play("Idle");
        rightHand.Play("HandRIdle");
        leftHand.Play("HandRIdle");

        rightHand.gameObject.tag = "Untagged";
        leftHand.gameObject.tag = "Untagged";
    }

    public IEnumerator Jump()
    {
        yield return new WaitForSeconds(.8f);
        jumping = false;
        rigBod.AddForce(-orientation.up * 150, ForceMode.Force);
    }

    private IEnumerator Dizzy()
    {
        bodyAnim.Play("Dizzy");
        yield return new WaitForSeconds(1f);
        bodyAnim.Play("Idle");
        dizzy = false;

        rigBod.AddForce(-orientation.up * 150, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if (!swingingLHand && !swingingRHand && !shoving) transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

        if (doJump)
        {
            if (jmp != null) StopCoroutine(jmp);
            jumping = true;
            var moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rigBod.AddForce(orientation.up * 150, ForceMode.Force);
            rigBod.AddForce(moveDirection.normalized * moveSpeed * 5 * 10f, ForceMode.Force);

            jmp = StartCoroutine(Jump());

            doJump = false;
        }

        if (doLSlap)
        {
            if (lSlap != null) StopCoroutine(lSlap);
            leftHand.Play("HandRSlap");
            lSlap = StartCoroutine(LSlap());
            leftHand.gameObject.tag = "Hand";

            doLSlap = false;
        }

        if (doRSlap)
        {
            if (rSlap != null) StopCoroutine(rSlap);
            rightHand.Play("HandRSlap");
            rSlap = StartCoroutine(RSlap());
            rightHand.gameObject.tag = "Hand";

            doRSlap = false;
        }

        if (doShove)
        {
            if (shov != null) StopCoroutine(shov);
            rightHand.Play("HandRSlap");
            leftHand.Play("HandRSlap");
            bodyAnim.Play("Shove");
            shov = StartCoroutine(Shove());

            rightHand.gameObject.tag = "Hand";
            leftHand.gameObject.tag = "Hand";

            doShove = false;
        }

        if (!jumping) PhysicsUpdates();
    }

    void PhysicsUpdates()
    {
        var moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rigBod.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        if (rigBod.velocity.magnitude > maxSpeed && !jumping)
        {
            rigBod.velocity = rigBod.velocity.normalized * maxSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject != rightHand.gameObject || other.gameObject != leftHand.gameObject) 
            && other.gameObject.tag == "Hand" && !dizzy)
        {
            dizzy = true;
            GetSlapped();

            if (oponent.shoving)
            {
                KnockBack();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ArenaLimits")
        {
            MatchOver();
        }
    }

    void GetSlapped()
    {
        if (jmp != null) StopCoroutine(jmp);

        switch (Random.Range(0, 4))
        {
            case 0:
                sudio.clip = slap1;
                break;
            case 1:
                sudio.clip = slap2;
                break;
            case 2:
                sudio.clip = slap3;
                break;
            case 3:
                sudio.clip = slap4;
                break;
        }
        sudio.Play();

        rigBod.velocity = Vector3.zero;
        StopAllCoroutines();
        swingingLHand = false;
        swingingRHand = false;
        rightHand.gameObject.tag = "Untagged";
        leftHand.gameObject.tag = "Untagged";

        shoving = false;
        bodyAnim.Play("Idle");
        rightHand.Play("HandRIdle");
        leftHand.Play("HandRIdle");

        dizzy = false;
        StartCoroutine(Dizzy());
    }

    void KnockBack()
    {
        if (jmp != null) StopCoroutine(jmp);
        jumping = true;
        var moveDirection = -orientation.forward * 1;

        var knockBack = 4;
        if (dizzy) knockBack = 6;

        rigBod.AddForce(orientation.up * 150, ForceMode.Force);
        rigBod.AddForce(moveDirection.normalized * moveSpeed * knockBack * 10f, ForceMode.Force);

        jmp = StartCoroutine(Jump());
    }

    void MatchOver()
    {
        Time.timeScale = 0;
        gm.MatchOver(this);
    }
}
