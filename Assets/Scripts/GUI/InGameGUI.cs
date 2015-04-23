using UnityEngine;
using System.Collections;
using Pathfinding;

public class InGameGUI : MonoBehaviour {

    //Anchor to go up
    public bool buildPanelOpen = false;
    public bool upgradePanelOpen = false;
    public TweenPosition buildPanelTweener;
    public TweenPosition upgradePanelTweener;

    //Placement Plane Items
    public Transform placementPlanesRoot;
    public Material hoverMat;
    public LayerMask placementLayerMask;
    private Material originalMat;
    private GameObject lastHitObj;

    //Build Selection Items
    public GameObject[] allStructures;
    public GameObject[] upgradeStructures;
    public GameObject planeSelected;

    public AstarPath pathfinding;
    public GameObject testCollider;
    public GameObject spawnPoint;
    public GameObject aiTarget;

    public static int money;
    public static int score;
    private int lowestCost;

    //GUI Items
    public GUIStyle style;

	// Use this for initialization
	void Start () {
        money = 1000;
        score = 0;
        lowestCost = 100;
	}

    void OnGUI()
    {
        GUI.Label (new Rect (20, (Screen.height * 3) / 4, 100, 20), "Money: " + money, style);
        GUI.Label (new Rect (20, (Screen.height * 3) / 3.5f, 100, 20), "Score: " + score, style);
    }
	
	// Update is called once per frame
	void Update () {

        ToggleBuildPanel();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, placementLayerMask))
        {
            lastHitObj = hit.collider.gameObject;
            originalMat = lastHitObj.GetComponent<Renderer>().material;

            if (!planeSelected)
            {

                lastHitObj.GetComponent<Renderer>().material = hoverMat;
            }

            if(Input.GetMouseButtonDown(0) && !planeSelected)
            {

                if(lastHitObj.tag == "PlacementPlane_Open")
                {
                    lastHitObj.tag = "PlacementPlane_Selected";
                    buildPanelOpen = true;
                    upgradePanelOpen = false;

                } else if (lastHitObj.tag == "PlacementPlane_Taken")
                {
                    lastHitObj.tag = "PlacementPlane_TakenSelected";
                    upgradePanelOpen  = true;
                    buildPanelOpen = false;
                }
            } 

            if (Input.GetMouseButtonDown(0) && planeSelected && lastHitObj.tag != "PlacementPlane_Open")
            {
                if (planeSelected.tag == "PlacementPlane_Selected")
                {
                    planeSelected.tag = "PlacementPlane_Open";
                    buildPanelOpen = false;
                    upgradePanelOpen = false;
                    planeSelected = null;
                } else if (lastHitObj.tag == "PlacementPlane_TakenSelected")
                {
                    lastHitObj.tag = "PlacementPlane_Taken";
                    buildPanelOpen = false;
                    upgradePanelOpen = false;
                    planeSelected = null;
                }
            }
        }
	}

    void ToggleBuildPanel()
    {
        if (buildPanelOpen)
            buildPanelTweener.Play(true);
        else
            buildPanelTweener.Play(false);

        if (upgradePanelOpen)
            upgradePanelTweener.Play(true);
        else
            upgradePanelTweener.Play(false);
    }

    void SetBuildChoice(GameObject btnChoice)
    {
        string name = btnChoice.name;

        // yield return new WaitForFixedUpdate();
        GameObject test = (GameObject)Instantiate(testCollider, planeSelected.transform.position, Quaternion.identity);
        GraphUpdateObject guo = new GraphUpdateObject(test.GetComponent<Collider>().bounds);
        
        bool pathBlocked = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, AstarPath.active.GetNearest(spawnPoint.transform.position).node, 
                                                                    AstarPath.active.GetNearest(aiTarget.transform.position).node, false);
        //pathfinding.Scan();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<TemporaryEnemyScript>().GetNewPath();
        }
        
        if (pathBlocked)
        {
            buildTower(name);
            
        } else if (!pathBlocked)
        {
            Debug.Log("Blocked");
            planeSelected.tag = "PlacementPlane_Open";
            Destroy(test);
            
            buildPanelOpen = false;
            planeSelected = null;
            lastHitObj = null;
        }
    }

    void buildTower(string name)
    {
        if (money > lowestCost)
        {
            if (name == "Button_Turret" && planeSelected.tag == "PlacementPlane_Selected")
            {
                GameObject go = (GameObject)Instantiate(allStructures [0], planeSelected.transform.position, Quaternion.identity);
                planeSelected.GetComponent<PlacementPlane>().myStructure = go;
                money -= go.GetComponent<Turret>().cost;
                
            } else if (name == "Button_AirTurret" && planeSelected.tag == "PlacementPlane_Selected" && money >= 150)
            {
                GameObject go = (GameObject)Instantiate(allStructures [1], planeSelected.transform.position, Quaternion.identity); 
                planeSelected.GetComponent<PlacementPlane>().myStructure = go;
                money -= go.GetComponent<Mortar>().cost;
            }
        }
        
        planeSelected.tag = "PlacementPlane_Taken";
        
        
        
        buildPanelOpen = false;
        planeSelected = null;
        lastHitObj = null;
    }

    void Sell()
    {
        if (planeSelected.tag == "PlacementPlane_TakenSelected")
        {
            GameObject tower = planeSelected.GetComponent<PlacementPlane>().myStructure;

            AstarPath.RegisterSafeUpdate (delegate () {
                GraphNode node = AstarPath.active.GetNearest(tower.transform.position).node;
                node.walkable = true;
            },true);

            switch(tower.name)
            {
                case "Turret Tower(Clone)":
                    money += (tower.GetComponent<Turret>().cost / 2);
                    break;
                case "Air Turret(Clone)":
                    money += (tower.GetComponent<AirTurret>().cost / 2);
                    break;
                case "Air Turret2(Clone)":
                    money += (tower.GetComponent<AirTurret>().cost / 2);
                    break;
            }
            Destroy(tower);
            planeSelected.tag = "PlacementPlane_Open";
            planeSelected = null;
            upgradePanelOpen = false;
        }
    }

    void Upgrade()
    {
        if (planeSelected.tag == "PlacementPlane_TakenSelected")
        {
            GameObject tower = planeSelected.GetComponent<PlacementPlane>().myStructure;

            if(tower.tag == "AirTurret")
            {
                if(money > lowestCost)
                {
                    Destroy(tower);
                    GameObject go = (GameObject) Instantiate(upgradeStructures[1], planeSelected.transform.position, Quaternion.identity); 
                    planeSelected.GetComponent<PlacementPlane>().myStructure = go;
                    money -= go.GetComponent<AirTurret>().cost;
                }

                upgradePanelOpen = false;
                planeSelected.tag = "PlacementPlane_Taken";
                planeSelected = null;
                lastHitObj = null;
            }
        }
    }
}






















