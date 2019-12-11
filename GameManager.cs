using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Game Manager variables
    [FoldoutGroup("Set Up", false)]
    public GameObject Dino;
    [FoldoutGroup("Set Up", false)]
    public Transform Target;
    [FoldoutGroup("Set Up", false)]
    public Transform OldTarget;

    [Space]
    [FoldoutGroup("Set Up" , false)]
	public List<GameObject> Children;
    [FoldoutGroup("Set Up", false)]
    public Transform DinoHolder;
    [FoldoutGroup("Set Up", false)]
    public Animator DinoAnimator;
    [FoldoutGroup("Set Up", false)]
    public GameObject CurrentCell;

    [Space]
    [InfoBox("Input number of rows 'X' and collums 'Z'")]
    [FoldoutGroup("Set Up", false)]
    [ReadOnly] public int RoadID;
    [FoldoutGroup("Set Up", false)]
    public int RowsX;
    [FoldoutGroup("Set Up", false)]
    public int ColsZ;

    [Space]
    [InfoBox("Input position of Dino at the start")]
    [FoldoutGroup("Set Up", false)]
    public int Dino1x;
    [FoldoutGroup("Set Up", false)]
    public int Dino1z;
    [FoldoutGroup("Set Up", false)]
    [ReadOnly] public int OldDino1x;
    [FoldoutGroup("Set Up", false)]
    [ReadOnly] public int OldDino1z;

	private bool isOver;

    [TabGroup("Can Go Directions")]
    [ReadOnly] public bool CanGoFwd;
    [TabGroup("Can Go Directions")]
    [ReadOnly] public bool CanGoBck;
    [TabGroup("Can Go Directions")]
    [ReadOnly] public bool CanGoLft;
    [TabGroup("Can Go Directions")]
    [ReadOnly] public bool CanGoRgt;

    [TabGroup("Is Going Directions")]
    [ReadOnly] public bool goingFwd;
    [TabGroup("Is Going Directions")]
    [ReadOnly] public bool goingBck;
    [TabGroup("Is Going Directions")]
    [ReadOnly] public bool goingLft;
    [TabGroup("Is Going Directions")]
    [ReadOnly] public bool goingRgt;

    [Space]
    [FoldoutGroup("Set Up", false)]
    public int speed = 4;

    [Space]
    [Header("Cell Management")]
    [FoldoutGroup("Set Up", false)]
    [ReadOnly] public CellScript[,] cellsS;
    [FoldoutGroup("Set Up", false)]
    [ReadOnly] public GameObject[,] cells;

    [TabGroup("Initiate Directions")]
    [ReadOnly] public bool initiateMoveLft;
    [TabGroup("Initiate Directions")]
    [ReadOnly] public bool initiateMoveRgt;
    [TabGroup("Initiate Directions")]
    [ReadOnly] public bool initiateMoveFwd;
    [TabGroup("Initiate Directions")]
    [ReadOnly] public bool initiateMoveBck;

    [TabGroup("Old Can Go Directions")]
    [ReadOnly] public bool oldCanGoFwd;
    [TabGroup("Old Can Go Directions")]
    [ReadOnly] public bool oldCanGoBck;
    [TabGroup("Old Can Go Directions")]
    [ReadOnly] public bool oldCanGoLft;
    [TabGroup("Old Can Go Directions")]
    [ReadOnly] public bool oldCanGoRgt;

    [FoldoutGroup("Organizer", false)]
    public Mesh meshStraight;
    [FoldoutGroup("Organizer", false)]
    public Mesh meshL;
    [FoldoutGroup("Organizer", false)]
    public Mesh meshT;
    [FoldoutGroup("Organizer", false)]
    public Mesh meshX;

    [FoldoutGroup("Rotation" , false)]
    [ReadOnly] public bool smoothRotation = true;
    [FoldoutGroup("Rotation", false)]
    [ReadOnly] public bool absRotation = false;
    [FoldoutGroup("Rotation", false)]
    [ReadOnly] public bool noRotation = false;
    [FoldoutGroup("Rotation", false)]
    [ReadOnly] public Vector3 targetRot;
    [FoldoutGroup("Rotation", false)]
    [Range(0,1)]
    public float rotSpeed = 0.25f;
    #endregion

    #region GameObject variables

    [Header("Slots / Indicators / Canvas / etc.")]
    [TitleGroup("Locate Important Scripts")]
    public Transform indicatorParent;
    [TitleGroup("Locate Important Scripts")]
    public SlotManager slotManager;
    [TitleGroup("Locate Important Scripts")]
    public SpecialMeter specialMeter;
    [TitleGroup("Locate Important Scripts")]
    public Image dinoImage;
    [TitleGroup("Locate Important Scripts")]
    public Text successfulDisplay;
    [TitleGroup("Locate Important Scripts")]
    public Text failedDisplay;
    [TitleGroup("Locate Important Scripts")]
    public AudioSource music;
    [TitleGroup("Locate Important Scripts")]
    public GameObject scoreMultiplied;

    [Space]
    [Header("Game Management")]
    [TitleGroup("Locate Important Scripts")]
    public NewDeliveryPointManager dpManager;
    [TitleGroup("Locate Important Scripts")]
    public NewGameValues gameValues;
    [TitleGroup("Locate Important Scripts")]
    [InfoBox("Use Game Over Screen to edit win / losing values")]
    public NewGameOverScreen gameOverScreen;
    [TitleGroup("Locate Important Scripts")]
    public TapController tapController;

    [Space]
    [Header("Cameras")]
    [TitleGroup("Locate Important Scripts")]
    public Camera mainCam;
    [TitleGroup("Locate Important Scripts")]
    public Camera uiCam;
    [TitleGroup("Locate Important Scripts")]
    public Camera dinoCam;
    [TitleGroup("Locate Important Scripts")]
    public RenderTexture uiRendTex;
    [TitleGroup("Locate Important Scripts")]
    public RenderTexture dinoRendTex;

    [Space]
    [Header("Pterodactyls")]
    [TitleGroup("Locate Important Scripts")]
    public List<PteroDactyl> pteros;

    [Button("SetUp Values" , ButtonSizes.Large)]
    public void SetValues()
    {

        //Deal with Camera's and render textures
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("ViewUICam").GetComponent<Camera>();
        dinoCam = GameObject.Find("ViewDinoCam").GetComponent<Camera>();
        uiRendTex = GameObject.Find("ViewUIRendTex").GetComponent<RenderTexture>();
        dinoRendTex = GameObject.Find("ViewDinoRendTex").GetComponent<RenderTexture>();
        if (dinoRendTex != null && uiRendTex != null)
        {
            uiCam.targetTexture = uiRendTex; dinoCam.targetTexture = dinoRendTex;
        }

        //Deal with Scripts
        NewGameOverScreen ngo = FindObjectOfType<NewGameOverScreen>();
        gameOverScreen = ngo; tapController = FindObjectOfType<TapController>();

        NewGameValues gv = FindObjectOfType<NewGameValues>();
        gameValues = gv; gv.TimeGO = GameObject.Find("Time Display");
        if(gv.TimeGO != false) gv.TimeTXT = gv.TimeGO.GetComponent<Text>();
        gameValues.ScoreTXT = GameObject.Find("YourScore").GetComponent<Text>();
        gameValues.GameOverScreen = ngo.transform.gameObject;
        gameOverScreen.GV = gameValues; if (music != null) gameOverScreen.Music = music;
        ngo.GV = gv; gv.NGOS = ngo;

        SpecialMeter sm = FindObjectOfType<SpecialMeter>();
        specialMeter = sm; sm.AS = music.GetComponent<AudioSource>();
        sm.MultiPlyUI = GameObject.Find("ScoreMultiplied");
        scoreMultiplied = sm.MultiPlyUI; sm.GV = gameValues;
        sm.NormalTrailRend = GameObject.Find("TR1");
        sm.SpecialTrailRend = GameObject.Find("TR2");
        specialMeter.GM = this; specialMeter.GV = gameValues;
        if (gv.boostr != null) gv.boostr.SM = specialMeter; gv.boostr.GV = gv;

        //Complex dealings
        NewDeliveryPointManager dpm = FindObjectOfType<NewDeliveryPointManager>();
        if (dpm != null)
        {
            dpManager = dpm; dpManager.SM = specialMeter; sm.DPmanager = dpManager;
        }

        SlotManager slot = FindObjectOfType<SlotManager>();
        slotManager = slot; gv.SM = slotManager; slot.GV = gameValues;

        foreach (Transform child in dpManager.transform)
        {
            NewDeliveryPoint dp = child.GetComponent<NewDeliveryPoint>();
            dp.SlotMgr = slot;
        }

        Image img = GameObject.Find("DinoImage").GetComponent<Image>();
        dinoImage = img; gameValues.CharImg = img;

        specialMeter.Indicators.Clear();
        foreach(Transform child in indicatorParent)
            specialMeter.Indicators.Add(child.gameObject.GetComponent<Image>());

        gameValues.SDelGO = GameObject.Find("Successful Deliveries");
        foreach (Transform t in gameValues.SDelGO.transform)
        {
            if (t.name == "SDel_Txt")
            {
                successfulDisplay = t.gameObject.GetComponent<Text>();
                gameValues.SDelTXT = successfulDisplay;
            }
        }
        gameValues.FDelGO = GameObject.Find("Failed Deliveries");
        foreach(Transform t in gameValues.FDelGO.transform)
                {
            if (t.name == "FDel_Txt")
            {
                failedDisplay = t.gameObject.GetComponent<Text>();
                gameValues.FDelTXT = failedDisplay;
            }

        }//Pterodactyls
        GameObject ptero = GameObject.Find("Pterodactyls");
        if (ptero != false)
        {
            pteros.Clear();
            foreach (Transform child in ptero.transform) pteros.Add(child.GetComponent<PteroDactyl>());
            GameObject stolenUI = GameObject.Find("PostStolen"); GameObject shakeUI = GameObject.Find("ShakePrompt");
            Image shake = GameObject.Find("ShakeMeter").GetComponent<Image>(); GameObject pteroNice = GameObject.Find("Nice");
            foreach (PteroDactyl pt in pteros)
            {
                pt.DPmanager = dpManager; pt.SManager = slotManager; pt.StolenUI = stolenUI; pt.ShakePrompt = shakeUI;
                pt.ShakeMeter = shake; pt.Nice = pteroNice;
            }
        }
    }
    #endregion






    [FoldoutGroup("Rotation", false)]
    [Button("Activate Smooth Rotation" , ButtonSizes.Small)]
    public void SmoothRotate()
    {
        smoothRotation = true;
        absRotation = false;
        noRotation = false;
    }
    [FoldoutGroup("Rotation", false)]
    [Button("Activate Absolute Rotation", ButtonSizes.Small)]
    public void AbsRotate()
    {
        smoothRotation = false;
        absRotation = true;
        noRotation = false;
    }
    [FoldoutGroup("Rotation", false)]
    [Button("Disable Rotation", ButtonSizes.Small)]
    public void NoRotate()
    {
        smoothRotation = false;
        absRotation = false;
        noRotation = true;
    }

    [FoldoutGroup("Organizer", false)]
    [Button("Organise", ButtonSizes.Large)]
    public void Organise()
    {
        CollectChildren();
        foreach (GameObject GO in Children)
        {
            if(GO.GetComponent<CellScript>() != null)
            {
                CellScript cs = GO.GetComponent<CellScript>();
                //makes sure a mesh filter is connected
                if (cs.meshF == null)
                {
                    cs.meshF = cs.transform.gameObject.GetComponent<MeshFilter>();
                }

                //Sets the mesh
                if (cs.is_L_road)
                {
                    cs.meshF.mesh = meshL;
                }
                else if (cs.is_T_road)
                {
                    cs.meshF.mesh = meshT;
                }
                else if (cs.is_X_road)
                {
                    cs.meshF.mesh = meshX;
                }
                else if (cs.is_Straight_road)
                {
                    cs.meshF.mesh = meshStraight;
                }
            }
        }
    }

    [FoldoutGroup("Organizer", false)]
    [Button("Fix L roads", ButtonSizes.Large)]
    public void FixLroads()
    {
        foreach (GameObject GO in Children)
        {
            if (GO.GetComponent<CellScript>() != null)
            {
                CellScript cs = GO.GetComponent<CellScript>();
                if (cs.is_L_road)
                {
                    float newRot = GO.transform.localRotation.y - 90;
                    if (newRot < -45f)
                    {
                        newRot = 270f;
                    }
                    GO.transform.localRotation = Quaternion.Euler(0f, newRot, 0f);
                }
            }
        }
    }
    // Use this for initialization
    void Awake () {
        //SetValues();
        MusicSource ms = FindObjectOfType<MusicSource>();
        if (ms != null)
        {
            music = ms.audioS; specialMeter.AS = music; gameOverScreen.Music = music;
        }
        dpManager.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false); scoreMultiplied.SetActive(false);

        ChangeChar();
		CollectChildren();
		Target = cells[(int)(Dino.transform.position.x)/2,(int)(Dino.transform.position.z)/2].transform;
		Dino.transform.position = Target.transform.position;
	}

	void ChangeChar(){

		speed += CharacterSelect.CharacterSpeed;

		if (CharacterSelect.CurrentCharacterInt != 0) {

			Transform NewTrans = DinoAnimator.transform;
			Destroy (DinoAnimator.transform.gameObject);
			GameObject go = (GameObject)Instantiate(CharacterSelect.SelectedCharacter) as GameObject;
			go.transform.parent = DinoHolder;
			go.transform.position = NewTrans.position;
			go.transform.rotation = NewTrans.rotation;
			DinoAnimator = go.GetComponent<Animator> ();
		}
	}

	void Start(){
        Debug.Log("START");
        OldDino1x = Dino1x;
        OldDino1z = Dino1z;
        UpdateCellInfo ();
	}

    void CheckIfOver()
    {

        OldTarget = cellsS[OldDino1x, OldDino1z].gameObject.transform;

		if (goingBck) {
			isOver |= ((Vector3.Distance(Dino.transform.position, OldTarget.position) < 1f) && (Dino.transform.position.z < OldTarget.position.z));
		}
		if (goingFwd) {
			isOver |= ((Vector3.Distance(Dino.transform.position, OldTarget.position) < 1f) && (Dino.transform.position.z > OldTarget.position.z));
		}
		if (goingRgt) {
			isOver |= ((Vector3.Distance(Dino.transform.position, OldTarget.position) < 1f) && (Dino.transform.position.x > OldTarget.position.x));
		}
		if (goingLft) {
			isOver |= ((Vector3.Distance(Dino.transform.position, OldTarget.position) < 1f) && (Dino.transform.position.x < OldTarget.position.x));
		}
	}
	public void ReverseDr(){
		if (goingLft) {
			MoveRgt();
		}
		if (goingRgt) {
			MoveLft();
		}
		if (goingFwd) {
			MoveBck ();
		}
		if (goingBck){
			MoveFwd ();
		}
	}
	public void TriggerFWD(){
		

        if (goingFwd) {                 // If going forward already
			return;                     // Then do nothing
		} else if (goingBck) {          //If going back
			MoveFwd ();                 // Then go forward
		} else {
			CheckIfOver ();
            if (isOver)
            {
                if (oldCanGoFwd)
                {
					ReverseDr ();       //goes back to old position before initiating move fwd
				}
				isOver = false;
			}
			initiateMoveFwd = true;
			initiateMoveBck = false;
			initiateMoveLft = false;
			initiateMoveRgt = false;
		}
	}

	public void TriggerBCK(){
		if (goingBck) {
			return;
		} else if (goingFwd) {
			MoveBck ();
		} else {
			CheckIfOver ();
			if (isOver) {
				if (oldCanGoBck) {
					ReverseDr ();
				}
				isOver = false;
			}
			initiateMoveFwd = false;
			initiateMoveBck = true;
			initiateMoveLft = false;
			initiateMoveRgt = false;
		}
	}

	public void TriggerLFT(){
		if (goingLft) {
			return;
		} else if (goingRgt) {
			MoveLft ();
		}else{
			CheckIfOver ();
			if (isOver) {
				if (oldCanGoLft) {
					ReverseDr ();
				}
				isOver = false;
			}
			initiateMoveFwd = false;
			initiateMoveBck = false;
			initiateMoveLft = true;
			initiateMoveRgt = false;
		}
	}
	public void TriggerRGT(){
		if (goingRgt) {
			return;
		} else if (goingLft) {
			MoveRgt ();
		}else{
			CheckIfOver ();
			if (isOver) {
				if (oldCanGoRgt) {
					ReverseDr ();
				}
				isOver = false;
			}
			initiateMoveFwd = false;
			initiateMoveBck = false;
			initiateMoveLft = false;
			initiateMoveRgt = true;
		}
	}

	void DisableTriggers(){
		initiateMoveFwd = false;
		initiateMoveBck = false;
		initiateMoveLft = false;
		initiateMoveRgt = false;
	}

	void CorrectPos(){
		if (Dino1x > RowsX-1) {
			Dino1x = RowsX-1;
		} else if (Dino1x < 0) {
			Dino1x = 0;
		}

		if (Dino1z > ColsZ-1) {
			Dino1z = ColsZ-1;
		}else if (Dino1z < 0) {
			Dino1z = 0;
		}
	}

	public void MoveFwd(){
		if (CanGoFwd) {
            targetRot = new Vector3 (0, 0, 0);
            if (absRotation)
            {
                DinoHolder.localEulerAngles = targetRot;
            }
			if (Dino1z < ColsZ) {
				OldDino1z = Dino1z;
				OldDino1x = Dino1x;
				Dino1z++;
			}
			CorrectPos ();
			Target = cells [Dino1x, (Dino1z)].transform;
			goingFwd = true;

            goingBck = false;
			goingRgt = false;
			goingLft = false;

			UpdateCellInfo ();
		}
	}
	public void MoveBck(){
		if (CanGoBck) {
            targetRot = new Vector3(0, 180, 0);
            if (absRotation)
            {
                DinoHolder.localEulerAngles = targetRot;
            }
			if (Dino1z > 0) {
				OldDino1z = Dino1z;
				OldDino1x = Dino1x;
				Dino1z--;
			}
			CorrectPos ();
			Target = cells [Dino1x, (Dino1z)].transform;
			goingBck = true;

			goingFwd = false;
			goingLft = false;
			goingRgt = false;

			UpdateCellInfo ();
		}
	}
	public void MoveLft(){
		if (CanGoLft) {
            targetRot = new Vector3(0, 270, 0);
            if (absRotation)
            {
                DinoHolder.localEulerAngles = targetRot;
            }
			if (Dino1x > 0) {
				OldDino1x = Dino1x;
				OldDino1z = Dino1z;
				Dino1x--;
			}
			CorrectPos ();
			Target = cells [Dino1x, (Dino1z)].transform;
			goingLft = true;

			goingFwd = false;
			goingRgt = false;
			goingBck = false;

			UpdateCellInfo ();
		}
	}
	public void MoveRgt(){
		if (CanGoRgt) {
            targetRot = new Vector3(0, 90, 0);
            if (absRotation)
            {
                DinoHolder.localEulerAngles = targetRot;
            }
			if (Dino1x < RowsX) {
				OldDino1x = Dino1x;
				OldDino1z = Dino1z;
				Dino1x++;
			}
			CorrectPos ();
			Target = cells [Dino1x, Dino1z].transform;
			goingRgt = true;

			goingFwd = false;
			goingLft = false;
			goingBck = false;

			UpdateCellInfo ();

		}
	}

	public void UpdateCellInfo(){
		CurrentCell = cellsS[Dino1x,Dino1z].gameObject;
		RoadID = cellsS[Dino1x,Dino1z].RoadID;
		CalculatePosDirections ();
	}

	public void CollectChildren()
	{
        Children.Clear();
        foreach (Transform child in transform) {
			Children.Add (child.gameObject);
		}

		cells = new GameObject[RowsX, ColsZ];
		cellsS = new CellScript[RowsX, ColsZ];
		foreach (GameObject Cell in Children) {
			int xPos = (int)(Cell.transform.localPosition.x);
			int zPos = (int)(Cell.transform.localPosition.z);

			cells [(xPos / 2), (zPos / 2)] = Cell;
			cellsS [(xPos / 2), (zPos / 2)] = cells [(xPos / 2), (zPos / 2)].GetComponent<CellScript> ();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		CorrectPos ();

        if (smoothRotation)
        {
            float t = rotSpeed * Time.deltaTime;
            DinoHolder.transform.localRotation = 
                Quaternion.RotateTowards(DinoHolder.transform.localRotation, Quaternion.Euler(targetRot), rotSpeed / Time.deltaTime);
        }

		float step = speed * Time.deltaTime;
		if (Dino.transform.position != Target.position) {
			Dino.transform.position = Vector3.MoveTowards (Dino.transform.position, Target.position, step);

			DinoAnimator.SetBool ("running", true);
		} else {
			DinoAnimator.SetBool ("running", false);
		}

		if (Vector3.Distance (Dino.transform.position, Target.position) < 0.035f) {
			Dino.transform.position = Target.position;
			CurrentCell = Target.gameObject;

			if (initiateMoveBck && CanGoBck) {
				MoveBck ();
				DisableTriggers ();
			} else if (initiateMoveFwd && CanGoFwd) {
				MoveFwd ();
				DisableTriggers ();
			} else if (initiateMoveLft && CanGoLft) {
				MoveLft ();
				DisableTriggers ();
			} else if (initiateMoveRgt && CanGoRgt) {
				MoveRgt ();
				DisableTriggers ();
			} else {
				if (CanGoFwd && goingFwd) {
					MoveFwd ();
				} else {
					goingFwd = false;
				}

				if (CanGoBck && goingBck) {
					MoveBck ();
				} else {
					goingBck = false;
				}

				if (CanGoLft && goingLft) {
					MoveLft ();
				} else {
					goingLft = false;
				}

				if (CanGoRgt && goingRgt) {
					MoveRgt ();
				} else {
					goingRgt = false;
				}
			}
		}
	}

	public void CalculatePosDirections(){

		oldCanGoFwd = CanGoFwd;
		oldCanGoBck = CanGoBck;
		oldCanGoLft = CanGoLft;
		oldCanGoRgt = CanGoRgt;

		float Yrot = CurrentCell.transform.localEulerAngles.y;

        // Checks ID of the road and the orientation of it
        // Sets directions player can go accordingly

        //I road
		if (RoadID == 1) {
			if (Mathf.Abs(Yrot - 0) < (5)) {
				CanGoFwd = false;
				CanGoBck = false;
				CanGoLft = true; 
				CanGoRgt = true;
			} else {
				CanGoFwd = true;
				CanGoBck = true;
				CanGoLft = false; 
				CanGoRgt = false;
			}
		}
        //L road
		else if (RoadID == 2) {
			if (Mathf.Abs(Yrot - 270) < (5)) {
				CanGoFwd = false;
				CanGoBck = true;
				CanGoLft = false; 
				CanGoRgt = true;
			}
			else if (Mathf.Abs(Yrot - 0) < (5)) {
				CanGoFwd = false;
				CanGoBck = true;
				CanGoLft = true; 
				CanGoRgt = false;
			}
			else if (Mathf.Abs(Yrot - 90) < (5)) {
				CanGoFwd = true;
				CanGoBck = false;
				CanGoLft = true; 
				CanGoRgt = false;
			}
			else if (Mathf.Abs(Yrot - 180) < (5)) {
				CanGoFwd = true;
				CanGoBck = false;
				CanGoLft = false; 
				CanGoRgt = true;
			}
		}
        //T road
		else if (RoadID == 3) {
			if (Mathf.Abs(Yrot - 270) < (5)) {
				CanGoFwd = false;
				CanGoBck = true;
				CanGoLft = true; 
				CanGoRgt = true;
			}
			if (Mathf.Abs(Yrot - 0) < (5)) {
				CanGoFwd = true;
				CanGoBck = true;
				CanGoLft = true; 
				CanGoRgt = false;
			}
			if (Mathf.Abs(Yrot - 90) < (5)) {
				CanGoFwd = true;
				CanGoBck = false;
				CanGoLft = true; 
				CanGoRgt = true;
			}
			if (Mathf.Abs(Yrot - 180) < (5)) {
				CanGoFwd = true;
				CanGoBck = true;
				CanGoLft = false; 
				CanGoRgt = true;
			}
		}
        //X road
		else if (RoadID == 4) {
			CanGoBck = true;
			CanGoFwd = true;
			CanGoLft = true;
			CanGoRgt = true;
		}

		if (cellsS[Dino1x,Dino1z].OneWay) {
			CanGoBck &= !cellsS[Dino1x, Dino1z].DisableBck;
			CanGoFwd &= !cellsS[Dino1x, Dino1z].DisableFwd;
			CanGoLft &= !cellsS[Dino1x, Dino1z].DisableLft;
			CanGoRgt &= !cellsS[Dino1x, Dino1z].DisableRgt;
		}
		CurrentCell = cells [(int)(Dino.transform.position.x) / 2, (int)(Dino.transform.position.z) / 2];
	}
}