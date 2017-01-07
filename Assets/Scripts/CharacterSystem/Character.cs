using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Character : MonoBehaviour
{

    // Update is called once per frame

    string playerName;
    int hp;
    int mana;

    bool alive;
    bool spawned;
    bool visible;
    public bool mainPlayer;
    private List<Spell> spells;

    Skin skin;
    //Skeleton skeleton;

    Rigidbody myRigidbody;
    Collider myCollider;
    //Weapon weaponRight;
    //Weapon weaponLeft;
    public GameObject myGameObject;
    List<GameObject> hierarchy;

    public GameObject body_main;
    public GameObject TopSkin;
    public GameObject LegsSkin;
    public GameObject ArmRight;
    public GameObject WeaponRight;
    public GameObject ArmRightSkin;

    public GameObject HatSkin;
    public GameObject ArmLeft;
    public GameObject WeaponLeft;
    public GameObject ArmLeftSkin;

    public GameObject ShoesSkin;
    public GameObject myCamera;

    protected virtual void Update()
    {
        // test.rotatePlayer();
        if (Input.mousePosition.x < Screen.width / 2)
        {
            //Negative X
            body_main.transform.eulerAngles = new Vector3(0, 180, 0);
            WeaponRight.GetComponent<SpriteRenderer>().flipY = true;
            ArmRight.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            //Positive X
            body_main.transform.eulerAngles = new Vector3(0, 0, 0);
            WeaponRight.GetComponent<SpriteRenderer>().flipY = false;
            ArmRight.GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    protected virtual void Start()
    {
        spawned = false;
        visible = false;
        alive = false;

        spells = new List<Spell>();
        spells.Add(new Spell("Fire1", 100, 0));
        spells.Add(new Spell("Fire2", 100, 0));
        spells.Add(new Spell("Fire3", 100, 0));
        spells.Add(new Spell("Fire4", 100, 0));

        myGameObject = this.gameObject;//.transform.SetParent(this.gameObject.transform);+
        myGameObject.transform.SetParent(this.gameObject.transform);
        hierarchy = new List<GameObject>();
        myCamera = myGameObject.transform.Find("Main Camera").gameObject;
        //print( myCamera.name);
        //myGameObject.name = this.name+"Base";
        if (mainPlayer == true)
        {
            myCamera.gameObject.SetActive(true);
        }

        skin = new Skin();

        body_main = myGameObject.transform.Find("body_main").gameObject;
        TopSkin = body_main.transform.Find("TopSkin").gameObject;
        LegsSkin = body_main.transform.Find("LegsSkin").gameObject;
        ArmRight = body_main.transform.Find("ArmRight").gameObject;
        HatSkin = body_main.transform.Find("HatSkin").gameObject;
        ArmLeft = body_main.transform.Find("ArmLeft").gameObject;
        ShoesSkin = body_main.transform.Find("ShoesSkin").gameObject;
        //Arms
        WeaponRight = ArmRight.transform.Find("WeaponRight").gameObject;
        ArmRightSkin = ArmRight.transform.Find("ArmRightSkin").gameObject;
        WeaponLeft = ArmLeft.transform.Find("WeaponLeft").gameObject;
        ArmLeftSkin = ArmLeft.transform.Find("ArmLeftSkin").gameObject;

        hierarchy.Add(TopSkin);
        hierarchy.Add(LegsSkin);
        hierarchy.Add(ArmRight);
        hierarchy.Add(HatSkin);
        hierarchy.Add(ArmLeft);
        hierarchy.Add(ShoesSkin);
        hierarchy.Add(WeaponRight);
        hierarchy.Add(ArmRightSkin);
        hierarchy.Add(WeaponLeft);
        hierarchy.Add(ArmLeftSkin);

        HatSkin.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Textures/Player/Skin/Hat_Explorer");
    }
    private void generateSkin()
    {

    }
    public void rotatePlayer()
    {
        // body_main.transform.localRotation.eulerAngles.Set(0, 120, 0);
        body_main.transform.Rotate(0, 1, 0);
        Debug.Log("Donete");
    }

    public Spell GetSpell(int position)
    {
        if (position >= 0 && position < spells.Count)
        {
            return spells[position];
        }
        return null;
    }

    public string GetName()
    {
        return playerName;
    }
}
