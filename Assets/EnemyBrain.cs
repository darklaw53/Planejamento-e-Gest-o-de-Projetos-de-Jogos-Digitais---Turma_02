using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public PlayerController controllerScript;

    public GameObject debugPos;
    public Vector3 targetMovePosition;

    float intelect, dexterity, strength, bioMass;

    private void Start()
    {
        intelect = Random.Range(0, 6);
        dexterity = Random.Range(0, 6);
        strength = Random.Range(0, 6);

        bioMass = .1f * strength - .1f * dexterity;
        transform.localScale = new(2 + bioMass, 2 + bioMass, 2 + bioMass);
    }

    private void Update()
    {
        targetMovePosition = debugPos.transform.position;
        if (!controllerScript.jumping && !controllerScript.dizzy) MoveToPoint();
        else
        {
            controllerScript.verticalInput = 0;
            controllerScript.horizontalInput = 0;
        }

        if (Vector3.Distance (controllerScript.oponent.transform.position, transform.position) > 5)
        {
            //attack
        }
    }

    Vector3 ChooseTargetMove()
    {
        Vector3 centerOfRadius = new Vector3(0, 3, 0);
        float radius = 10f;
        return centerOfRadius + (Vector3)(radius * Random.insideUnitCircle);
    }

    void Slap()
    {
        if (Random.Range(0, 2) == 0)
        {
            if (!controllerScript.swingingRHand) controllerScript.doRSlap = true;
            else if (!controllerScript.swingingLHand) controllerScript.doLSlap = true;
        }
        else
        {
            if (!controllerScript.swingingLHand) controllerScript.doLSlap = true;
            else if (!controllerScript.swingingRHand) controllerScript.doRSlap = true;
        }
    }

    void Shove()
    {
        if (!controllerScript.shoving && !controllerScript.swingingRHand && !controllerScript.swingingLHand)
            controllerScript.shoving = true;
    }

    void MoveToPoint()
    {
        var moveDirection = new Vector3(targetMovePosition.x, 0f, targetMovePosition.z) - new Vector3(transform.position.x, 0, transform.position.z);

        controllerScript.rigBod.AddForce(moveDirection.normalized * controllerScript.moveSpeed * 10f, ForceMode.Force);
        if (controllerScript.rigBod.velocity.magnitude > controllerScript.maxSpeed && !controllerScript.jumping)
        {
            controllerScript.rigBod.velocity = controllerScript.rigBod.velocity.normalized * controllerScript.maxSpeed;
        }
    }
}
