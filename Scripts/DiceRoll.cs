using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour {

    public float speed = 1000f;
    public float diceRollTime = 1f;

    private bool roll = true;
    private float timer = 0f;
    public int diceRoll = 0;
    private int[,] diceAngleValues = new int[6,2]{ { 0,0 },{270,0 },{ 0,90 },{ 0,270 },{ 90,0 },{ 0,180 } };
    private System.Random rand;

    // Use this for initialization
    void Start () {
        rand = new System.Random(diceRoll + (int)DateTime.Now.Ticks & 0x0000FFFF);
        roll = false;
    }
	
	// Update is called once per frame

	void Update () {
        if (roll)
        {
            RunDiceVisual();
        }

        if(timer < diceRollTime)
        {
            timer += Time.deltaTime;
        }
        else if(roll)
        {
            ChooseValues();
            roll = false;
        }
    }

    public void RollDice()
    {
        roll = true;
        timer = 0;
    }

    public bool IsDiceRolling()
    {
        return roll && gameObject.activeInHierarchy;
    }

    private void RunDiceVisual()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        transform.Rotate(Vector3.right, speed * Time.deltaTime);
    }

    private void ChooseValues()
    {
        diceRoll = rand.Next(0, 6);
        if (diceAngleValues[diceRoll, 1] > 0)
        {
            transform.rotation = Quaternion.AngleAxis(diceAngleValues[diceRoll, 1], Vector3.right);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(diceAngleValues[diceRoll, 0], Vector3.up);
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }

    public int GetDiceRoll()
    {
        return (diceRoll + 1);
    }
}
