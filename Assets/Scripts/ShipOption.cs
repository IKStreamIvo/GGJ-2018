﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOption : MonoBehaviour {

    public int ship;
    public bool isSelected;

    public SpriteRenderer image;
    public new Animation animation;
    Vector3 defaultScale;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animation>();
        defaultScale = transform.lossyScale;
    }

    public void Select(bool yes)
    {
        if (yes)
        {
            isSelected = true;
            //image.enabled = true;
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
            animation.enabled = true;
        }
        else
        {
            isSelected = false;
            //image.enabled = false;
            transform.localScale = defaultScale;
            animation["ShipSelectionIdle"].time = 0f;
            animation.enabled = false;
        }
    }
}
