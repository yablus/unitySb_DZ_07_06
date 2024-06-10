using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SoundPack soundPack;

    [SerializeField] private int wheatCount;
    [SerializeField] private int enemiesCount;
    [SerializeField] private int peasantsCount;
    [SerializeField] private int warriorsCount;

    [SerializeField] private int harvestMaxTime; // время сбора урожая
    [SerializeField] private int eatingMaxTime; // время обеда
    [SerializeField] private int raidMaxTime; // время между рейдами
    [SerializeField] private int peasantCreateTime; // время найма крестьянина
    [SerializeField] private int warriorCreateTime; // время найма воина

    [SerializeField] private int wheatPerPeasant; // количество пшеницы добываемой одним крестьянином
    [SerializeField] private int wheatToEatWarrior; // еоличество пшеницы съедаемой одним воином
    [SerializeField] private int raidEnemyIncremrnt; // количество на которое увеличивается число врагов
    [SerializeField] private int peasantCost; // стоимость найма крестьянина
    [SerializeField] private int warriorCost; // стоимость найма воина

    [SerializeField] private int wheatToWin;
    [SerializeField] private int peasantsToWin;

    [SerializeField] private ImageTimerChanger harvestTimerImg;
    [SerializeField] private ImageTimerChanger eatingTimerImg;
    [SerializeField] private ImageTimerChanger raidTimerImg;
    [SerializeField] private ImageTimerChanger peasantTimerImg;
    [SerializeField] private ImageTimerChanger warriorTimerImg;

    [SerializeField] private ButtonEvents peasantButton;
    [SerializeField] private ButtonEvents warriorButton;

    [SerializeField] private Text wheatCountText;
    [SerializeField] private Text enemiesCountText;
    [SerializeField] private Text peasantsCountText;
    [SerializeField] private Text warriorsCountText;
    [SerializeField] private Text wheatCountBar;
    [SerializeField] private Text peasantsCountBar;
    [SerializeField] private Text warriorsCountBar;

    [SerializeField] private ImageOnOff hall;
    [SerializeField] private ImageOnOff barracks;
    [SerializeField] private ImageChanger house2;
    [SerializeField] private ImageChanger house1ur;
    [SerializeField] private ImageChanger house1l;
    [SerializeField] private ImageChanger house1r;
    [SerializeField] private ImageChanger house3l;
    [SerializeField] private ImageChanger house3c;
    [SerializeField] private ImageChanger house3r;
    [SerializeField] private ImageChanger well;
    [SerializeField] private ImageChanger house1dr;
    [SerializeField] private ImageChanger watermill;
    [SerializeField] private ImageHider tree3;
    [SerializeField] private ImageHider tree2;
    [SerializeField] private ImageHider tree1;
    [SerializeField] private ImagePackChanger wheat;
    [SerializeField] private ImageCountChanger ambar;

    [SerializeField] private IndicatorTransform indWheatFromHarvest;
    [SerializeField] private IndicatorTransform indWheatFromEating;
    [SerializeField] private IndicatorTransform indWheatCostPeasant;
    [SerializeField] private IndicatorTransform indWheatCostWarrior;
    [SerializeField] private IndicatorTransform indPeasantCreated;
    [SerializeField] private IndicatorTransform indWarriorCreated;
    [SerializeField] private IndicatorTransform indWarriorsPerRaid;
    [SerializeField] private FightTransform animationFightCloud;

    private AudioSource warriorCreatedSound;
    private AudioSource peasantCreatedSound;
    private AudioSource wheatCreatedSound;
    private AudioSource timeToEatSound;
    private AudioSource timeToFightSound;
    private AudioSource gameWinSound;
    private AudioSource gameLostSound;

    private int peasantsCountDefault, warriorsCountDefault, wheatCountDefault, enemiesCountDefault;
    private float harvestTimer, eatingTimer, raidTimer, peasantTimer, warriorTimer;
    private bool warriorCanEatForCreate, peasantCanEatForCreate, oneSecBeforeRaid;

    private void Start()
    {
        SetDefaultCounts();
        SetUpSound();
        GameRestart();
    }

    private void Update()
    {
        if (enemiesCount <= 0 && raidTimerImg.ParentIsActive())
            raidTimerImg.ParentSetActive(false);
        else if (enemiesCount > 0 && !raidTimerImg.ParentIsActive())
            raidTimerImg.ParentSetActive(true);
        raidTimer = _SetTimer(raidTimer, raidMaxTime, 0, true);
        raidTimerImg.SetFillAmount(1 - (raidTimer / raidMaxTime));
        if (raidTimer <= 1 && !oneSecBeforeRaid)
        {
            oneSecBeforeRaid = true;
            if (enemiesCount > 0)
                animationFightCloud.Show();
        }
        else if (raidTimer <= 0)
        {
            if (enemiesCount > 0)
            {
                uiManager.OnEventPlaySound(timeToFightSound);
                warriorsCount -= enemiesCount;
                if (warriorsCount < 0)
                {
                    uiManager.ViewWinLoseWindow(false);
                    uiManager.OnEventPlayLost(gameLostSound);
                    warriorsCountText.color = Color.red;
                    warriorsCountBar.color = Color.red;
                }
            }
            enemiesCount += raidEnemyIncremrnt;
            UpdateWarriorsCountText();
            UpdateEnemiesCountText();
            indWarriorsPerRaid.Show((enemiesCount - raidEnemyIncremrnt) * -1);
            oneSecBeforeRaid = false;
        }

        harvestTimer = _SetTimer(harvestTimer, harvestMaxTime, 0, true);
        harvestTimerImg.SetFillAmount(1 - (harvestTimer / harvestMaxTime));
        wheat.SetFill(1 - (harvestTimer / harvestMaxTime));
        if (harvestTimer <= 0)
        {
            wheatCount += peasantsCount * wheatPerPeasant;
            UpdateSpritesFromWheat();
            UpdateWheatCountText();
            indWheatFromHarvest.Show(peasantsCount * wheatPerPeasant);
            uiManager.OnEventPlaySound(wheatCreatedSound);
            if (wheatCount >= wheatToWin)
            {
                uiManager.ViewWinLoseWindow(true);
                uiManager.OnEventPlayWin(gameWinSound);
                wheatCountText.color = new Color(0.35f, 0.48f, 0.06f, 1);
                wheatCountBar.color = new Color(0.35f, 0.48f, 0.06f, 1);
            }
        }

        if (warriorsCount <= 0 && eatingTimerImg.ParentIsActive())
            eatingTimerImg.ParentSetActive(false);
        else if (warriorsCount > 0 && !eatingTimerImg.ParentIsActive())
            eatingTimerImg.ParentSetActive(true);
        if (warriorsCount > 0)
            eatingTimer = _SetTimer(eatingTimer, eatingMaxTime, 0, true);
        else if (eatingTimer != eatingMaxTime)
            eatingTimer = eatingMaxTime;
        eatingTimerImg.SetFillAmount(1 - (eatingTimer / eatingMaxTime));
        if (eatingTimer <= 0)
        {
            wheatCount -= warriorsCount * wheatToEatWarrior;
            UpdateSpritesFromWheat();
            UpdateWheatCountText();
            indWheatFromEating.Show(warriorsCount * wheatToEatWarrior * -1);
            uiManager.OnEventPlaySound(timeToEatSound);
            if (wheatCount < 0)
            {
                uiManager.ViewWinLoseWindow(false);
                uiManager.OnEventPlayLost(gameLostSound);
                wheatCountText.color = Color.red;
                wheatCountBar.color = Color.red;
            }
        }

        if (!peasantCanEatForCreate && (wheatCount >= peasantCost))
            peasantCanEatForCreate = true;
        else if (peasantCanEatForCreate && wheatCount < peasantCost)
            peasantCanEatForCreate = false;
        peasantTimer = _SetTimer(peasantTimer, peasantCreateTime, 0, false);
        if (peasantTimer > 0)
            peasantTimerImg.SetFillAmount(1 - (peasantTimer / peasantCreateTime));
        else if (peasantTimer > -1)
        {
            peasantTimerImg.SetFillAmount(0);
            if (peasantCanEatForCreate)
            {
                peasantButton.SetInteractable(true);
                if (peasantButton.IsOnTarget())
                    peasantButton.HoveredTextActive();
            }
            peasantsCount++;
            UpdateSpritesFromPeasants();
            UpdatePeasantsCountText();
            hall.ToOld();
            indPeasantCreated.Show(1);
            uiManager.OnEventPlaySound(peasantCreatedSound);
            if (peasantsCount >= peasantsToWin)
            {
                uiManager.ViewWinLoseWindow(true);
                uiManager.OnEventPlayWin(gameWinSound);
                peasantsCountText.color = new Color(0.35f, 0.48f, 0.06f, 1);
                peasantsCountBar.color = new Color(0.35f, 0.48f, 0.06f, 1);
            }
        }
        else
        {
            peasantTimerImg.SetFillAmount(0);
            if (peasantCanEatForCreate && !peasantButton.IsInteractable())
            {
                peasantButton.SetInteractable(true);
                if (peasantButton.IsOnTarget())
                    peasantButton.HoveredTextActive();
            }
            else if (!peasantCanEatForCreate && peasantButton.IsInteractable())
                peasantButton.SetInteractable(false);
        }

        if (!warriorCanEatForCreate && (wheatCount >= warriorCost))
            warriorCanEatForCreate = true;
        else if (warriorCanEatForCreate && wheatCount < warriorCost)
            warriorCanEatForCreate = false;
        warriorTimer = _SetTimer(warriorTimer, warriorCreateTime, 0, false);
        if (warriorTimer > 0)
            warriorTimerImg.SetFillAmount(1 - (warriorTimer / warriorCreateTime));
        else if (warriorTimer > -1)
        {
            warriorTimerImg.SetFillAmount(0);
            if (warriorCanEatForCreate)
            {
                warriorButton.SetInteractable(true);
                if (warriorButton.IsOnTarget())
                    warriorButton.HoveredTextActive();
            }
            warriorsCount++;
            UpdateWarriorsCountText();
            barracks.ToOld();
            indWarriorCreated.Show(1);
            uiManager.OnEventPlaySound(warriorCreatedSound);
        }
        else
        {
            warriorTimerImg.SetFillAmount(0);
            if (warriorCanEatForCreate && !warriorButton.IsInteractable())
            {
                warriorButton.SetInteractable(true);
                if (warriorButton.IsOnTarget())
                    warriorButton.HoveredTextActive();
            }
            else if (!warriorCanEatForCreate && warriorButton.IsInteractable())
                warriorButton.SetInteractable(false);
        }
    }

    private float _SetTimer(float timer, float timeFrom, float timeTo, bool loop)
    {
        bool inc = false;
        if (timeFrom < timeTo)
            inc = true;
        if (loop)
        {
            if ((inc && timer > timeTo) || (!inc && timer < timeTo))
                timer = timeFrom;
        }
        else
        {
            if (inc)
            {
                if (timer > timeTo && timer < timeTo + 1)
                    timer = timeTo + 1;
            }
            else
            {
                if (timer < timeTo && timer > timeTo - 1)
                    timer = timeTo - 1;
            }
        }
        if (inc)
        {
            if (timer < timeTo)
                timer += Time.deltaTime;
        }
        else
        {
            if (timer > timeTo)
                timer -= Time.deltaTime;
        }
        return timer;
    }

    private void UpdateSpritesFromPeasants()
    {
        switch (peasantsCount)
        {
            case 2:  well.ChangeSprite(); break;
            case 3:  house1ur.ChangeSprite(); break;
            case 5:  house1dr.ChangeSprite(); break;
            case 7:  house3r.ChangeSprite(); break;
            case 10: house1l.ChangeSprite(); break;
            case 12: house3c.ChangeSprite(); break;
            case 15: house1r.ChangeSprite(); break;
            case 17: house2.ChangeSprite(); break;
            case 19: watermill.ChangeSprite(); break;
            case 22: house3l.ChangeSprite(); break;
        }
        if (peasantsCount == 4 || peasantsCount == 9 || peasantsCount == 16)
        {
            wheat.ToCurrent();
            if (peasantsCount == 4)
                tree3.ToHide();
            else if (peasantsCount == 9)
                tree2.ToHide();
            else if (peasantsCount == 16)
                tree1.ToHide();
        }
    }

    private void UpdateSpritesFromWheat()
    {
        ambar.ToCurrent((float)wheatCount / wheatToWin);
    }

    private void UpdatePeasantsCountText()
    {
        peasantsCountText.text = peasantsCount.ToString();
        peasantsCountBar.text = peasantsCount.ToString();
    }

    private void UpdateWarriorsCountText()
    {
        warriorsCountText.text = warriorsCount.ToString();
        warriorsCountBar.text = warriorsCount.ToString();
    }

    private void UpdateWheatCountText()
    {
        wheatCountText.text = wheatCount.ToString();
        wheatCountBar.text = wheatCount.ToString();
    }

    private void UpdateEnemiesCountText()
    {
        enemiesCountText.text = "x" + enemiesCount;
    }

    private void SetDefaultCounts()
    {
        peasantsCountDefault = peasantsCount;
        warriorsCountDefault = warriorsCount;
        wheatCountDefault = wheatCount;
        enemiesCountDefault = enemiesCount;
    }

    private void SetUpSound()
    {
        warriorCreatedSound = soundPack.GetAudio("warriorCreated");
        peasantCreatedSound = soundPack.GetAudio("peasantCreated");
        wheatCreatedSound = soundPack.GetAudio("wheatCreated");
        timeToEatSound = soundPack.GetAudio("timeToEat");
        timeToFightSound = soundPack.GetAudio("timeToFight");
        gameWinSound = soundPack.GetAudio("gameWin");
        gameLostSound = soundPack.GetAudio("gameLost");
    }

    private void ToDefaults()
    {
        peasantsCount = peasantsCountDefault;
        warriorsCount = warriorsCountDefault;
        wheatCount = wheatCountDefault;
        enemiesCount = enemiesCountDefault;

        harvestTimer = harvestMaxTime;
        eatingTimer = eatingMaxTime;
        raidTimer = raidMaxTime;
        peasantTimer = -1;
        warriorTimer = -1;

        warriorCanEatForCreate = false;
        peasantCanEatForCreate = false;
        oneSecBeforeRaid = false;

        peasantsCountText.color = new Color(0.25f, 0.25f, 0.25f, 1);
        warriorsCountText.color = new Color(0.25f, 0.25f, 0.25f, 1);
        wheatCountText.color = new Color(0.25f, 0.25f, 0.25f, 1);
        peasantsCountBar.color = new Color(0.25f, 0.25f, 0.25f, 1);
        warriorsCountBar.color = new Color(0.25f, 0.25f, 0.25f, 1);
        wheatCountBar.color = new Color(0.25f, 0.25f, 0.25f, 1);

        hall.ToOld();
        barracks.ToOld();
        house2.DefaultSprite();
        house1ur.DefaultSprite();
        house1l.DefaultSprite();
        house1r.DefaultSprite();
        house3l.DefaultSprite();
        house3c.DefaultSprite();
        house3r.DefaultSprite();
        well.DefaultSprite();
        house1dr.DefaultSprite();
        watermill.DefaultSprite();
        tree3.ToVisible();
        tree2.ToVisible();
        tree1.ToVisible();
        wheat.ToDefault();
        ambar.ToEmpty();

        indWheatFromHarvest.Hide();
        indWheatFromEating.Hide();
        indWheatCostPeasant.Hide();
        indWheatCostWarrior.Hide();
        indPeasantCreated.Hide();
        indWarriorCreated.Hide();
        indWarriorsPerRaid.Hide();
        animationFightCloud.Hide();
    }

    public void CreatePeasant()
    {
        wheatCount -= peasantCost;
        peasantTimer = peasantCreateTime;
        peasantButton.SetInteractable(false);
        UpdateSpritesFromWheat();
        UpdateWheatCountText();
        hall.ToNew();
        indWheatCostPeasant.Show(peasantCost * -1);
    }

    public void CreateWarrior()
    {
        wheatCount -= warriorCost;
        warriorTimer = warriorCreateTime;
        warriorButton.SetInteractable(false);
        UpdateSpritesFromWheat();
        UpdateWheatCountText();
        barracks.ToNew();
        indWheatCostWarrior.Show(warriorCost * -1);
    }

    public void GameRestart()
    {
        ToDefaults();

        UpdatePeasantsCountText();
        UpdateWarriorsCountText();
        UpdateWheatCountText();
        UpdateEnemiesCountText();
    }
}
