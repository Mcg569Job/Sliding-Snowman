using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    [SerializeField] [Range(1, 30)] private float maxX = 2;
    [SerializeField] [Range(1, 10)] private float sensitivity = 5;
    [SerializeField] private Transform player;
    [SerializeField] private RectTransform swipeButton;
    private float _screenX, _x;
    private bool _isHold;

    void Start()
    {
        _screenX = Screen.width / 2;
        SetSensitivity();
    }

    public void SetSensitivity()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity");
    }
    void FixedUpdate()
    {

        if (GameManager.instance.gameStatus == GameStatus.Null && _isHold)
            GameManager.instance.Play();
        else if (!GameManager.instance.gameStatusIsNormal())        
            return;
       

        if (_isHold)
        {
  
           /* float x =Input.mousePosition.x > _screenX ? Time.deltaTime * sensitivity : -Time.deltaTime *sensitivity;             
               player.position += new Vector3(Mathf.Clamp(x,-maxX,maxX), 0, 0);
            */
        
            float mouseX = Input.mousePosition.x;
            _x = mouseX < _screenX * .5f ? maxX * -(_screenX - mouseX) / _screenX : maxX * (mouseX - _screenX) / _screenX;
            Vector3 vec = player.position;
            vec.x = Mathf.Lerp(vec.x, _x, Time.deltaTime * sensitivity);
            player.position = vec;//Vector3.Lerp(player.position, vec, Time.deltaTime * sensitivity);     

            Vector2 sBtn = swipeButton.localPosition;
            sBtn.x = (_x / maxX) * _screenX;
            swipeButton.localPosition = sBtn;
           
        }
    }

    public void Hold(bool b) =>_isHold = b;  
}
