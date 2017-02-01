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
        last = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter.transform.position.y;
    }
    
    public Vector2 Max = new Vector2(150,150);
    private Vector2 OriginalPosition;
    float currentY = 0;
    float DestinoY = 0,currSpeed = 0;
    float last;
    int count = 0;
    // Update is called once per frame
    void Update () {
        Player = GameObject.Find("CharacterSystem").GetComponent<CharacterSystem>().mainCharacter;
        DestinoY = Player.transform.position.y;
        //DestinoY = Mathf.Clamp01(DestinoY);
        switch (Layer)
        {
            case ParallaxLayer.Background:
                Vector3 tempPos = Player.GetComponent<Character>().myCamera.transform.position;
                this.transform.position = new Vector3(tempPos.x, tempPos.y, 0.5f);
                break;
            case ParallaxLayer.Middle:

                
                
                Vector3 tempPos2 = Player.GetComponent<Character>().myCamera.transform.position;
                Vector2 currentPos = new Vector2(this.transform.position.x, DestinoY);
                Vector2 maxPos = OriginalPosition + Max;
                Vector2 minPos = OriginalPosition - Max;
                if(count == 0)
                {
                    last = this.transform.position.y;
                    count = 1;
                }
                //this.transform.Translate(new Vector2(0, this.transform.position.y - (Player.transform.position.y - last)) );//* 400 / 500)
                this.transform.Translate(new Vector2(0,  (Player.transform.position.y - last) * 400 / 500));//)
                print(Player.transform.position.y -last);
                last = Player.transform.position.y;
                print(last);
                

                break;
            case ParallaxLayer.Close:
                break;
            default:
                break;
        }
    }
}
