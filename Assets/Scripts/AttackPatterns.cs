using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPatterns : MonoBehaviour
{
    public static Dictionary<string, Sprite> attackPatterns = new Dictionary<string, Sprite>();
    public string attackPatternFolder = "AttackPatternFolder";
    
    // Start is called before the first frame update
    void Awake()
    {
        Sprite[] loadedObjects = Resources.LoadAll<Sprite>(attackPatternFolder);

        //Add attack patterns from the respective folder
        foreach(Sprite attack in loadedObjects)
            if (!attackPatterns.ContainsKey(attack.name))
                attackPatterns.Add(attack.name, attack);
    }
}
