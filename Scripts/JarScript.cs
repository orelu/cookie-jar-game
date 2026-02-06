using UnityEngine;
using TMPro; // Required for TextMeshProUGUI

public class JarScript : MonoBehaviour
{
    public JarType jarType;

    public bool opened = false;

    public int x = 0;
    public int y = 0;

    public string label = "0";

    private TextMeshProUGUI labelTMP;

    private Transform lid;


    


    void Start()
    {

        labelTMP = GetComponentInChildren<Canvas>()?.GetComponentInChildren<TextMeshProUGUI>();
        if (labelTMP == null)
        {
            Debug.LogWarning("No TextMeshProUGUI found in child Canvas of " + gameObject.name);
        }

        

        lid = transform.Find("Lid");
        lid.localPosition = new Vector3(0, 0.0821f, 0);
    }

    void Update()
    {
        if (labelTMP != null)
        {
            labelTMP.text = label;
        }

        
    }

    public void SetSpriteToAE(GameObject go)
    {

        var sr = go.GetComponent<SpriteRenderer>();
        if (sr == null) sr = go.GetComponentInChildren<SpriteRenderer>(true);

        if (sr == null)
        {
            Debug.LogWarning($"No SpriteRenderer found on {go.name} or its children.");
            return;
        }


        Color32 aeGray = new Color32(0xD8, 0xD8, 0xD8, 0xFF);

        sr.color = aeGray;

        var mat = sr.material;
        if (mat != null && mat.HasProperty("_Color"))
            mat.SetColor("_Color", aeGray);
    }


    void OnMouseDown()
    {
        if (!GameManager.instance.openingJar && !opened)
        {
            GameManager.instance.openJar(jarType);
            opened = true;
            lid.gameObject.SetActive(false);

            SetSpriteToAE(gameObject); 
            
        }
    }

    void OnMouseOver()
    {
        
        if (!GameManager.instance.openingJar)
        {
            SelectorScript.instance.SetSprite(jarType.sprite);
        }
    }

    void OnMouseEnter() {
        AudioManager.Instance.PlayAlt1();
    }

    void OnMouseExit()
    {
        if (!GameManager.instance.openingJar)
        {
            SelectorScript.instance.SetSprite(null);
        }
    }
}
