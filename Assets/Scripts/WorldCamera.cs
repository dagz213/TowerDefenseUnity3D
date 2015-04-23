using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class WorldCamera : MonoBehaviour {

    GameObject plane;
    GameObject temp;

    public Texture2D cursorTexture;
	private float xAxis;
	private float yAxis;

	//box limit Struct
	public struct BoxLimit {
		public float LeftLimit;
		public float RightLimit;
		public float TopLimit;
		public float BottomLimit;
	}

	//Variables
	public static BoxLimit cameraLimits = new BoxLimit();
	public static BoxLimit mouseScrollLimits = new BoxLimit();
	public static WorldCamera Instance;

	private float cameraMoveSpeed = 60f;
	private float shiftBonus = 45f;
    private float mouseBoundary = 25f;

	void Awake()
    {
        Instance = this;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 16, 16), cursorTexture);
    }


	// Use this for initialization
	void Start () {
        plane = GameObject.Find("td1");
        //Screen.fullScreen = true;
        cameraLimits.LeftLimit = plane.transform.position.x - (plane.GetComponent<Renderer>().bounds.size.x / 8);
        cameraLimits.RightLimit = plane.transform.position.x + (plane.GetComponent<Renderer>().bounds.size.x / 6);
        cameraLimits.TopLimit = plane.transform.position.z + (plane.GetComponent<Renderer>().bounds.size.z / 6);
        cameraLimits.BottomLimit = plane.transform.position.z - (plane.GetComponent<Renderer>().bounds.size.z / 2.8f);

        mouseScrollLimits.LeftLimit = mouseBoundary;
        mouseScrollLimits.RightLimit = mouseBoundary;
        mouseScrollLimits.TopLimit = mouseBoundary;
        mouseScrollLimits.BottomLimit = mouseBoundary;
	}
	
	// Update is called once per frame
	void Update () {
	    if (CheckIfUserCameraInput())
        {
            Vector3 cameraDesiredMove = GetDesiredTranslation();

            if(!isDesiredPositionOverBoundaries(cameraDesiredMove))
            {
                this.transform.Translate(cameraDesiredMove, Space.World);
            }
        }

        Scroll();
	}

    public bool CheckIfUserCameraInput()
    {
        bool keyboardMove;
        bool mouseMove;
        bool canMove;

        //check Keyboard
        if (WorldCamera.AreCameraKeyboardButtonsPressed())
            keyboardMove = true;
        else
            keyboardMove = false;


        //check mouse position
        if (WorldCamera.IsMousePositionWithinBoundaries())
            mouseMove = true;
        else
            mouseMove = false;

        if (keyboardMove || mouseMove)
            canMove = true;
        else
            canMove = false;

        return canMove;
    }

    public void Scroll()
    {
        float moveSpeed = cameraMoveSpeed * Time.deltaTime;
        float desiredY = 0f;
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel > 0)
            desiredY = moveSpeed * -1;
        if (wheel < 0)
            desiredY = moveSpeed;

        this.transform.Translate(0, desiredY, 0, Space.World);
    }

    public Vector3 GetDesiredTranslation()
    {
        float moveSpeed = 0f;
        float desiredX = 0f;

        float desiredZ = 0f;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            moveSpeed = (cameraMoveSpeed + shiftBonus) * Time.deltaTime;
        else
            moveSpeed = cameraMoveSpeed * Time.deltaTime;

        //move via keyboard
        if (Input.GetKey(KeyCode.W))
            desiredZ = moveSpeed;
		

        if (Input.GetKey(KeyCode.A))
            desiredX = moveSpeed * -1;

        if (Input.GetKey(KeyCode.S))
            desiredZ = moveSpeed * -1;

        if (Input.GetKey(KeyCode.D))
            desiredX = moveSpeed;

        //move via mouse
        if (Input.mousePosition.x < mouseScrollLimits.LeftLimit) {
			desiredX = moveSpeed * -1;
		}

        if(Input.mousePosition.x > (Screen.width - mouseScrollLimits.RightLimit))
            desiredX = moveSpeed;

        if(Input.mousePosition.y < mouseScrollLimits.BottomLimit)
            desiredZ = moveSpeed * -1;

        if(Input.mousePosition.y > (Screen.height - mouseScrollLimits.TopLimit))
            desiredZ = moveSpeed;



        return new Vector3(desiredX, 0, desiredZ);
    }

    public bool isDesiredPositionOverBoundaries(Vector3 desiredPosition)
    {
        bool overBoundaries = false;

        //check Boundaries
        //Left Boundaries
        if ((this.transform.position.x + desiredPosition.x) < cameraLimits.LeftLimit)
            overBoundaries = true;

        //Right Boundaries
        if ((this.transform.position.x + desiredPosition.x) > cameraLimits.RightLimit)
            overBoundaries = true;

        //Top Boundaries
        if ((this.transform.position.z + desiredPosition.z) > cameraLimits.TopLimit)
            overBoundaries = true;

        //Bottom Boundaries
        if ((this.transform.position.z + desiredPosition.z) < cameraLimits.BottomLimit)
            overBoundaries = true;

        return overBoundaries;
    }

    //Helper Functions
    public static bool AreCameraKeyboardButtonsPressed()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            return true;
        else
            return false;
    }

    public static bool IsMousePositionWithinBoundaries()
    {
        if (
            (Input.mousePosition.x < mouseScrollLimits.LeftLimit && Input.mousePosition.x > -5) ||
            (Input.mousePosition.x > (Screen.width - mouseScrollLimits.RightLimit) && Input.mousePosition.x < (Screen.width + 5)) ||
            (Input.mousePosition.y < mouseScrollLimits.BottomLimit && Input.mousePosition.y > -5) ||
            (Input.mousePosition.y > (Screen.height - mouseScrollLimits.TopLimit) && Input.mousePosition.y < (Screen.height + 5))
            )
            return true;
        else
            return false;
    }












}
