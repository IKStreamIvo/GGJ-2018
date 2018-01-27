using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOption : MonoBehaviour {

    public int ship;
    public bool isSelected;

    public SpriteRenderer image;
    public new Animation animation;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animation>();
    }

    public void Select(bool yes)
    {
        if (yes)
        {
            isSelected = true;
            image.enabled = true;
            animation.enabled = true;
        }
        else
        {
            isSelected = false;
            image.enabled = false;
            animation["ShipSelectionIdle"].time = 0f;
            animation.enabled = false;
        }
    }
}
