// script for player functionality
// attach to blob object
// set active to true if this is starting line and false if not
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    // default character animator
    public Animator animator;

    // fire character animator
    public Animator fireAnim;

    // ice character animator
    public Animator iceAnim;

    // electric character animator
    public Animator electricAnim;


    // which way player is facing
    // player is assumed to be facing right at start
    public bool FaceRight = true;


    // whether a player is on a certain element
    public bool onFire = false;

    public bool onIce = false;
    public bool onElectric = false;

    // whether player contains element properties

    public bool fire = false;
    
    public bool ice = false;

    public bool electric = false;

    // explosion aspects
    public bool runExplode = false;
    public GameObject explosion;
    

    // for wall collision
    Box wall;


    // which path player is on and script
    public GameObject path;
    private Path pathFollower;

    // jumping child
    GameObject jumper;

    // seperate art
    GameObject PlayerArt_Default;
    GameObject PlayerArt_Fire;
    GameObject PlayerArt_Ice;
    GameObject PlayerArt_Electric;


    // for explosion
    ParticleSystem fireParticles;


    // timers for elements (how long they last)
    public float fireTimer;

    public float iceTimer;
    public float electricTimer;


    // renderers for hiding object
    public Renderer defaultRend;

    public Renderer fireRend;

    public Renderer spriteMaskRend;

    public Renderer iceRend;

    public Renderer electricRend;

    // whether this player / line is the one the user is moving
    public bool active;

    // started after last level
    bool victory = false;

    // timer for victory animation
    float VictoryTimer;

    // jumping script
    PlayerJump jump;

    // moving sound
    //public AudioSource movingSFX;

    // camera script
    CameraFollow2DLERP mainCamera;


    // Start is called before the first frame update
    void Start()
    {

        // filling in variables
        pathFollower = path.GetComponent<Path>();

        jumper = this.gameObject.transform.GetChild(0).gameObject;
        PlayerArt_Default = jumper.transform.GetChild(1).gameObject;
        PlayerArt_Fire = jumper.transform.GetChild(2).gameObject;
        PlayerArt_Ice = jumper.transform.GetChild(3).gameObject;
        PlayerArt_Electric = jumper.transform.GetChild(4).gameObject;

        jump = jumper.GetComponent<PlayerJump>();

        animator = PlayerArt_Default.GetComponent<Animator>();
        fireAnim = PlayerArt_Fire.GetComponent<Animator>();
        iceAnim = PlayerArt_Ice.GetComponent<Animator>();
        electricAnim = PlayerArt_Electric.GetComponent<Animator>();

        defaultRend = PlayerArt_Default.GetComponent<Renderer>();
        fireRend = PlayerArt_Fire.GetComponent<Renderer>();
        spriteMaskRend = jumper.GetComponentInChildren<Renderer>();
        iceRend = PlayerArt_Ice.GetComponent<Renderer>();
        electricRend = PlayerArt_Electric.GetComponent<Renderer>();
        
        fireParticles = GetComponent<ParticleSystem>();

        mainCamera = Camera.main.GetComponent<CameraFollow2DLERP>();

        // if not active, hide object
        if (!active) {
            disappear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if player is on fire, show fire art
        if (onFire || fire) {
            PlayerArt_Default.SetActive(false);
            PlayerArt_Ice.SetActive(false);
            PlayerArt_Electric.SetActive(false);
            PlayerArt_Fire.SetActive(true);
        }
        // if player is on ice, show ice art
        else if (onIce || ice) {
            PlayerArt_Default.SetActive(false);
            PlayerArt_Fire.SetActive(false);
            PlayerArt_Electric.SetActive(false);
            PlayerArt_Ice.SetActive(true);
        }
        // if player is on electric, show electric art
        else if (onElectric || electric) {
            PlayerArt_Default.SetActive(false);
            PlayerArt_Fire.SetActive(false);
            PlayerArt_Ice.SetActive(false);
            PlayerArt_Electric.SetActive(true);
        }
        // default art
        else {
            PlayerArt_Default.SetActive(true);
            PlayerArt_Fire.SetActive(false);
            PlayerArt_Ice.SetActive(false);
            PlayerArt_Electric.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        // explosion
        if (runExplode) {
            runExplode = false;
            onElectric = false;
            electric = false;
            explosion.SetActive(true);
            StartCoroutine(Explode(1f));
        }
        // if the player is on fire, but not on part of the fire line
        // decriment timer
        if (fire) {
            fireTimer -= Time.deltaTime;
            // fire out
            if (fireTimer <= 0) {
                onFire = false;
                fire = false;
            }
        }
        // if the player is on ice, but not on part of the ice line
        // decriment timer
        if (ice) {
            iceTimer -= Time.deltaTime;
            Debug.Log("Ice Timer: " + iceTimer);
            // ice out
            if (iceTimer <= 0) {
                Debug.Log("No longer ice");
                onIce = false;
                ice = false;
            }
            
        }
        // if the player is electrified, but not on node
        // decriment timer
        else if (electric) {
            electricTimer -= Time.deltaTime;
            if (electricTimer <= 0) {
                Debug.Log("No longer electric");
                electric = false;
            }
        }
        // if in victory mode
        else if (victory) {
            VictoryTimer -= Time.deltaTime;
            // move to the right
            Vector3 hello = new Vector3(transform.position.x + 0.05f, transform.position.y, transform.position.z);
            Move(hello);
            if (VictoryTimer <= 0) {
                victory = false;
                SceneManager.LoadScene("END");
                Time.timeScale = 0f;
                fireParticles.Stop();
            }
            //Victory();
        }
    }

    IEnumerator Explode(float delayTime)
    {
        Debug.Log("BOOM!!!");
        yield return new WaitForSeconds(delayTime);
        explosion.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // checks collision with switch object
        if (other.gameObject.tag == "Switch" && electric) {
            Debug.Log("hit switch");
            Switch switchScript = other.gameObject.GetComponent<Switch>();
            // see switch script
            switchScript.Hit();
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        // setting collide animation
        if (other.gameObject.tag == "Wall") {
            // checks for which element player is on
            if (fire || onFire) {
                fireAnim.SetTrigger("Collide");
            }
            else if (ice || onIce) {
                iceAnim.SetTrigger("Collide");
            }
            else if (electric || onElectric) {
                electricAnim.SetTrigger("Collide");
            }
            else {
                animator.SetTrigger("Collide");
            }
            
            Debug.Log("hit wall");
            // manually locks movement in direction player is going
            // if hits wall (check path script for how it does this)
            if (FaceRight) {
                pathFollower.stopRight = true;
            } else {
                pathFollower.stopLeft = true;
            }

            wall = other.gameObject.GetComponent<Box>();
            // if the wall is an ice wall
            // the player is on a fire line or has fire properties
            // and the play is the one currently active
            if (wall.isIce && (onFire || fire) && active) {
                // melt the wall
                StartCoroutine(wall.melt());
                // unlock movement
                if (FaceRight) {
                    pathFollower.stopRight = false;
                } 
                else {
                    pathFollower.stopLeft = false;
                }
            }
            // if player is on ice and hits fire wall
            // destory wall, unlock movement
            if (wall.isFire && (onIce || ice) && active) {
                other.gameObject.SetActive(false);
                if (FaceRight) {
                    pathFollower.stopRight = false;
                } 
                else {
                    pathFollower.stopLeft = false;
                }
            }
        }
    }


    // starts victory sequence, called from path
    public void Victory() {
        StartCoroutine(wait());
        mainCamera.follow = false;
        victory = true;
        VictoryTimer = 5;
        fireParticles.Play();

    }

    // helper for victory
    IEnumerator wait() {
        yield return new WaitForSeconds(1);
    }

    // shows player
    public void appear() {
        defaultRend.enabled = true;
        fireRend.enabled = true;
        spriteMaskRend.enabled = true;
        iceRend.enabled = true;
        electricRend.enabled = true;
        
    }

    // hides player
    public void disappear() {
        defaultRend.enabled = false;
        fireRend.enabled = false;
        spriteMaskRend.enabled = false;
        iceRend.enabled = false;
        electricRend.enabled = false;
    }

    public void OnCollisionExit2D(Collision2D other) {
        // when move away from wall, unlocks movement
        if (other.gameObject.tag == "Wall") {
            moveFree();
        }
    }

    // unlocks movement
    public void moveFree() {
        pathFollower.stopRight = false;
        pathFollower.stopLeft = false;
        Debug.Log("Free");
    }

    // function to move player, given new move position
    // handles animation and sound
    public void Move(Vector3 newpos) {

        transform.position = newpos;

        // sets animation depending on state
        if (jump.isGrounded) {
            if (fire || onFire) {
                fireAnim.SetBool("Walk", true);
            }
            else if (ice || onIce) {
                iceAnim.SetBool("Walk", true);
            }
            else if (electric || onElectric) {
                electricAnim.SetBool("Walk", true);
            }
            else {
                animator.SetBool("Walk", true);
            }
        }    
    }

    // when player is no longer moving, go back to idle
    public void Stop() {
        if (fire || onFire) {
            fireAnim.SetBool("Walk", false);
        }
        else if (ice || onIce) {
            iceAnim.SetBool("Walk", false);
        }
        else if (electric || onElectric) {
            electricAnim.SetBool("Walk", false);
        }
        else {
            animator.SetBool("Walk", false);
        }
        //WalkSFX.Stop();
    }

    // when the player changes direction
    // called by path script
    public void turn() {
        FaceRight = !FaceRight;
        Debug.Log("Turn");
		// Multiply player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
        // checking in case turning sets player off path
        pathFollower.cap();
    }



}
