using UnityEngine;

public class EnemyHighlight : MonoBehaviour
{
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Show()
    {
        sprite.color = new Color(1.3f, 1.3f, 1.3f);
    }

    public void Hide()
    {
        sprite.color = Color.white;
    }
}
