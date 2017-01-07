using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum NewGameState //controller for when we go back with back button
{
    PlayerSelection, SelectionNewPlayer
}

public class UICharacterSelection : MonoBehaviour
{
    public GameObject SelectPlayerButton;
    public GameObject SelectionPlayer;
    public GameObject NewPlayerPanel;
    public GameObject PlayerCharacter;
    public GameObject PlayerNotSelectedText;
    public GameObject BackButton;
    public GameObject CharacterPrefab;
    public GameObject UISystemController;

    private NewGameState currentNewGameState;
    private Toggle selectedCharacter;
    private GameObject finalCharacter;


    // Use this for initialization
    void Start()
    {
        //If there is not character selected, we disable to create the world and not show the character container
        PlayerCharacter.SetActive(PlayerCharacter.transform.childCount == 0 ? false : true);
        SelectPlayerButton.GetComponent<Button>().interactable = PlayerCharacter.transform.childCount == 0 ? false : true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateSelectionPlayer()
    {
        SelectionPlayer.SetActive(false);
        NewPlayerPanel.SetActive(true);
        currentNewGameState = NewGameState.SelectionNewPlayer;
    }

    private void OnEnable()
    {
        BackButton.SetActive(true);
        currentNewGameState = NewGameState.PlayerSelection;
    }

    public void GoBack()
    {
        if (currentNewGameState == NewGameState.PlayerSelection)
        {
            gameObject.SetActive(false);
            BackButton.SetActive(false);
            UISystemController.GetComponent<UISystemController>().ActivateMainMenu();
            return;
        }
        if (currentNewGameState == NewGameState.SelectionNewPlayer)
        {
            currentNewGameState = NewGameState.PlayerSelection;
            NewPlayerPanel.SetActive(false);
            SelectionPlayer.SetActive(true);
            return;
        }
    }

    public void InitCharacterList(List<string> list)
    {
        foreach (string name in list)
        {
            GameObject newCharacter = Instantiate(CharacterPrefab);
            newCharacter.transform.FindChild("CharacterName").GetComponent<Text>().text = name;
            newCharacter.transform.SetParent(NewPlayerPanel.transform.Find("Panel"));
            newCharacter.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SelectCharacter(newCharacter.GetComponent<Toggle>()); });
        }
    }

    public void SelectCharacter(Toggle toggle)
    {
        if (selectedCharacter != null)
        {
            selectedCharacter.GetComponent<Image>().color = Color.white;
        }
        selectedCharacter = toggle;
        selectedCharacter.GetComponent<Image>().color = Color.cyan;
    }

    public void ConfirmCharacter()
    {
        if (selectedCharacter != null)
        {
            finalCharacter = Instantiate(selectedCharacter.gameObject);
            Destroy(finalCharacter.GetComponent<Toggle>());
            //Destroy(finalCharacter.GetComponent<LayoutElement>());
            finalCharacter.GetComponent<Image>().color = Color.white;
            finalCharacter.transform.SetParent(PlayerCharacter.transform, false);
            finalCharacter.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            currentNewGameState = NewGameState.PlayerSelection;
            SelectPlayerButton.GetComponent<Button>().interactable = true;
            PlayerNotSelectedText.SetActive(false);
            NewPlayerPanel.SetActive(false);
            PlayerCharacter.SetActive(true);
            SelectionPlayer.SetActive(true);
        }
    }

    public void CreateWorld()
    {
        SelectionPlayer.SetActive(false);
        BackButton.SetActive(false);
        UISystemController.GetComponent<UISystemController>().CreateWorld(finalCharacter.transform.FindChild("CharacterName").GetComponent<Text>().text);
    }
}
