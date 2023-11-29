using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float radius;
    float timer = 0f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletPoint;
    [SerializeField] LayerMask layerUnit;
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer = 0f;
            GetClosestUnit();
        }
    }

    void Attack(Transform target)
    {
        var bulletGO = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
        bulletGO.transform.DOMove(target.position, 1f) ;
    }


    [ContextMenu("Run")]
    private Unit GetClosestUnit()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius,layerUnit);
        float closestPos = 100;
        Unit closestUnit = null;
        foreach (Collider2D c in enemies)
        {
            float newClosest = Vector2.Distance(transform.position, c.transform.position);

            if (newClosest < closestPos)
            {
                closestPos = newClosest;
                if (c.TryGetComponent<Unit>(out Unit unit))
                {
                    closestUnit = unit;
                    Attack(closestUnit.transform);
                    Debug.Log(closestUnit.name);
                }
            }


        }

        return closestUnit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}
