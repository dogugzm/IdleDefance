using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Color baseColor;
    [SerializeField] Color offsetColor;
    [SerializeField] Color baseColorE;
    [SerializeField] Color offsetColorE;
    SpriteRenderer spriteRenderer;
    public int x, y;

    public bool hasBuilding = false;
    public bool isEnemySide = false;

    private Color defaultColor;

    Tween alphaTween;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(bool _isOffset, int _x, int _y)
    {
        if (isEnemySide)
        {
            spriteRenderer.color = _isOffset ? offsetColorE : baseColorE;
            defaultColor = spriteRenderer.color;
        }
        else
        {
            spriteRenderer.color = _isOffset ? offsetColor : baseColor;
            defaultColor = spriteRenderer.color;

        }
        x = _x;
        y = _y;
    }

    private void OnMouseDown()
    {
        MouseClicked();
    }

    private void MouseClicked()
    {
        Debug.Log(x + " - " + y);
        GridManager.TileClickEvent?.Invoke(x, y);
    }

    public void YoyoColorChange()
    {
        if (hasBuilding)
        {
            return;
        }
         alphaTween = spriteRenderer.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void SetDefaultColor()
    {
        DOTween.Kill(alphaTween);
        spriteRenderer.DOColor(defaultColor, 0.5f);
    }

}
