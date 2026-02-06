using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class Sequence {
    public int sequenceType;
    public int start;
    public int[] steps;
    public int multiplier;
    public int calculations;
}

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    
    public TextMeshProUGUI cookieCounter;
    public TextMeshProUGUI jarCounter;
    public Image heart1;
    public Image heart2;
    public Image heart3;

    public Sprite openHeart;
    public Sprite brokenHeart;

    public int lives = 3;
    public int level = 1;
    public int openedJars = 0;

    public int immunityCount = 0;
    public int turnsImmune = 0;

    public JarType cookie;
    public JarType crumb;
    public JarType empty;


    public JarType[] jarDatabase;

    public static GameManager instance;

    public GameObject MySceneManager;

    public Animator animator;
    public Animator lidAnimator;
    public bool openingJar = false;

    public Sequence currentSequence;


    void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found.");
            return;
        } else {
            instance = this;

        }


        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateLevel(1);
        PlayerPrefs.SetInt("score", 0);
    }

    public string getSequenceResult(int x, Sequence s)
    {
        int sequenceType = s.sequenceType;
        int start = s.start;
        int[] steps = s.steps;
        int multiplier = s.multiplier;
        int calculations = s.calculations;

        if (sequenceType < 2)
        {
            // Linear: multiplier * (start + step[0]*x)
            return (multiplier * (start + steps[0] * x)).ToString();
        }
        else if (sequenceType < 4)
        {
            // Geometric: start + multiplier * x^step[0]
            //return (start + multiplier * Mathf.Pow(x, steps[0])).ToString();
            return (multiplier * (start + steps[0] * x)).ToString();
        }
        else if (sequenceType < 6)
        {
            // Multistep
            int ans = start;
            for (int i = 0; i < x; i++)
            {
                ans += steps[i % calculations];
            }
            return ans.ToString();
        }
        else if (sequenceType < 8)
        {
            // Quadratic: multiplier*x^2 + steps[0]*x + start
            //return (multiplier * Mathf.Pow(x, 2) + steps[0] * x + start).ToString();
            int ans = start;
            for (int i = 0; i < x; i++)
            {
                ans += steps[i % calculations];
            }
            return ans.ToString();
        }


        return start.ToString();
    }

    public Sequence generateSequence()
    {
        int topLevel = level + 1;
        if (topLevel > 10)
            topLevel = 10;

        int sequenceType = Random.Range(1, topLevel);
        int start = Random.Range(-5, 6);
        int multiplier = Random.Range(-3, 4);
        int calculations = Random.Range(1, level+1);
        int[] steps;
        //Force multistep
        sequenceType = 5;

        if (sequenceType >= 4 && sequenceType < 6)
        {
            // Multistep: array with "calculations" length
            steps = new int[calculations];
            for (int i = 0; i < calculations; i++)
                steps[i] = Random.Range(-15, 16);
        }
        else
        {
            // Linear, Geometric, Quadratic: only one step
            steps = new int[1];
            while (steps[0]==null||steps[0]==0) {
                steps[0] = Random.Range(-5, 6);
            }   
            
        }


        return new Sequence()
        {
            sequenceType = sequenceType,
            start = start,
            multiplier = multiplier,
            steps = steps,
            calculations = calculations
        };
    }




    public JarType pickJar(JarType[] jars) {
        float totalWeight = 0f;

        foreach (var jar in jars)
        {
            if (jar.minimumLevel<=level) {
                 totalWeight += jar.weight;
            } 
           
        }

        float randomValue = Random.Range(0, totalWeight);

        float cumulativeWeight = 0f;
        foreach (var jar in jars)
        {
            if (jar.minimumLevel > level) continue;
            cumulativeWeight += jar.weight;
            if (randomValue <= cumulativeWeight)
            {
                return jar;
            }
        }

        return empty;
    
    }

    public void ClearChildren(GameObject parent)
    {
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    public void generateLevel(int level)
    {
        ClearChildren(MySceneManager);
        StartCoroutine(GenerateGridNextFrame(level));
    }

    private IEnumerator GenerateGridNextFrame(int level)
    {
        yield return null; 
        MySceneManager.GetComponent<MySceneManager>().generateGrid(level);
        JarScript[] jars = MySceneManager.GetComponentsInChildren<JarScript>();

        currentSequence = generateSequence();

        int randomIndex = Random.Range(0, jars.Length);

        jars[randomIndex].jarType = cookie;
        int x = jars[randomIndex].x;
        int y = jars[randomIndex].y;

        int crumbCounter = 0;
        
        for (int i=0;i<jars.Length;i++) {
            if (jars[i].jarType == cookie) {
                int value = int.Parse(getSequenceResult(i, currentSequence)) + Random.Range(-5, 6);
                if (value==int.Parse(getSequenceResult(i, currentSequence))) {value=int.Parse(getSequenceResult(i, currentSequence))+1;}
                jars[i].label = value.ToString();
                continue;
            }

            if (Random.Range(0, 100)>30) {
                jars[i].jarType=pickJar(jarDatabase);
            } else {
                jars[i].jarType=empty;
            }

            jars[i].label = getSequenceResult(i, currentSequence);


            if (Mathf.Abs(x - jars[i].x) < 2 && Mathf.Abs(y - jars[i].y) < 2 && Random.Range(0, 100) > 20 && crumbCounter < 4 && level > 2) {
                jars[i].jarType = crumb;
                crumbCounter += 1;
            }
        
        }
    }

    public void openJar(JarType jar) {
        if (!openingJar) {
            if (jar.type!=0) {
                animator.SetTrigger("OpenJar");
            }

            lidAnimator.SetTrigger("OpenJar");

            AudioManager.Instance.PlayAlt2();
            
            StartCoroutine(OpenJarCoroutine(jar));
            StartCoroutine(FinishOpeningJarCoroutine());
            openingJar = true;
        }
        
        
    }

    private IEnumerator FinishOpeningJarCoroutine() {
        yield return new WaitForSeconds(1.5f);
        openingJar = false;
    }

    

    private IEnumerator OpenJarCoroutine(JarType jar)
    {
        yield return new WaitForSeconds(1f);


        openedJars+=1;
        title.text = jar.name;
        description.text = jar.description;

        if (jar.lifeChange<0 && (turnsImmune>0||immunityCount>0)) {
            if (immunityCount>0) {
                immunityCount-=1;
                description.text = "You sniffed out the "+jar.name+"! No lives taken.";
            } else if (turnsImmune>0) {
                description.text = "The Cabinet Key makes you immune from everything for now. No lives taken.";
            }
        } else {
            lives+=jar.lifeChange;
        }

        if (lives>3) {
            lives = 3;
        }

        if (turnsImmune>0) {
            turnsImmune-=1;
            
        }

        if (jar.turnsImmune>0) {
            turnsImmune=jar.turnsImmune;
        }

        if (jar.immunityCount>0) {
            immunityCount=jar.immunityCount;
        }
        
        

        PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0)+jar.experienceGained);


        if (PlayerPrefs.GetInt("score", 0)>PlayerPrefs.GetInt("highscore", 0)) {
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score", 0));
        }

        PlayerPrefs.Save();
        
        if (jar.progress) {
            level+=1;
            generateLevel(level);
        }

        

    }

    void Update() {
        cookieCounter.text = $"{level-1}";
        jarCounter.text = $"{openedJars}";
        if (lives>0) {
            heart1.sprite=openHeart;
        } else {
            heart1.sprite = brokenHeart;
        }

        if (lives>1) {
            heart2.sprite=openHeart;
        } else {
            heart2.sprite = brokenHeart;
        }

        if (lives>2) {
            heart3.sprite=openHeart;
        } else {
            heart3.sprite = brokenHeart;
        }

        if (lives <= 0) {
            SceneManager.LoadScene("Death Scene");
        }
    }
}
