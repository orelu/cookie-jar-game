using UnityEngine;

[CreateAssetMenu(fileName = "JarType", menuName = "Scriptable Objects/JarType")]
public class JarType : ScriptableObject
{
    //Base

    public int type = 0;
    public string name = "";
    public string description = "";
    public Sprite sprite;

    
    
    //Behaviours

    public int lifeChange;
    public bool progress;
    public int turnsImmune = 0;
    public int immunityCount = 0;
    public int experienceGained = 0;

    //Generation

    public int minimumLevel = 0;
    public int weight = 10;
}
