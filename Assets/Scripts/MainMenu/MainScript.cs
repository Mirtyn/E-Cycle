using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;

public class MainScript : MapSettings
{
    private const string SEED = "SEED: \nThe world seed generates always the same world if the seed and ALL the settings are the same.\nNOTE!: if set to 0 the world will be set to a random seed.\nDefault: 0";
    private const string MAP_Y_SIZE = "MAP Y SIZE: \nThe amount of tiles there will be in the up direction of the world.\nNOTE!: can't be lower than 1.\nDefault: 64";
    private const string MAP_X_SIZE = "MAP X SIZE: \nThe amount of tiles there will be in the right direction of the world.\nNOTE!: can't be lower than 1.\nDefault: 64";
    private const string PROCENT_CHANCE_OF_WATER_BECOME_SAND = "PROCENT CHANCE OF WATER BECOME SAND: \nThe chance that the sea ends on a tile and becomes land.\nNOTE!: can't be lower than 0 nor higher than 1.\nDefault: 0.50";
    private const string PROCENT_CHANCE_OF_SAND_BECOME_WATER = "PROCENT CHANCE OF SAND BECOME WATER: \nWhenever there is sand that isn't connected to a main land than that patch of sand will be removed.\nNOTE!: can't be lower than 0 nor higher than 1.\nDefault: 0.20";
    private const string PROCENT_CHANCE_OF_LOSE_SAND_IN_SEA_BECOME_SEAWATER = "PROCENT CHANCE OF LOSE SAND IN SEA BECOME SEAWATER: \nThis is used to remove any left over sand that's surrounded by water.\nNOTE!: can't be lower than 0 nor higher than 1.\nDefault: 0.98";
    private const string WATER_MAX_RANGE_LAND_TILES_DEVIDER = "WATER MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of water lakes a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 80";
    private const string WATER_MIN_RANGE_LAND_TILES_DEVIDER = "WATER MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of water lakes a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 55";
    private const string DIRT_MAX_RANGE_LAND_TILES_DEVIDER = "DIRT MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of dirt patches a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 80";
    private const string DIRT_MIN_RANGE_LAND_TILES_DEVIDER = "DIRT MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of dirt patches a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 55";
    private const string FOREST_MAX_RANGE_LAND_TILES_DEVIDER = "FOREST MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of forest patches a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 72";
    private const string FOREST_MIN_RANGE_LAND_TILES_DEVIDER = "FOREST MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of forest patches a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 45";
    private const string MOUNTAIN_MAX_RANGE_LAND_TILES_DEVIDER = "MOUNTAIN MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of mountain patches a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 95";
    private const string MOUNTAIN_MIN_RANGE_LAND_TILES_DEVIDER = "MOUNTAIN MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of mountain patches a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 70";
    private const string STONE_MAX_RANGE_LAND_TILES_DEVIDER = "STONE MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of stones a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 160";
    private const string STONE_MIN_RANGE_LAND_TILES_DEVIDER = "STONE MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of stones a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 145";
    private const string TREE_MAX_RANGE_LAND_TILES_DEVIDER = "TREE MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of trees a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 3";
    private const string TREE_MIN_RANGE_LAND_TILES_DEVIDER = "TREE MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of trees a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 1";
    private const string CHICKEN_MAX_RANGE_LAND_TILES_DEVIDER = "CHICKEN MAX RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of chickens a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 60";
    private const string CHICKEN_MIN_RANGE_LAND_TILES_DEVIDER = "CHICKEN MIN RANGE LAND TILES DEVIDER: \nThese parameters are used to determen the amount of attempted creation of chickens a tile. The amount of tiles will be devided by a random value in between the min and max value. Calculation: XTiles * YTiles / (random Number in between min, max value).\nNOTE!: can't be lower than 1.\nDefault: 55";
    private const string DIRT_MAX_GENERATIONS = "DIRT MAX GENERATIONS: \nThis parameter is used to set a maximum of times a dirt tile may spread once placed. The more a dirt tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 6";
    private const string DIRT_MAX_GENERATIONS_RESULT_MULTIPLIER = "DIRT MAX GENERATIONS RESULT MULTIPLIER: \nThis parameter is used to get a chance of a tile becoming dirt while also being lowered by the amount of generations. The more a dirt tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 12";
    private const string WATER_MAX_GENERATIONS = "WATER MAX GENERATIONS: \nThis parameter is used to set a maximum of times a water tile may spread once placed. The more a water tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 7";
    private const string WATER_MAX_GENERATIONS_RESULT_MULTIPLIER = "WATER MAX GENERATIONS RESULT MULTIPLIER: \nThis parameter is used to get a chance of a tile becoming water while also being lowered by the amount of generations. The more a water tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 10";
    private const string FOREST_MAX_GENERATIONS = "FOREST MAX GENERATIONS: \nThis parameter is used to set a maximum of times a forest tile may spread once placed. The more a forest tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 8";
    private const string FOREST_MAX_GENERATIONS_RESULT_MULTIPLIER = "FOREST MAX GENERATIONS RESULT MULTIPLIER: \nThis parameter is used to get a chance of a tile becoming forest while also being lowered by the amount of generations. The more a forest tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 11";
    private const string MOUNTAIN_MAX_GENERATIONS = "MOUNTAIN MAX GENERATIONS: \nThis parameter is used to set a maximum of times a mountain tile may spread once placed. The more a mountain tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 6";
    private const string MOUNTAIN_MAX_GENERATIONS_RESULT_MULTIPLIER = "MOUNTAIN MAX GENERATIONS RESULT MULTIPLIER: \nThis parameter is used to get a chance of a tile becoming mountain while also being lowered by the amount of generations. The more a mountain tile has generated tiles around it the less chance there is for one to be generated but it can never be more than the max amount. Calculation: ABSOLUTE(generation - max generations) * result modifier / 100.\nNOTE!: can't be lower than 1.\nDefault: 8";

