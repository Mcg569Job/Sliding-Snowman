using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image selected, question;
    public int price;
    public Text text;
    public Image image;
    private bool isSelected;
   


    public void Purchased(bool b) { question.gameObject.SetActive(!b); text.text = b ? "" : price.ToString(); }

    public void Select(bool b)
    {
        try
        {
            selected.gameObject.SetActive(b);
            if (b)
                SelectMe();
            else
                UnSelect();
        }
        catch { }
    }

    private void SelectMe()
    {
        if (!isSelected && gameObject.activeSelf)
        {
            isSelected = true;
            image.transform.localScale = new Vector3(.75f, .75f, 1);
            
        }
    }

   

    private void UnSelect()
    {
        if (!isSelected) return;
        SelectMe();
        isSelected = false;
       
        image.transform.localScale = new Vector3(1, 1, 1);
        image.transform.rotation = Quaternion.identity;
    }
}
