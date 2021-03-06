﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    [SerializeField]
    Text tutorial;

    [SerializeField]
    float fadeSpeed;

	// Use this for initialization
	void Start () {
        tutorial.color = new Color(tutorial.color.r, tutorial.color.g, tutorial.color.b, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        StartCoroutine(FadeIn(fadeSpeed, tutorial));
    }

    void OnTriggerExit(Collider col)
    {
        StartCoroutine(FadeOut(fadeSpeed, tutorial));
    }

    IEnumerator FadeIn(float s, Text t)
    {
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        while (t.color.a < 1.0f)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a + (Time.deltaTime / s));
            yield return null;
        }
    }

    public IEnumerator FadeOut(float s, Text t)
    {
        t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
        while (t.color.a > 0.0f)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, t.color.a - (Time.deltaTime / s));
            yield return null;
        }
    }
}
