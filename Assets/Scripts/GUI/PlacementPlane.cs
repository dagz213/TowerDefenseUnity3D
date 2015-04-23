using UnityEngine;
using System.Collections;

public class PlacementPlane : MonoBehaviour {

    public Material placementOk;
    private Material originalMat;

    public GameObject myStructure;

	// Use this for initialization
	void Start () {
        originalMat = gameObject.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

        switch (gameObject.tag)
        {
            case "PlacementPlane_Selected":
                gameObject.GetComponent<Renderer>().material = placementOk;
                GameObject.Find("Main Camera").GetComponent<InGameGUI>().planeSelected = gameObject;
                break;
            case  "PlacementPlane_Open":
                gameObject.GetComponent<Renderer>().material = originalMat;
                break;
            case "PlacementPlane_Taken":
                gameObject.GetComponent<Renderer>().material = originalMat;
                break;
            case "PlacementPlane_TakenSelected":
                gameObject.GetComponent<Renderer>().material = placementOk;
                GameObject.Find("Main Camera").GetComponent<InGameGUI>().planeSelected = gameObject;
                break;

        }
	}
}
