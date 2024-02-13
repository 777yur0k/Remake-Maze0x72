using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] Sprite openSprite;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.instance.hasKey)
        {
            GetComponent<SpriteRenderer>().sprite = openSprite;
            GameManager.instance.PlayerWin();
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), other.collider, true);
            Camera.main.GetComponent<SetPanels>().SetPanel("Win");
        }
    }
}