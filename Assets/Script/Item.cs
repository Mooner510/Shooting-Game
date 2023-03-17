using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Heal, Fuel, Protect, Weapon
    }

    [SerializeField] private Sprite[] sprites;

    private ItemType itemType;

    public ItemType GetItemType() => itemType;

    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        itemType = (ItemType) Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[(int) itemType];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log($"trigger: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            switch (GetItemType())
            {
                case Item.ItemType.Heal:
                    DataManager.Heal(40);
                    break;
                case Item.ItemType.Fuel:
                    DataManager.SetFuel(100);
                    break;
                case Item.ItemType.Protect:
                    player.Protect();
                    break;
                case Item.ItemType.Weapon:
                    DataManager.UpgradeWeapon();
                    break;
            }
            Destroy(gameObject);
        }
    }

  /*  private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"collision: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            switch (GetItemType())
            {
                case Item.ItemType.Heal:
                    DataManager.Heal(40);
                    break;
                case Item.ItemType.Fuel:
                    DataManager.AddFuel(30);
                    break;
                case Item.ItemType.Protect:
                    player.Protect();
                    break;
                case Item.ItemType.Weapon:
                    DataManager.UpgradeWeapon();
                    break;
            }
        }
    }*/
}
