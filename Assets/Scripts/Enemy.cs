using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float radius;

    private void Update()
    {
        

    }

    [ContextMenu("Run")]
    private Unit Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius);
        float closestPos = 100;
        Unit closestUnit = null;
        foreach (Collider2D c in enemies)
        {
            float newClosest = Vector2.Distance(transform.position, c.transform.position);         

            if (newClosest<closestPos)
            {
                closestPos = newClosest;
                closestUnit = c.GetComponent<Unit>();
                Debug.Log(closestUnit.name);
            }


        }

        return closestUnit;
    }


}
