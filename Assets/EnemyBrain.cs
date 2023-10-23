using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public PlayerController controllerScript;

    public GameObject debugPos;
    public Vector3 targetMovePosition;

    public float intelect, dexterity, strength, bioMass, reflexes;

    public float dist;

    bool readyToAttack = true;

    Coroutine moveTarg;

    private void Start()
    {
        intelect = controllerScript.gm.inte;
        strength = controllerScript.gm.stre;
        dexterity = controllerScript.gm.dext;

        controllerScript.render.materials[1].color = new Color(strength/5, dexterity/5, intelect/5);

        bioMass = .1f * strength - .1f * dexterity;
        //controllerScript.rigBod.mass += bioMass;
        transform.localScale = new(2 + bioMass, 2 + bioMass, 2 + bioMass);

        reflexes = Mathf.RoundToInt((intelect + dexterity) / 2);

        controllerScript.maxSpeed += dexterity * 2 - 5;
        controllerScript.rotationSpeed += dexterity * 2 - 5;
        controllerScript.moveSpeed += dexterity * 2 - 5;

        targetMovePosition = ChooseTargetMove();
    }

    private void Update()
    {
        targetMovePosition = new Vector3(targetMovePosition.x, 3.5f, targetMovePosition.z);
        dist = Vector3.Distance(targetMovePosition,
            transform.position);

        if (Vector3.Distance(targetMovePosition,
            transform.position) < 1)
        {
            targetMovePosition = ChooseTargetMove();
            if (moveTarg != null) StopCoroutine(moveTarg);
            moveTarg = StartCoroutine(ChangeMoveTarget());
        }

        if (!controllerScript.jumping && !controllerScript.dizzy &&
            !controllerScript.swingingLHand && !controllerScript.swingingRHand
            && !controllerScript.shoving) MoveToPoint();

        if (Vector3.Distance(controllerScript.oponent.transform.position,
            transform.position) < 5 && (controllerScript.oponent.swingingLHand 
            || controllerScript.oponent.swingingRHand || controllerScript.oponent.shoving))
        {
            int x = 5 - Mathf.RoundToInt(reflexes);
            switch (Random.Range(0, x))
            {
                case 0:
                    Dodge();
                    break;
                default:
                    break;
            }
        }

        if (Vector3.Distance (controllerScript.oponent.transform.position, 
            transform.position) < 5 && readyToAttack)
        {
            readyToAttack = false;

            int x = 5 - Mathf.RoundToInt(intelect);
            switch (Random.Range(0, 6 - x))
            {
                case 0:
                    Slap();
                    break;
                case 1:
                    Shove();
                    break;
                default:
                    break;
            }

            Invoke("AttackCoolDown", 1);
        }
    }

    void Dodge()
    {
        if (controllerScript.jmp != null) StopCoroutine(controllerScript.jmp);
        controllerScript.jumping = true;
        var moveDirection = CalculateEscapeVector();

        controllerScript.rigBod.velocity = Vector3.zero;
        controllerScript.rigBod.AddForce(controllerScript.orientation.up * 150, ForceMode.Force);
        controllerScript.rigBod.AddForce(moveDirection.normalized * 
            controllerScript.moveSpeed * (6 * (bioMass + 1)) * 10f, ForceMode.Force);

        controllerScript.jmp = StartCoroutine(controllerScript.Jump());
    }

    Vector3 CalculateEscapeVector()
    {
        switch (Random.Range(0, 5 - intelect))
        {
            case 0:
                if (Vector3.Distance(transform.position + new Vector3(-6, 0, -6),
                    new Vector3(0, transform.position.y, 0)) < 5)
                {
                    return new Vector3(-1, 0, -1);
                }
                else if (Vector3.Distance(transform.position + new Vector3(6, 0, -6),
                    new Vector3(0, transform.position.y, 0)) < 5)
                {
                    return new Vector3(1, 0, -1);
                }
                else if (Vector3.Distance(transform.position + new Vector3(6, 0, 0),
                    new Vector3(0, transform.position.y, 0)) < 5)
                {
                    return new Vector3(1, 0, 0);
                }
                else if (Vector3.Distance(transform.position + new Vector3(-6, 0, 0),
                    new Vector3(0, transform.position.y, 0)) < 5)
                {
                    return new Vector3(-1, 0, 0);
                }
                else
                {
                    return new Vector3(0, 0, 0);
                }
            default:
                switch (Random.Range(0, 4))
                {
                    case 0:
                        return new Vector3(-1, 0, -1);
                    case 1:
                        return new Vector3(1, 0, -1);
                    case 2:
                        return new Vector3(1, 0, 0);
                    case 3:
                        return new Vector3(-1, 0, 0);
                }
                break;
        }

        return new Vector3(0, 0, 0);
    }

    private IEnumerator ChangeMoveTarget()
    {
        yield return new WaitForSeconds(1);
        targetMovePosition = ChooseTargetMove();
    }

    void AttackCoolDown()
    {
        readyToAttack = true;
    }

    Vector3 ChooseTargetMove()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                return controllerScript.oponent.transform.position;
            default:
                return new Vector3(Random.Range(-5, 6), transform.position.y, Random.Range(-5, 6));
        }
    }

    void Slap()
    {
        if (Random.Range(0, 2) == 0)
        {
            if (!controllerScript.swingingRHand)
            {
                controllerScript.doRSlap = true;
                controllerScript.swingingRHand = true;
            }
            else if (!controllerScript.swingingLHand)
            { 
                controllerScript.doLSlap = true;
                controllerScript.swingingLHand = true;
            }
        }
        else
        {
            if (!controllerScript.swingingLHand)
            {
                controllerScript.doLSlap = true;
                controllerScript.swingingLHand = true;
            }
            else if (!controllerScript.swingingRHand)
            {
                controllerScript.doRSlap = true;
                controllerScript.swingingRHand = true;
            }
        }
    }

    void Shove()
    {
        if (!controllerScript.shoving && !controllerScript.swingingRHand && !controllerScript.swingingLHand)
            controllerScript.shoving = true;
            controllerScript.doShove = true;
    }

    void MoveToPoint()
    {
        //controllerScript.rigBod.AddForce(-controllerScript.orientation.up * 150, ForceMode.Force);
        var moveDirection = new Vector3(targetMovePosition.x, 0f, targetMovePosition.z) - new Vector3(transform.position.x, 0, transform.position.z);

        controllerScript.rigBod.AddForce(moveDirection.normalized * controllerScript.moveSpeed * 10f, ForceMode.Force);
        if (controllerScript.rigBod.velocity.magnitude > controllerScript.maxSpeed && !controllerScript.jumping)
        {
            controllerScript.rigBod.velocity = controllerScript.rigBod.velocity.normalized * controllerScript.maxSpeed;
        }
    }
}