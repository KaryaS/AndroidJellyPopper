using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_GameManager : MonoBehaviour {

    [SerializeField]
    private Sprite[] Jellies;
    [SerializeField]
    private Image[] OutsideJellies;
    [SerializeField]
    private Text StartOfLevelText;
    [SerializeField]
    private GameObject EndGame;
    [SerializeField]
    private AudioSource Pop;
    [SerializeField]
    private AudioSource AmazingSound;
    [SerializeField]
    private AudioSource BrilliantSound;
    [SerializeField]
    private AudioSource BopSound;
    [SerializeField]
    private AudioSource TadaSound;
    [SerializeField]
    private AudioSource BurstSound;
    [SerializeField]
    private AudioSource SadSound;
    [SerializeField]
    private Text Scoretext;
    [SerializeField]
    private Text Targettext;
    [SerializeField]
    private GameObject amazing;
    [SerializeField]
    private GameObject brilliant;
    [SerializeField]
    private Slider starSlider;
    [SerializeField]
    private GameObject endLevel;
    [SerializeField]
    private Image[] StarsAtEnd;
    [SerializeField]
    private Text endLevelText;
    [SerializeField]
    private Text endLevelTotalText;
    [SerializeField]
    private Text endLevelCompleteText;
    [SerializeField]
    private GameObject NextLevelButton;
    [SerializeField]
    private GameObject RestartButton;
    [SerializeField]
    private ParticleSystem[] Confetti;
    [SerializeField]
    private GameObject[] LevelsPrefabs;
    [SerializeField]
    private Image[] OutsideJelliesPerLevel;
    [SerializeField]
    private int[] iJellyNumInLevel;
    [SerializeField]
    private GameObject GamePlay;
    [SerializeField]
    private GameObject SettleButton;
    public GameObject Instructions;
    private GameObject[] Levels;
    public bool bShowBrilliant = false;
    public int iTotalScore= 0;
    public int iScoreCount = 0;
    public int iJellyNum = 0;
    public Matrix9x9 GridSystem;
    public int[] iLevelTarget;
    private int iNumOfLevels = 3;
    private int iCurrentLevel;
    private int iAddedBonus = 500;
    public bool bLevelEnded = false;
    public static CS_GameManager instance = null;
    public bool bPlaySound = false;
    private bool bPause = false;
    private bool bNextLevel = false;
    private int iCurValOfEndScore = 0;
    private bool bTitleStart = false;
    public bool[] bNoMoreSpace;
    private int iNumOfNoSpace = 0;
    private bool bPlayWows = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        GridSystem = new Matrix9x9();
    }

    // Use this for initialization
    void Start () {
        iCurrentLevel = 0;
        EndGame.SetActive(false);
        bNoMoreSpace = new bool[28];
        Levels = new GameObject[LevelsPrefabs.Length];
        Levels[iCurrentLevel] = Instantiate(LevelsPrefabs[iCurrentLevel], GamePlay.transform);
        Levels[iCurrentLevel].transform.SetAsFirstSibling();
        //Randomly assigns the outside jellies to a colour
        OutsideJellies = new Image[28];
        GameObject[] OutJel = GameObject.FindGameObjectsWithTag("OutJelly");
        for (int i = 0; i < 28; i++)
        {
            OutsideJellies[i] = OutJel[i].GetComponent<Image>();
        }
        foreach (Image a_image in OutsideJellies)
        {
            int iRand = Random.Range(0, 5);
            a_image.sprite = Jellies[iRand];
        }
        iJellyNum = 3;
        iTotalScore = 0;
        iScoreCount = 0;
        Scoretext.text = "0";
        Targettext.text = iLevelTarget[iCurrentLevel].ToString();
        amazing.SetActive(false);
        brilliant.SetActive(false);
        starSlider.value = 0;
        endLevel.SetActive(false);
        StartCoroutine("ShowStartOfLevelText");
        Instructions.SetActive(true);
	}

    private void Update()
    {
        if (bPlaySound && bPlayWows)
        {
            Pop.Play();
            if (iScoreCount >= 200)
            {
                //Give them more scores for completing more
                iScoreCount += 50;
                bShowBrilliant = false;
                StartCoroutine("ShowAmazing");
            }
            else if (bShowBrilliant)
            {
                StartCoroutine("ShowBrilliant");
            }
            iTotalScore += iScoreCount;
            float fPerc = iLevelTarget[iCurrentLevel] + iAddedBonus;
            float fSliderVal = iTotalScore / fPerc;
            starSlider.value = fSliderVal;
            iScoreCount = 0;
            Scoretext.text = iTotalScore.ToString();
            if (iTotalScore >= iLevelTarget[iCurrentLevel])
            {
                Scoretext.color = new Color(255, 0, 125);
            }
            bPlaySound = false;
        }
        iNumOfNoSpace = 0;
        foreach (bool b in bNoMoreSpace)
        {
            if(b == true)
            {
                iNumOfNoSpace++;
            }
        }
        if(iNumOfNoSpace == 24)
        {
            bLevelEnded = true;
            endLevel.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                StarsAtEnd[i].color = Color.black;
                endLevelCompleteText.text = "";
            }
            StartCoroutine("AddedEndTotal");
            bPause = true;
        }
        if(iCurrentLevel < iLevelTarget.Length)
        {
            if (iTotalScore >= iLevelTarget[iCurrentLevel])
            {
                SettleButton.SetActive(true);
            }
            else
            {
                SettleButton.SetActive(false);
            }
            if ((iJellyNum <= 0 || iTotalScore >= iLevelTarget[iCurrentLevel] + iAddedBonus) && bPause == false)
            {
                bPlaySound = false;
                bLevelEnded = true;
                endLevel.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    StarsAtEnd[i].color = Color.black;
                    endLevelCompleteText.text = "";
                }
                StartCoroutine("AddedEndTotal");
                bPause = true;
                //endLevelText.text = iTotalScore.ToString();

            }
            if (bNextLevel)
            {
                if (iCurrentLevel < LevelsPrefabs.Length)
                {
                    //Levels[iCurrentLevel].SetActive(true);
                    GameObject[] OutJel = GameObject.FindGameObjectsWithTag("OutJelly");
                    for (int i = 0; i < 28; i++)
                    {
                        OutsideJellies[i] = OutJel[i].GetComponent<Image>();
                    }
                    foreach (Image a_image in OutsideJellies)
                    {
                        int iRand = Random.Range(0, 5);
                        a_image.sprite = Jellies[iRand];
                    }
                    if (iCurrentLevel == 1)
                    {
                        StartOfLevelText.text = "LEVEL TWO";
                        StartCoroutine("ShowStartOfLevelText");
                    }
                    else if (iCurrentLevel == 2)
                    {
                        StartOfLevelText.text = "LEVEL THREE";
                        StartCoroutine("ShowStartOfLevelText");
                    }
                    else if (iCurrentLevel == 3)
                    {
                        StartOfLevelText.text = "LEVEL FOUR";
                        StartCoroutine("ShowStartOfLevelText");
                    }
                }
            }
            if (OutsideJellies[25] == null)
            {
                if(iCurrentLevel <= 3)
                {
                    GameObject[] OutJel = GameObject.FindGameObjectsWithTag("OutJelly");
                    for (int i = 0; i < 28; i++)
                    {
                        OutsideJellies[i] = OutJel[i].GetComponent<Image>();
                    }
                    foreach (Image a_image in OutsideJellies)
                    {
                        int iRand = Random.Range(0, 5);
                        a_image.sprite = Jellies[iRand];
                    }
                }

            }
        }

    }

    public void SettleClick()
    {
        bPlaySound = false;
        bLevelEnded = true;
        endLevel.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            StarsAtEnd[i].color = Color.black;
            endLevelCompleteText.text = "";
        }
        StartCoroutine("AddedEndTotal");
        bPause = true;
    }

    public void RestartClick()
    {

    }

    private IEnumerator AddedEndTotal()
    {
        float timer = 0;
        bPlayWows = false;
        AmazingSound.Stop();
        BrilliantSound.Stop();
        float fGrowFactor = 1.5f;
        endLevelCompleteText.gameObject.transform.localScale = Vector3.zero;
        endLevelCompleteText.gameObject.SetActive(true);
        endLevelTotalText.text = "/ " + iLevelTarget[iCurrentLevel].ToString();
        while (iCurValOfEndScore <= iTotalScore - 50)
        {
            iCurValOfEndScore += 50;
            endLevelText.text = iCurValOfEndScore.ToString();
            BopSound.Play();
            yield return new WaitForSeconds(0.05f);
        }
        if(iTotalScore >= iLevelTarget[iCurrentLevel])
        {
            if (iTotalScore >= iLevelTarget[iCurrentLevel] / 2)
            {
                StarsAtEnd[0].color = Color.white;
                endLevelCompleteText.text = "LEVEL COMPLETE";
                RestartButton.SetActive(false);
                NextLevelButton.SetActive(true);
            }
            if (iTotalScore >= iLevelTarget[iCurrentLevel])
            {
                StarsAtEnd[1].color = Color.white;
            }
            if (iTotalScore >= iLevelTarget[iCurrentLevel] + iAddedBonus)
            {
                StarsAtEnd[2].color = Color.white;
            }
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (1 > endLevelCompleteText.gameObject.transform.localScale.x)
            {
                timer += Time.deltaTime;
                endLevelCompleteText.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * fGrowFactor;
                yield return null;
            }
            // reset the timer
            bTitleStart = false;

            yield return new WaitForSeconds(0.1f);
            //Play particles
            TadaSound.Play();
            Confetti[0].gameObject.SetActive(true);
            BurstSound.Play();
            Confetti[0].Play();
            yield return new WaitForSeconds(0.3f);
            Confetti[1].gameObject.SetActive(true);
            BurstSound.Play();
            Confetti[1].Play();
            //play yay
            yield return new WaitForSeconds(0.8f);
            Confetti[0].gameObject.SetActive(false);
            Confetti[1].gameObject.SetActive(false);
            //Show Level Complete
        }
        else
        {
            endLevelCompleteText.text = "LEVEL FAILED";
            SadSound.Play();
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (1 > endLevelCompleteText.gameObject.transform.localScale.x)
            {
                timer += Time.deltaTime;
                endLevelCompleteText.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * fGrowFactor;
                yield return null;
            }
            // reset the timer
            bTitleStart = false;

            yield return new WaitForSeconds(0.5f);
            RestartButton.SetActive(true);
            NextLevelButton.SetActive(false);
        }
    }

    public void NextLevelClick()
    {
        Destroy(Levels[iCurrentLevel]);
        iCurValOfEndScore = 0;
        //Start next level
        iCurrentLevel++;
        if (iCurrentLevel < LevelsPrefabs.Length)
        {
            bNextLevel = true;
            iAddedBonus = iLevelTarget[iCurrentLevel] / 2;
            Levels[iCurrentLevel] = Instantiate(LevelsPrefabs[iCurrentLevel], GamePlay.transform);
            Levels[iCurrentLevel].SetActive(true);
            Levels[iCurrentLevel].transform.SetAsFirstSibling();
            endLevel.SetActive(false);
            Scoretext.color = Color.white;
            bLevelEnded = false;
            bPause = false;
            iJellyNum = iJellyNumInLevel[iCurrentLevel];
            iTotalScore = 0;
            iScoreCount = 0;
            Scoretext.text = "0";
            Targettext.text = iLevelTarget[iCurrentLevel].ToString();
            amazing.SetActive(false);
            brilliant.SetActive(false);
            starSlider.value = 0;
            if (iCurrentLevel == 1)
            {
                StartOfLevelText.text = "LEVEL TWO";
                StartCoroutine("ShowStartOfLevelText");
            }
            else if (iCurrentLevel == 2)
            {
                StartOfLevelText.text = "LEVEL THREE";
                StartCoroutine("ShowStartOfLevelText");
            }
            else if (iCurrentLevel == 3)
            {
                StartOfLevelText.text = "LEVEL FOUR";
                StartCoroutine("ShowStartOfLevelText");
            }
            RestartButton.SetActive(false);
            NextLevelButton.SetActive(false);
            bNextLevel = false;
        }
        else
        {
            EndGame.SetActive(true);

        }
    }

    public void ResetCick()
    {
        Destroy(Levels[iCurrentLevel]);
        bNextLevel = true;
        iCurValOfEndScore = 0;
        if (iCurrentLevel < LevelsPrefabs.Length)
        {
            if(iCurrentLevel == 0)
            {
                iJellyNumInLevel[iCurrentLevel] = 2;
            }
            Levels[iCurrentLevel] = Instantiate(LevelsPrefabs[iCurrentLevel], GamePlay.transform);
            Levels[iCurrentLevel].SetActive(true);
            Levels[iCurrentLevel].transform.SetAsFirstSibling();
            endLevel.SetActive(false);
            Scoretext.color = Color.white;
            bLevelEnded = false;
            bPause = false;
            iJellyNum = iJellyNumInLevel[iCurrentLevel];
            iTotalScore = 0;
            iScoreCount = 0;
            Scoretext.text = "0";
            Targettext.text = iLevelTarget[iCurrentLevel].ToString();
            amazing.SetActive(false);
            brilliant.SetActive(false);
            starSlider.value = 0;
            bNextLevel = false;
            RestartButton.SetActive(false);
            NextLevelButton.SetActive(false);
        }
        bPlayWows = true;

    }

    public void SetJelly(GameObject a_Jelly)
    {
        int iRand = Random.Range(0, 6);
        a_Jelly.GetComponent<Image>().sprite = Jellies[iRand];
    }

    public void OnRotClick()
    {
        //GridSystem.RotateGrid();
    }
	
    public string GetJellyName(int a_Val)
    {
        return Jellies[a_Val].name;
    }

    private IEnumerator ShowBrilliant()
    {
        if(bPlayWows)
        {
            float timer = 0;
            BrilliantSound.Play();
            Color resetcolour = Color.white;
            brilliant.GetComponent<Image>().color = resetcolour;
            float fGrowFactor = 1.5f;
            brilliant.transform.localScale = Vector3.zero;
            brilliant.SetActive(true);
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (1 > brilliant.transform.localScale.x)
            {
                timer += Time.deltaTime;
                brilliant.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * fGrowFactor;
                yield return null;
            }
            // reset the timer

            yield return new WaitForSeconds(0.5f);

            timer = 0;
            float fFadeFactor = 2;
            while (0 < brilliant.GetComponent<Image>().color.a)
            {
                timer += Time.deltaTime;
                Color c = brilliant.GetComponent<Image>().color;
                c.a -= 1 * Time.deltaTime * fFadeFactor;
                brilliant.GetComponent<Image>().color = c;
                yield return null;
            }

            timer = 0;
            bShowBrilliant = false;
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    private IEnumerator ShowAmazing()
    {
        if (bPlayWows)
        {
            float timer = 0;
            AmazingSound.Play();
            Color resetcolour = Color.white;
            amazing.GetComponent<Image>().color = resetcolour;
            float fGrowFactor = 1.5f;
            amazing.transform.localScale = Vector3.zero;
            amazing.SetActive(true);
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (1 > amazing.transform.localScale.x)
            {
                timer += Time.deltaTime;
                amazing.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * fGrowFactor;
                yield return null;
            }
            // reset the timer

            yield return new WaitForSeconds(0.5f);

            timer = 0;
            float fFadeFactor = 2;
            while (0 < amazing.GetComponent<Image>().color.a)
            {
                timer += Time.deltaTime;
                Color c = amazing.GetComponent<Image>().color;
                c.a -= 1 * Time.deltaTime * fFadeFactor;
                amazing.GetComponent<Image>().color = c;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    private IEnumerator ShowStartOfLevelText()
    {
        float timer = 0;
        bPlaySound = false;
        Color resetcolour = Color.white;
        StartOfLevelText.color = resetcolour;
        float fGrowFactor = 1.5f;
        StartOfLevelText.gameObject.transform.localScale = Vector3.zero;
        StartOfLevelText.gameObject.SetActive(true);
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while (1 > StartOfLevelText.gameObject.transform.localScale.x)
        {
            timer += Time.deltaTime;
            StartOfLevelText.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * fGrowFactor;
            yield return null;
        }
        // reset the timer

        yield return new WaitForSeconds(0.5f);

        timer = 0;
        float fFadeFactor = 2;
        while (0 < StartOfLevelText.color.a)
        {
            timer += Time.deltaTime;
            Color c = StartOfLevelText.color;
            c.a -= 1 * Time.deltaTime * fFadeFactor;
            StartOfLevelText.color = c;
            yield return null;
        }

        timer = 0;
        bPlayWows = true;
        yield return new WaitForSeconds(1);
    }

    private IEnumerator ShowTitle()
    {
        float timer = 0;
        float fGrowFactor = 1.5f;
        endLevelCompleteText.gameObject.transform.localScale = Vector3.zero;
        endLevelCompleteText.gameObject.SetActive(true);
        // we scale all axis, so they will have the same value, 
        // so we can work with a float instead of comparing vectors
        while (1 > amazing.transform.localScale.x)
        {
            timer += Time.deltaTime;
            endLevelCompleteText.gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * fGrowFactor;
            yield return null;
        }
        // reset the timer
        bTitleStart = false;

        yield return new WaitForSeconds(0.5f);
    }

}
