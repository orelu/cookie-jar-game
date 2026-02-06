using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    public GameObject jarPrefab;
    public int level = 1;
    public float spacing = 1f;   
    public float baseSize = 1f; 
    
    void Start()
    {

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }


    public void generateGrid(int level)
    {
        float overallScale = (0.5f*Mathf.Sqrt(level))-0.25f;
        if (overallScale>1) {overallScale=1+(level/100);}
    
        
        transform.localScale = new Vector3(overallScale, overallScale, 1);
        transform.localPosition = new Vector3(0, 0.3f-overallScale, 0);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        float size = baseSize / (1.4f * level - 0.2f);

        for (int row = 0; row <= level; row++)
        {
            for (int col = 0; col <= level; col++)
            {
                GameObject jar = Instantiate(jarPrefab, transform);
                jar.transform.localScale = Vector3.one * size;

                float backset = spacing * size * (level/2f);

                float x = (col * spacing * size)-backset;
                float y = (row * spacing * size)-backset;


                jar.transform.localPosition = new Vector3(x, y, 0);

                JarScript JarScript = jar.GetComponent<JarScript>();
                JarScript.x = row;
                JarScript.y = col;
            }
        }
    }
}


