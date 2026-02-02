using UnityEngine;

public class EnemyHighlight : MonoBehaviour
{
    private SpriteRenderer sprite;
    private GameObject targetCircle;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        Transform circle = transform.Find("TargetCircle");

        if (circle != null)
            targetCircle = circle.gameObject;
        else
            Debug.LogError("TargetCircle NOT FOUND on " + gameObject.name);
    }


    public void Show()
    {
        sprite.color = new Color(1.3f, 1.3f, 1.3f); // подсветка
        targetCircle.SetActive(true);
    }

    public void Hide()
    {
        sprite.color = Color.white;
        targetCircle.SetActive(false);
    }
}
