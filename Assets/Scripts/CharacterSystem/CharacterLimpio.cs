using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLimpio : MonoBehaviour {

    string playerName { get; set; }
    ushort hp { get; set; }
    ushort mana { get; set; }
    bool alive { get; set; }
    bool derecha { get; set;}
    public List<Spell> Spells
    {
        get
        {
            return spells;
        }

        set
        {
            spells = value;
        }
    }
    private List<Spell> spells;
    Rigidbody2D rgbd2D { get; set; }

    public float velocidad;

    bool suelo { get; set; }
    public Transform comprobarSuelo;
    float radioSuelo = 0.2f;
    public LayerMask esSuelo;

    const float salto = 200;
    const float dash = 10000;
    // Use this for initialization
    void Start () {
        alive = true;
        derecha = false;
        hp = 100;
        mana = 100;
        rgbd2D = GetComponent<Rigidbody2D>();
        comprobarSuelo = this.gameObject.transform.GetChild(0);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        suelo = Physics2D.OverlapCircle(comprobarSuelo.position, radioSuelo, esSuelo);
        rgbd2D.velocity = new Vector2(Input.GetAxis("Horizontal") * velocidad, rgbd2D.velocity.y);
        rotatePlayer();
	}

    private void Update()
    {
        if(suelo && Input.GetButtonDown("Jump"))
        {
            rgbd2D.AddForce(new Vector2(0, salto));
        }
        if (Input.GetButtonDown("RecuperarHP"))
        {
            hp = 100;
        }
        if (Input.GetButtonDown("RecuperarMP"))
        {
            mana = 100;
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (derecha)
            {
                rgbd2D.AddForce(new Vector2(dash, 0));
            }
            else
            {
                rgbd2D.AddForce(new Vector2(-dash, 0));
            }
        }
    }


    //Girar en funcion de la direccion en la que mira el personaje
    public void rotatePlayer()
    {
        if (Input.GetAxis("Horizontal") > 0 && !derecha)
        {
            derecha = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetAxis("Horizontal") < 0 && derecha)
        {
            derecha = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

}
