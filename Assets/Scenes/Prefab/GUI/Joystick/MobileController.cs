using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    [SerializeField] private Image JoystickBG; 
    private Transform JoystickTransform;
    private Vector3 StartPosition;
    [SerializeField] private Image Joystick;
    [SerializeField] private Canvas canvas;

    private Vector2 InputVector;
    private Vector2 JosPosition;

    private void Start()
    {        
        JoystickTransform = JoystickBG.transform;
        StartPosition = JoystickTransform.localPosition;
        canvas.worldCamera = Camera.main;
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            JoystickTransform.position = hit.point;
        }
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        
        InputVector = Vector2.zero;
        Joystick.rectTransform.anchoredPosition = Vector2.zero;
        JoystickTransform.localPosition = StartPosition;
    }   
    
    public virtual void OnDrag(PointerEventData ped)
    {
     
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBG.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / JoystickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / JoystickBG.rectTransform.sizeDelta.x);
            InputVector = new Vector2(pos.x, pos.y);
            InputVector = (InputVector.magnitude > 1.0f) ? InputVector.normalized : InputVector;
            Joystick.rectTransform.anchoredPosition=new Vector2(InputVector.x*(JoystickBG.rectTransform.sizeDelta.x/2), InputVector.y * (JoystickBG.rectTransform.sizeDelta.y / 2));
        }
    }

    public float Horizontal()
    {
        if (InputVector.x!=0)
        {
            return InputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float Vertical()
    {
        if (InputVector.x != 0)
        {
            return InputVector.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
    public void HideJoystick()
    {
        InputVector = Vector2.zero;
        Joystick.rectTransform.anchoredPosition = Vector2.zero;
        JoystickTransform.localScale = Vector3.zero;
    }

}
