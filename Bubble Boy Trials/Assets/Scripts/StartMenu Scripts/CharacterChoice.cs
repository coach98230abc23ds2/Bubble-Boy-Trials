using UnityEngine;
using System.Collections;

public class CharacterChoice : MonoBehaviour {

    private string character_chosen;

	public void SaveCharacter(string character)
    {   
        character_chosen = character;
    }

    public void PrintCharacter()
    {
        Debug.Log(character_chosen);
    }
}