    private enum _ActivePanel { None, Start, Options };
    private _ActivePanel activePanel = _ActivePanel.None;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private GameObject[] startMenuInputFields;

    [SerializeField] private TMP_Text infoMenu;

    [SerializeField] private RectTransform mapSettingsPanel;
    [SerializeField] private Scrollbar mapSettingsSlider;

    private void Start()
    {
        TMP_InputField i;

        //seed
        i = startMenuInputFields[0].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.seed.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(SEED); });
        i.onDeselect.AddListener(delegate { Debug.Log("seedg"); });

        //MapYSize
        i = startMenuInputFields[1].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.MapYSize.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(MAP_Y_SIZE); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[1].GetComponentInChildren<TMP_InputField>()); });
        i.onDeselect.AddListener(delegate { Debug.Log("mXg"); });

        //MapXSize
        i = startMenuInputFields[2].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.MapXSize.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(MAP_X_SIZE); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[2].GetComponentInChildren<TMP_InputField>()); });

        //procentChanceOfWaterBecomeSand
        i = startMenuInputFields[3].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.procentChanceOfWaterBecomeSand.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(PROCENT_CHANCE_OF_WATER_BECOME_SAND); });
        i.onValueChanged.AddListener(delegate { ValueInBetweenProcentValues(startMenuInputFields[3].GetComponentInChildren<TMP_InputField>()); });
        
        //procentChanceOfSandBecomeSeaWater
        i = startMenuInputFields[4].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.procentChanceOfSandBecomeSeaWater.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(PROCENT_CHANCE_OF_SAND_BECOME_WATER); });
        i.onValueChanged.AddListener(delegate { ValueInBetweenProcentValues(startMenuInputFields[4].GetComponentInChildren<TMP_InputField>()); });
        
        //procentChanceOfLoseSandInSeaBecomeSeaWater
        i = startMenuInputFields[5].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.procentChanceOfLoseSandInSeaBecomeSeaWater.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(PROCENT_CHANCE_OF_LOSE_SAND_IN_SEA_BECOME_SEAWATER); });
        i.onValueChanged.AddListener(delegate { ValueInBetweenProcentValues(startMenuInputFields[5].GetComponentInChildren<TMP_InputField>()); });
        
        //waterMinRangeLandTilesDevider
        i = startMenuInputFields[6].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.waterMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(WATER_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[6].GetComponentInChildren<TMP_InputField>()); });
        
        //waterMaxRangeLandTilesDevider
        i = startMenuInputFields[7].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.waterMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(WATER_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[7].GetComponentInChildren<TMP_InputField>()); });
        
        //dirtMinRangeLandTilesDevider
        i = startMenuInputFields[8].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.dirtMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(DIRT_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[8].GetComponentInChildren<TMP_InputField>()); });
        
        //dirtMaxRangeLandTilesDevider
        i = startMenuInputFields[9].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.dirtMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(DIRT_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[9].GetComponentInChildren<TMP_InputField>()); });
        
        //forestMinRangeLandTilesDevider
        i = startMenuInputFields[10].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.forestMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(FOREST_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[10].GetComponentInChildren<TMP_InputField>()); });
        
        //forestMaxRangeLandTilesDevider
        i = startMenuInputFields[11].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.forestMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(FOREST_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[11].GetComponentInChildren<TMP_InputField>()); });
        
        //mountainMinRangeLandTilesDevider
        i = startMenuInputFields[12].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.mountainMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(MOUNTAIN_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[12].GetComponentInChildren<TMP_InputField>()); });
        
        //mountainMaxRangeLandTilesDevider
        i = startMenuInputFields[13].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.mountainMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(MOUNTAIN_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[13].GetComponentInChildren<TMP_InputField>()); });
        
        //stoneMinRangeLandTilesDevider
        i = startMenuInputFields[14].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.stoneMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(STONE_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[14].GetComponentInChildren<TMP_InputField>()); });
        
        //stoneMaxRangeLandTilesDevider
        i = startMenuInputFields[15].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.stoneMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(STONE_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[15].GetComponentInChildren<TMP_InputField>()); });
        
        //treeMinRangeLandTilesDevider
        i = startMenuInputFields[16].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.treeMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(TREE_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[16].GetComponentInChildren<TMP_InputField>()); });
        
        //treeMaxRangeLandTilesDevider
        i = startMenuInputFields[17].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.treeMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(TREE_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[17].GetComponentInChildren<TMP_InputField>()); });
        
        //chickenMinRangeLandTilesDevider
        i = startMenuInputFields[18].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.chickenMinRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(CHICKEN_MAX_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[18].GetComponentInChildren<TMP_InputField>()); });
        
        //chickenMaxRangeLandTilesDevider
        i = startMenuInputFields[19].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.chickenMaxRangeLandTilesDevider.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(CHICKEN_MIN_RANGE_LAND_TILES_DEVIDER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[19].GetComponentInChildren<TMP_InputField>()); });
        
        //dirtMaxGenerations
        i = startMenuInputFields[20].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.dirtMaxGenerations.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(DIRT_MAX_GENERATIONS); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[20].GetComponentInChildren<TMP_InputField>()); });
        
        //dirtMaxGenerationsResultMultiplier
        i = startMenuInputFields[21].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.dirtMaxGenerationsResultMultiplier.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(DIRT_MAX_GENERATIONS_RESULT_MULTIPLIER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[21].GetComponentInChildren<TMP_InputField>()); });
        
        //waterMaxGenerations
        i = startMenuInputFields[22].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.waterMaxGenerations.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(WATER_MAX_GENERATIONS); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[22].GetComponentInChildren<TMP_InputField>()); });
        
        //waterMaxGenerationsResultMultiplier
        i = startMenuInputFields[23].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.waterMaxGenerationsResultMultiplier.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(WATER_MAX_GENERATIONS_RESULT_MULTIPLIER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[23].GetComponentInChildren<TMP_InputField>()); });
        
        //forestMaxGenerations
        i = startMenuInputFields[24].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.forestMaxGenerations.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(FOREST_MAX_GENERATIONS); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[24].GetComponentInChildren<TMP_InputField>()); });
        
        //forestMaxGenerationsResultMultiplier
        i = startMenuInputFields[25].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.forestMaxGenerationsResultMultiplier.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(FOREST_MAX_GENERATIONS_RESULT_MULTIPLIER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[25].GetComponentInChildren<TMP_InputField>()); });
        
        //mountainMaxGenerations
        i = startMenuInputFields[26].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.mountainMaxGenerations.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(MOUNTAIN_MAX_GENERATIONS); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[26].GetComponentInChildren<TMP_InputField>()); });
        
        //mountainMaxGenerationsResultMultiplier
        i = startMenuInputFields[27].GetComponentInChildren<TMP_InputField>();
        i.text = MapSettings.mountainMaxGenerationsResultMultiplier.ToString();
        i.onSelect.AddListener(delegate { SetInfoPanel(MOUNTAIN_MAX_GENERATIONS_RESULT_MULTIPLIER); });
        i.onValueChanged.AddListener(delegate { ValueNotLowerThen1(startMenuInputFields[27].GetComponentInChildren<TMP_InputField>()); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activePanel == _ActivePanel.None)
            {
                QuitButtonPressed();
            }
            else if (activePanel == _ActivePanel.Start)
            {
                BackStartButtonPressed();
            }
            else if (activePanel == _ActivePanel.Options)
            {
                BackOptionsButtonPressed();
            }
        }
    }



    public void StartButtonPressed()
    {
        activePanel = _ActivePanel.Start;
        startPanel.SetActive(true);
    }

    public void BackStartButtonPressed()
    {
        activePanel = _ActivePanel.None;
        startPanel.SetActive(false);
    }

    public void SetInfoPanel(string text)
    {
        infoMenu.text = text;
    }

    public void SetMapSettingsPanelHeight()
    {
        var min = -1080;
        var max = 2160;

        var y = (max * mapSettingsSlider.value) + min;

        mapSettingsPanel.localPosition = new Vector3(0, y, 0);
    }

    public void ValueNotLowerThen1(TMP_InputField inputField)
    {
        int.TryParse(inputField.text, out int result);

        if (result < 1)
        {
            inputField.text = "";
        }
    }

    public void ValueInBetweenProcentValues(TMP_InputField inputField)
    {
        float.TryParse(inputField.text, out float result);

        if (result < 0f || result > 1f)
        {
            inputField.text = "";
        }
    }

    //public void EndOfEditingInputField(TMP_InputField inputField)
    //{
    //    if (inputField.text == "")
    //    {
    //        inputField.text = MapSettings.
    //    }
    //}



    public void OptionsButtonPressed()
    {
        activePanel = _ActivePanel.Options;
        optionsPanel.SetActive(true);
    }

    public void BackOptionsButtonPressed()
    {
        activePanel = _ActivePanel.None;
        optionsPanel.SetActive(false);
    }



    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}
