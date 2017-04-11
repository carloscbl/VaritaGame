using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffects : MonoBehaviour {

	// Use this for initialization
	void Start () {
        text =this.GetComponent<Text>();

    }
    Text text;
    float time = 0;
    int counter = 0;
    // Update is called once per frame
    void Update () {
		
        
	}
    private void OnGUI()
    {
        time += Time.deltaTime;
        print(time);

        if (time > 1)
        {
            //time = 0;
            text.text.Replace(".", "");
            print("dentro");

            string dots = "";
            for (int i = 0; i < counter; i++)
            {
                dots += ".";
            }
            text.text.Insert(text.text.Length, dots);
            counter++;
            if (counter >= 4)
            {
                counter = 0;
            }
            text.text = time.ToString();
        }
    }
}
