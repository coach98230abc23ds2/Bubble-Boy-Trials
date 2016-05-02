using UnityEngine;
using System.Collections;

public class SelectionButton : MonoBehaviour {

    private Animator boy_anim;
    private Animator girl_anim;

	// Use this for initialization
	void Start () {
        boy_anim = GameObject.Find("Boy").GetComponent<Animator>();
        girl_anim = GameObject.Find("Girl").GetComponent<Animator>();
	}
	
    void OnMouseEnter()
    {   
        if (this.gameObject.name == "Button1")
        {   
            Debug.Log("touched button1");
            boy_anim.SetFloat("Speed", 1f);   
        }
        else if (this.gameObject.name == "Button2")
        {
            girl_anim.SetFloat("Speed", 1f);
        }
    }

    void OnMouseExit()
    {
        if (this.gameObject.name == "Button1")
        {
            boy_anim.SetFloat("Speed", 0f);   
        }
        else if (this.gameObject.name == "Button2")
        {
            girl_anim.SetFloat("Speed", 0f);
        }
    }
}
