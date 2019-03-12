using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_Jelly : MonoBehaviour
{

    private const float c_fOriginOffset = 0.9f;
    private Vector2 v2GridOrigin;
    [SerializeField]
    Vector2 v2Dir;
    [SerializeField]
    GameObject Particles;
    [SerializeField]
    GameObject InsideJellies;
    private bool bHasParticles = false;
    private bool bMoveJelly = false;
    GameObject newJelly = null;
    private float fSpeed = 0.07f;
    public bool bMatched = false;
    GameObject shine;
    public Vector2 v2BoardPos;
    private float fJellyRadius = 1.45f;
    Vector2 v2newDir;
    RaycastHit2D hit;
    private const int c_iXGridMax = 8;
    private const int c_iYGridMax = 4;
    private const int c_iGridMin = 0;
    private int iNumOfSame = 0;
    private int iNumOfNone = 0;
    public Matrix9x9.JELLYTYPE Colour;
    private int iWorthOfJelly = 50;
    private bool bCanClick = true;
    [SerializeField]
    private bool bCanBeNoSpace;
    [SerializeField]
    private int iNoSpaceVal;
    private GameObject OriginJelly;
    public string sHitName;
    // Use this for initialization
    void Start()
    {
        OriginJelly = GameObject.FindWithTag("Origin");
        v2GridOrigin = OriginJelly.transform.position;
        if (gameObject.tag == "InJelly")
        {
            SetJellyIntoGrid();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.tag == "InJelly")
        {
            if (CS_GameManager.instance.GridSystem.bRotating == false)
            {
                StartCoroutine("CheckMatches");
            }
        }
        else
        {
            StartCoroutine("RaycastUpdateCanShoot");
            MoveJelly();
        }

    }

    private void Update()
    {
        if (bMatched)
        {
            CS_GameManager.instance.GridSystem.ClearPosition(v2BoardPos);
            CS_GameManager.instance.iJellyNum--;
            CS_GameManager.instance.bPlaySound = true;
            Destroy(gameObject);
        }
    }


    private IEnumerator CheckMatches()
    {
        if (gameObject.name == "Yellow (3)")
        {
            //2
            Debug.Log("d");
        }
            GameObject[] aroundJellies = new GameObject[4];
        //Get left from gameobject
        aroundJellies[0] = CS_GameManager.instance.GridSystem.GetGameObject((int)v2BoardPos.x - 1, (int)v2BoardPos.y);
        //Right
        aroundJellies[1] = CS_GameManager.instance.GridSystem.GetGameObject((int)v2BoardPos.x + 1, (int)v2BoardPos.y);
        //Up
        aroundJellies[2] = CS_GameManager.instance.GridSystem.GetGameObject((int)v2BoardPos.x, (int)v2BoardPos.y - 1);
        //Down
        aroundJellies[3] = CS_GameManager.instance.GridSystem.GetGameObject((int)v2BoardPos.x, (int)v2BoardPos.y + 1);
        iNumOfSame = 0;
        iNumOfNone = 0;
        int iNum = 0;
        Matrix9x9.JELLYTYPE col = Matrix9x9.JELLYTYPE.NONE;

        bool[] iValidJellies = new bool[4] { false, false, false, false };
        int iMost = 0;

        if (Colour == Matrix9x9.JELLYTYPE.MULTI)
        {
            int[] iTempOfSame = new int[4] { 0, 0, 0, 0 };
            for(int i = 1; i < 4; i++)
            {
                if(aroundJellies[i])
                {
                    Matrix9x9.JELLYTYPE c = aroundJellies[i].GetComponent<CS_Jelly>().Colour;
                    if(aroundJellies[0])
                    {
                        if (c == aroundJellies[0].GetComponent<CS_Jelly>().Colour || c == Matrix9x9.JELLYTYPE.MULTI)
                        {
                            iTempOfSame[0]++;
                        }
                    }
                    if(aroundJellies[1])
                    {
                        if (c == aroundJellies[1].GetComponent<CS_Jelly>().Colour || c == Matrix9x9.JELLYTYPE.MULTI)
                        {
                            iTempOfSame[1]++;
                        }
                    }
                    if(aroundJellies[2])
                    {
                        if (c == aroundJellies[2].GetComponent<CS_Jelly>().Colour || c == Matrix9x9.JELLYTYPE.MULTI)
                        {
                            iTempOfSame[2]++;
                        }
                    }
                    if(aroundJellies[3])
                    {
                        if (c == aroundJellies[3].GetComponent<CS_Jelly>().Colour || c == Matrix9x9.JELLYTYPE.MULTI)
                        {
                            iTempOfSame[3]++;
                        }
                    }
                }
            }
            for(int i = 0; i < 4; i++)
            {
                if(iTempOfSame[i] > iMost)
                {
                    iMost = i;
                }
            }
            //Check if multiple around it are the same
        }
        foreach (GameObject a_jelly in aroundJellies)
        {
            if (a_jelly)
            {
                
                if(this.Colour == Matrix9x9.JELLYTYPE.MULTI)
                {
                    col = aroundJellies[iMost].GetComponent<CS_Jelly>().Colour;
                }
                else
                {
                    col = this.Colour;
                }

                Matrix9x9.JELLYTYPE c = a_jelly.GetComponent<CS_Jelly>().Colour;
                if (c == col || c == Matrix9x9.JELLYTYPE.MULTI)
                {
                    iNumOfSame++;
                    iValidJellies[iNum] = true;
                }
            }
            else
            {
                iNumOfNone++;
            }
            iNum++;
        }
        if (iNumOfSame >= 2)
        {
            Matrix9x9.JELLYTYPE ColCompare;
            if (this.Colour == Matrix9x9.JELLYTYPE.MULTI)
            {
                ColCompare = col;
            }
            else
            {
                ColCompare = this.Colour;
            }
            foreach (GameObject a_jelly in aroundJellies)
            {
                if (a_jelly)
                {
                    if (a_jelly.GetComponent<CS_Jelly>().Colour == ColCompare || a_jelly.GetComponent<CS_Jelly>().Colour == Matrix9x9.JELLYTYPE.MULTI)
                    {
                        a_jelly.GetComponent<CS_Jelly>().bMatched = true;
                        CS_GameManager.instance.iScoreCount += iWorthOfJelly;
                    }
                }

            }

            if ((iValidJellies[0] || iValidJellies[1])
                && (iValidJellies[2] || iValidJellies[3]))
            {
                //if a corner match
                CS_GameManager.instance.bShowBrilliant = true;
            }
            CS_GameManager.instance.iScoreCount += iWorthOfJelly;
            bMatched = true;
        }
        else if (iNumOfNone == 4)
        {
            //If its the last one left it also dissappears
            CS_GameManager.instance.iScoreCount += iWorthOfJelly;
            bMatched = true;
        }
        yield return new WaitForSeconds(0.2f);
    }


    private RaycastHit2D CheckRaycast()
    {
        //Offset the starting position by the direction the raycast needs to check
        Vector3 v2StartingPosition = new Vector3(transform.position.x + (v2Dir.x * c_fOriginOffset), transform.position.y + (v2Dir.y * c_fOriginOffset), transform.position.z);
        //Debug the ray
         Debug.DrawRay(v2StartingPosition, v2Dir * 10, Color.red);
        //Return the raycast
        return Physics2D.Raycast(v2StartingPosition, v2Dir, Mathf.Infinity);
    }

    private RaycastHit2D BackRaycast()
    {
        //Offset the starting position by the direction the raycast needs to check
        Vector3 v2StartingPosition = new Vector3(transform.position.x - (v2Dir.x * c_fOriginOffset), transform.position.y - (v2Dir.y * c_fOriginOffset), transform.position.z);
        //Debug the ray

        Debug.DrawRay(v2StartingPosition, -v2Dir * 10, Color.red);
        //Return the raycast
        return Physics2D.Raycast(v2StartingPosition, -v2Dir, Mathf.Infinity);
    }
    //This function is a coroutine to decrease the amount of times it is called
    //Stops it from being called every frame
    private IEnumerator RaycastUpdateCanShoot()
    {
        hit = CheckRaycast();
        sHitName = hit.collider.gameObject.name;
        //If the raycast hit a collider on layer 8
        if (hit.collider && hit.collider.gameObject.layer == 8 && hit.collider.tag != "Black")
        {
            if (!bMoveJelly)
            {
                if (v2BoardPos != hit.collider.gameObject.GetComponent<CS_Jelly>().v2BoardPos)
                {
                        Vector2 v3 = hit.collider.gameObject.GetComponent<CS_Jelly>().v2BoardPos;
                        v2BoardPos = v3;
                }
                v2newDir = new Vector2(v2Dir.x * -1, v2Dir.y);
                Vector2 v2Test = v2BoardPos + v2newDir;
                //If jelly doesnt have particles and the board position from raycast isnt off the grid
                if (v2Test.x <= c_iXGridMax && v2Test.y <= c_iYGridMax && v2Test.x >= c_iGridMin && v2Test.y >= c_iGridMin)
                {
                    if (!bHasParticles && CS_GameManager.instance.bLevelEnded == false)
                    {
                        bHasParticles = true;
                        shine = Instantiate(Particles, gameObject.transform);
                        shine.transform.position = gameObject.transform.position;
                    }
                    else if(bHasParticles && CS_GameManager.instance.bLevelEnded)
                    {
                        bHasParticles = false;
                        Destroy(shine);
                    }
                    if (bCanBeNoSpace)
                    {
                        CS_GameManager.instance.bNoMoreSpace[iNoSpaceVal] = false;
                    }
                }
                else if (bHasParticles && CS_GameManager.instance.bLevelEnded == false)
                {
                    Debug.Log(gameObject.name + " No More Space");
                    if(bCanBeNoSpace)
                    {
                        CS_GameManager.instance.bNoMoreSpace[iNoSpaceVal] = true;
                    }
                    bHasParticles = false;
                    Destroy(shine);
                }

            }
        }
        else if (shine)
        {
            Destroy(shine);
            bHasParticles = false;
        }
        yield return new WaitForSeconds(0.1f);
    }

    private void OnMouseDown()
    {
        if (bHasParticles && bCanClick)
        {
            if(CS_GameManager.instance.Instructions.activeSelf)
            {
                CS_GameManager.instance.Instructions.SetActive(false);
            }
            StartCoroutine("RaycastUpdateCanShoot");
            newJelly = Instantiate(gameObject, InsideJellies.transform);
            newJelly.transform.position = gameObject.transform.position;
            newJelly.transform.localScale = new Vector3(1.557272f, 1.557272f, 1.557272f);
            v2newDir = new Vector2(v2Dir.x * -1, v2Dir.y);
            newJelly.GetComponent<CS_Jelly>().v2BoardPos = v2BoardPos + v2newDir;
            // Debug.Log(hit.collider.gameObject.name + " Found: " + v2BoardPos + "new: " + newJelly.GetComponent<CS_Jelly>().v2BoardPos);
            newJelly.GetComponent<CS_Jelly>().bMoveJelly = true;
            newJelly.name = gameObject.name + CS_GameManager.instance.iJellyNum;
            CS_GameManager.instance.iJellyNum++;
            Destroy(shine);
            bHasParticles = false;
            SetNewJelly();
            StartCoroutine("CanClick");
        }
    }

    private IEnumerator CanClick()
    {
        bCanClick = false;
        yield return new WaitForSeconds(0.2f);
        bCanClick = true;
    }

    private void SetNewJelly()
    {
        //RaycastHit2D hit = BackRaycast();
        //If the raycast hit a collider on layer 8
        //if (hit.collider && hit.collider.gameObject.layer == 9)
        //{
        //    gameObject.GetComponent<Image>().sprite = hit.collider.gameObject.GetComponent<Image>().sprite;
        //    CS_GameManager.instance.SetJelly(hit.collider.gameObject);
        //}
        CS_GameManager.instance.SetJelly(gameObject);

    }

    private void MoveJelly()
    {
        if (bMoveJelly && CS_GameManager.instance.GridSystem.bRotating == false)
        {
            // transform.position += (new Vector3(v2Dir.x, v2Dir.y, 0) * fSpeed);
            if (transform.childCount > 0)
            {
                Transform particles = transform.GetChild(0);
                Destroy(particles.gameObject);
            }
            if (gameObject.name == "Yellow (3)")
            {
                Debug.Log("D");
            }
                SetJellyIntoGrid();
            gameObject.layer = 8;
            gameObject.tag = "InJelly";
            transform.position = new Vector2(v2GridOrigin.x + (fJellyRadius * v2BoardPos.x), v2GridOrigin.y - (fJellyRadius * v2BoardPos.y));
        }
    }

    private void SetJellyIntoGrid()
    {
        switch (gameObject.GetComponent<Image>().sprite.name)
        {
            case "Yellow":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.YELLOW, gameObject);
                Colour = Matrix9x9.JELLYTYPE.YELLOW;
                break;
            case "Green":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.GREEN, gameObject);
                Colour = Matrix9x9.JELLYTYPE.GREEN;
                break;
            case "Red":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.RED, gameObject);
                Colour = Matrix9x9.JELLYTYPE.RED;
                break;
            case "Blue":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.BLUE, gameObject);
                Colour = Matrix9x9.JELLYTYPE.BLUE;
                break;
            case "Pink":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.PINK, gameObject);
                Colour = Matrix9x9.JELLYTYPE.PINK;
                break;
            case "Black":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.BLACK, gameObject);
                Colour = Matrix9x9.JELLYTYPE.BLACK;
                break;
            case "Multi":
                CS_GameManager.instance.GridSystem.SetValue(v2BoardPos, Matrix9x9.JELLYTYPE.MULTI, gameObject);
                Colour = Matrix9x9.JELLYTYPE.MULTI;
                break;
        }
    }
}

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if(collision.gameObject.tag == "InJelly")
//        {
//            bMoveJelly = false;
//            gameObject.layer = 8;
//            gameObject.tag = "InJelly";
//        }
//    }
//}
