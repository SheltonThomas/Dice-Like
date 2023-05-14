using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceCubeFaceBehavior : MonoBehaviour
{
    public Transform Cube;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private List<Image> buttons = new List<Image>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform face in Cube)
        {
            foreach(Transform sprite in face)
            {
                sprites.Add(sprite.gameObject.GetComponent<SpriteRenderer>());
            }
        }

        foreach(Transform button in gameObject.transform)
        {
            buttons.Add(button.gameObject.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < sprites.Count; i++)
        {
            buttons[i].sprite = sprites[i].sprite;
        }
    }

    public void OnClick()
    {
        Debug.Log("Click");
    }
}
