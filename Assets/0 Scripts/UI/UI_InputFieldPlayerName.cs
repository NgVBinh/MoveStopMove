using TMPro;
using UnityEngine;

public class UI_InputFieldPlayerName : MonoBehaviour,ISaveManager
{
    [Header("Input field")]
    [SerializeField] private TMP_InputField inputField;

    public string playerName;

    private void Start()
    {
        inputField.onEndEdit.AddListener(GrapFromInputField);
    }

    public void GrapFromInputField(string inputTxt) {
        if (string.IsNullOrEmpty(inputTxt))
        {
            Debug.LogWarning("Name empty");
            inputField.text = playerName;
            return;
        }
        playerName = inputField.text;

        SaveManager.instance.SaveComponent(this);
        UIManager.instance.ChangePlayerName(playerName);
    }


    public void LoadData(GameData gameData)
    {
        playerName = gameData.playerName;
        inputField.text = playerName;
        UIManager.instance.ChangePlayerName(playerName);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerName = playerName;
    }
}
