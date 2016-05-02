using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    private Animator char_anim;

	// Use this for initialization
	void Start () 
    {
	    this.gameObject.transform.SetAsLastSibling();
        char_anim = this.gameObject.GetComponent<Animator>();
	}
	
	void OnMouseEnter()
    {
        char_anim.SetFloat("Speed", 1f);    
    }

    void OnMouseExit()
    {
        char_anim.SetFloat("Speed", 0f);
    }

    public void MakeCharacterJump()
    {
        char_anim.SetTrigger("Jump");
    }
}
