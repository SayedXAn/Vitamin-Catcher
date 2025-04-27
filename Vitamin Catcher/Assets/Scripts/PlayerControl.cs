using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    public Image playerImage;
    public Manager mngr;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            // Lock Y axis
            worldPoint.y = rectTransform.position.y;

            // Clamp X position (in world space)
            worldPoint.x = Mathf.Clamp(worldPoint.x, -2.3f, 2.3f);

            transform.position = worldPoint;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        playerImage.color = new Color32(255, 255, 255, 210);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerImage.color = new Color32(255, 255, 255, 255);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Debug.Log(transform.position.x);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int id = collision.gameObject.GetComponent<forCollectibles>().cID;
        if(collision.gameObject.tag == "immunity")
        {
            Destroy(collision.gameObject);
            mngr.CheckCollectibleHit(0, id);
        }
        else if (collision.gameObject.tag == "bones")
        {
            Destroy(collision.gameObject);
            mngr.CheckCollectibleHit(1, id);
        }
        else if (collision.gameObject.tag == "social")
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
