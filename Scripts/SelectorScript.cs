using UnityEngine;
using UnityEngine.UI;


public class SelectorScript : MonoBehaviour
{
    public static SelectorScript instance;


    public Image image;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of SelectorScript found.");
            return;
        }
        instance = this;

        image = GetComponent<Image>();
        SetSprite(null);
    }

    public void SetSprite(Sprite newSprite)
    {
        if (image.sprite != newSprite)
        {
            image.sprite = newSprite;
        }
        if (newSprite == null && (GameManager.instance == null || !GameManager.instance.openingJar))
        {
            image.sprite = null;
            image.color = new Color(1, 1, 1, 0); 
            
        }
        else
        {
            image.sprite = newSprite;
            image.color = Color.white; 
        }

    }

}
