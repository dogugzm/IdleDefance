using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitSO data;
    [SerializeField] Ease easeType;
    [HideInInspector] public Building building;
    [SerializeField] SpriteRenderer spriteRenderer;

    //public Vector2 targetVector;

    [ContextMenu("a")]
    public async void MoveTowardsTile(Vector2 target)
    {
        building?.RemoveUnit();
        spriteRenderer.DOFade(1f, 0.5f);
        List<Vector2> allTiles = Pathfinding.FindPath(new Vector2(transform.position.x, transform.position.y), target);

        foreach (Vector2 pos in allTiles)
        {
            transform.DOMove(pos, data.speed).SetEase(easeType);
            await Task.Delay((int)(data.speed * 1000));
            await Task.Delay(500);
        }

    }

    private void OnMouseDown()
    {
        spriteRenderer.DOFade(0.5f, 0.5f);
        GridManager.UnitClickEvent?.Invoke(this);     
    }

}
