using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public enum ParallaxLayer
    {
        Background,Middle,Close
    }
    public ParallaxLayer Layer;
    public enum DeepStage
    {
        Ground,Sky,Space,UnderGround,MotherRock,Core
    }
    GameObject Player;
    public DeepStage currentDeepStage;
	// Use this for initialization
	void Start () {
        OriginalPosition = this.transform.position;
        lastY = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position.y;
        lastX = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position.x;
        checkStage();
    }
    
    public Vector2 Max = new Vector2(150,150);
    private Vector2 OriginalPosition;
    float lastY;
    float lastX;
    int count = 0;
    // Update is called once per frame
    void Update () {
        Player = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter;
        //DestinoY = Mathf.Clamp01(DestinoY);
        switch (Layer)
        {
            case ParallaxLayer.Background:
                BackGround();
                break;
            case ParallaxLayer.Middle:
                Middle();
                break;
            case ParallaxLayer.Close:
                Close();
                break;
        }
        checkStage();
    }
    void checkStage()
    {
        DeepStage tempStage;
        if (Player.transform.position.y <           30)
        {
            tempStage = DeepStage.Core;
        }else if(Player.transform.position.y <      150)
        {
            tempStage = DeepStage.MotherRock;
        }else if (Player.transform.position.y <     290)
        {
            tempStage = DeepStage.UnderGround;
        }
        else if (Player.transform.position.y <      330)
        {
            tempStage = DeepStage.Ground;
        }
        else if (Player.transform.position.y <      400)
        {
            tempStage = DeepStage.Sky;
        }
        else if (Player.transform.position.y <      500)
        {
            tempStage = DeepStage.Space;
        }else
        {
            tempStage = DeepStage.Space;
        }

        if(tempStage != currentDeepStage)
        {
            changedStage = true;
            currentDeepStage = tempStage;
        }else
        {
            changedStage = false;
        }

    }
    void BackGround()
    {
        Vector3 PlayerPos = Player.GetComponent<Character>().myCamera.transform.position;
        this.transform.position = new Vector3(PlayerPos.x, PlayerPos.y, this.transform.position.z);

        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, Mathf.Clamp(Player.transform.position.y, 0, 450)/450));
    }
    public float BackGroundY = 100;
    public float closeY = 100;
    public float closeX = 100;
    public float middleY = 250;
    public float middleX = 250;
    bool changedStage = true;
    void Close()
    {
        Translation(closeY, closeX,true);
    }
    Material matGen;
    void Translation(float maxY, float maxX ,bool Close)
    {
        if (count == 0)
        {
            lastY = this.transform.position.y;
            lastX = this.transform.position.x;
            count = 1;
            
        }
        if (Close)
        {
            this.transform.Translate(new Vector2(Player.transform.position.x - lastX, Player.transform.position.y - lastY));
            lastY = Player.transform.position.y;
            lastX = Player.transform.position.x;
            if (changedStage)
            {
                switch (currentDeepStage)
                {
                    case DeepStage.Ground:
                        matGen = Resources.Load<Material>("Art/Textures/BackGrounds/Materials/TrigoGround");
                        break;
                    case DeepStage.Sky:
                        break;
                    case DeepStage.Space:
                        break;
                    case DeepStage.UnderGround:
                        matGen = Resources.Load<Material>("Art/Textures/BackGrounds/Materials/UnderGround 1");
                        break;
                    case DeepStage.MotherRock:
                        break;
                    case DeepStage.Core:
                        matGen = Resources.Load<Material>("Art/Textures/BackGrounds/Materials/pillar");
                        break;
                }
                this.GetComponent<MeshRenderer>().material = matGen;
            }
            //this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Mathf.Clamp(Player.transform.position.x, -250, 12000) / closeX, GetComponent<Renderer>().material.GetTextureOffset("_MainTex").y));
        }
        else//Middle
        {
            this.transform.Translate(new Vector2((Player.transform.position.x - lastX), Player.transform.position.y - lastY));
            lastY = Player.transform.position.y;
            lastX = Player.transform.position.x;
            if (changedStage)
            {
                switch (currentDeepStage)
                {
                    case DeepStage.Ground:
                        matGen = Resources.Load<Material>("Art/Textures/BackGrounds/Materials/TrigoGround");
                        break;
                    case DeepStage.Sky:
                        break;
                    case DeepStage.Space:
                        break;
                    case DeepStage.UnderGround:
                        matGen = Resources.Load<Material>("Art/Textures/BackGrounds/Materials/UnderGround 1");
                        break;
                    case DeepStage.MotherRock:
                        break;
                    case DeepStage.Core:
                        matGen = Resources.Load<Material>("Art/Textures/BackGrounds/Materials/pillar");
                        break;
                }
                this.GetComponent<MeshRenderer>().material = matGen;
            }
                Material mat = matGen;
            //this.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(mat.GetTextureScale("_MainTex").x*1.5f, mat.GetTextureScale("_MainTex").y*1.5f));
           // this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Mathf.Clamp(Player.transform.position.x, -250, 12000) / closeX, GetComponent<Renderer>().material.GetTextureOffset("_MainTex").y));
        }
        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Mathf.Clamp(Player.transform.position.x, -250, 12000) / closeX, GetComponent<Renderer>().material.GetTextureOffset("_MainTex").y));
    }
    void Middle()
    {
        Translation(middleY, middleX,false);
    }
}
