using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    public enum ParallaxLayer
    {
        Background,Middle,Close
    }
    public ParallaxLayer Layer;
    GameObject Player;
	// Use this for initialization
	void Start () {
        OriginalPosition = this.transform.position;
        lastY = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position.y;
        lastX = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position.x;
    }
    
    public Vector2 Max = new Vector2(150,150);
    private Vector2 OriginalPosition;
    float currentY = 0;
    float DestinoY = 0,currSpeed = 0;
    float lastY;
    float lastX;
    int count = 0;
    // Update is called once per frame
    void Update () {
        Player = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter;
        DestinoY = Player.transform.position.y;
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
            default:
                break;
        }
    }
    void BackGround()
    {
        Vector3 tempPos = Player.GetComponent<Character>().myCamera.transform.position;
        this.transform.position = new Vector3(tempPos.x, tempPos.y, 0.5f);
        
    }
    public float closeY = 100;
    public float closeX = 100;
    public float middleY = 250;
    public float middleX = 250;
    void Close()
    {
        Translation(closeY, closeX,true);
    }
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
            this.transform.Translate(new Vector2(Player.transform.position.x - lastX, (Player.transform.position.y - lastY) * maxY / 500));
            lastY = Player.transform.position.y;
            lastX = Player.transform.position.x;
            //float interp = Mathf.Lerp(0, 50, );
            this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Mathf.Clamp(Player.transform.position.x, -250, 12000) / closeX, 0 ));
        }else
        {
            this.transform.Translate(new Vector2((Player.transform.position.x - lastX), (Player.transform.position.y - lastY) * maxY / 500));
            lastY = Player.transform.position.y;
            lastX = Player.transform.position.x;
        }
        
    }
    void Middle()
    {
        Translation(middleY, middleX,false);
    }
}
