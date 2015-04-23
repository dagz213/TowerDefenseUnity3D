using UnityEngine;
using System.Collections;

public class MortarRange : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            switch(gameObject.name)
            {
                case "MortarRange1":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set1 = true;
                    break;
                case "MortarRange2":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set2 = true;
                    break;
                case "MortarRange3":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set3 = true;
                    break;
                case "MortarRange4":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set4 = true;
                    break;
                case "MortarRange5":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set5 = true;
                    break;
                case "MortarRange6":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set6 = true;
                    break;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            switch(gameObject.name)
            {
                case "MortarRange1":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set1 = false;
                    break;
                case "MortarRange2":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set2 = false;
                    break;
                case "MortarRange3":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set3 = false;
                    break;
                case "MortarRange4":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set4 = false;
                    break;
                case "MortarRange5":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set5 = false;
                    break;
                case "MortarRange6":
                    gameObject.transform.parent.gameObject.GetComponent<Mortar>().set6 = false;
                    break;
            }
            
        }
    }
}
