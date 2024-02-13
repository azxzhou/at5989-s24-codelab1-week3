using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrizeScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        
        //score +1
        GameManager.instance.Score++; //ref. game manager instance bc there is only one
        
        //prize relocates to random location
        transform.position = new Vector3(
            Random.Range(-10, 10), 
            Random.Range(-7, 7)
            );




    }
}
