using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Image image;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    Vector3 startPosApple;

    public Canvas canvas;

    bool inAir = true;
    float posY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); 
        image = GetComponent<Image>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();

        posY = eventData.position.y;

        inAir = true;
        rigidbody2D.gravityScale = 0;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        startPosApple = rectTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(posY > eventData.position.y && rectTransform.localScale.x < 1.3f)
            rectTransform.localScale *= 1.002f;
        else if (posY < eventData.position.y && rectTransform.localScale.x > 1f)
            rectTransform.localScale /= 1.002f;

        posY = eventData.position.y;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.localPosition.y > 540 || rectTransform.localPosition.y < -500)
        {
            rectTransform.localPosition = startPosApple;
            inAir = false;
        }

        if (inAir)
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            inAir = false;
            rigidbody2D.gravityScale = 1;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fruit")
            collision.transform.SetAsLastSibling();

        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        rigidbody2D.gravityScale = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        inAir = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inAir = true;
    }
}
