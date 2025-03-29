using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Image image;
    public Manager mngr;

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.color = new Color32(255, 255, 255, 170);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Keep Y position fixed, only update X position
        Vector3 newPosition = Input.mousePosition;
        newPosition.y = rectTransform.position.y; // Lock Y position
        newPosition.x = Mathf.Clamp(newPosition.x, 70f, 1000f);
        //Debug.Log("newPosition.x == " + newPosition.x);
        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.color = new Color32(255, 255, 255, 255);
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int id = collision.gameObject.GetComponent<forCollectibles>().cID;
        if(collision.gameObject.tag == "immunity")
        {
            Destroy(collision.gameObject);
            mngr.CheckCollectibleHit(0, id);
        }
        else if (collision.gameObject.tag == "social")
        {
            Destroy(collision.gameObject);
            mngr.CheckCollectibleHit(1, id);
        }
        else if (collision.gameObject.tag == "bones")
        {
            Destroy(collision.gameObject);
            mngr.CheckCollectibleHit(2, id);
        }
        else if(collision.gameObject.tag == "virus")
        {
            Destroy(collision.gameObject);
            mngr.VirusHit();
            //mngr.StartSpawn();
        }
    }
}
