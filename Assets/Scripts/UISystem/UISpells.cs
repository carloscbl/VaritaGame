using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpells : MonoBehaviour
{
    public GameObject spellGameObject;//prefab spell
    public List<Toggle> spells = new List<Toggle>();
    private Toggle oActivedToggle;
    private int iActivedTogglePosition = -1;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ChangeSpell(spells[0]);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ChangeSpell(spells[1]);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            ChangeSpell(spells[2]);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            ChangeSpell(spells[3]);
        }
    }

    public void ChangeSpell(Toggle newToggle)
    {
        if (oActivedToggle != newToggle)
        {
            if (oActivedToggle != null)
            {
                oActivedToggle.GetComponent<Image>().color = Color.white;
            }
            oActivedToggle = newToggle;
            oActivedToggle.GetComponent<Image>().color = Color.cyan;
            iActivedTogglePosition = spells.IndexOf(newToggle);
        }
    }

    public int GetSpellPosition()
    {
        return iActivedTogglePosition;
    }

    public void SetSpells(List<Spell> newSpells)
    {
        for (int i = 0; i < newSpells.Count; i++)
        {
            GameObject newSpell = Instantiate(spellGameObject);
            spells.Add(newSpell.GetComponent<Toggle>());
            newSpell.transform.SetParent(transform.GetChild(0));
            newSpell.transform.Find("Name").GetComponent<Text>().text = newSpells[i].GetName();
            newSpell.GetComponent<Toggle>().onValueChanged.AddListener(delegate { ChangeSpell(newSpell.GetComponent<Toggle>()); });
            newSpell.GetComponent<Image>().color = Color.white;
        }
    }
}
