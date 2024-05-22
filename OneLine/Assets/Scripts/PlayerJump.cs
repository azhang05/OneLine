using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for player jumping
// attach to child of blob object that is the player
// should have the different models as children in order:
// 1: sprite mask
// 2: default art
// 3: fire art
// 4: ice art
// 5: electric art
// 6: explosion
public class PlayerJump : MonoBehaviour
{
    // whether player is in air or not
    public bool isGrounded = true;
    private Rigidbody2D myRb;


    Box wall;

    // which way the player is jumping
    public bool vertical;
    public bool left;

    public bool right;

    public bool down;

    // main player object
    public GameObject overlord;

    // path player is currently on
    GameObject path;
    Path pathscript;

    // path and player on the line player jumps to
    Path otherpath;

    Player otherplayer;
    
    public Player playerscript;

    private Animator animator;

    CameraFollow2DLERP mainCamera;

    // if the player was stopped before jumping
    bool prevright;

    bool prevleft;

    Node node;

    private GameObject spritemask;
    
    // Start is called before the first frame update
    void Start()
    {
        // populating variables

        myRb = GetComponent<Rigidbody2D>();
        myRb.isKinematic = true;
        animator = overlord.GetComponentInChildren<Animator>();
        spritemask = transform.GetChild(0).gameObject;
        playerscript = overlord.GetComponent<Player>();
        path = playerscript.path;
        pathscript = path.GetComponent<Path>();

        mainCamera = Camera.main.GetComponent<CameraFollow2DLERP>();
    }


    // Update is called once per frame
    void Update()
    {
        // vertical jump (player is between 45 and -45 degrees)
        if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            // setting bool value for jumping and direction
            isGrounded = false;
            vertical = true;

            // setting rigidbody to dynamic, allowing forces and gravity to affect it
            myRb.isKinematic = false;
            // jumping force
            myRb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Debug.Log("I'm jumping");
            // setting animation
            animatorif();
            animator.SetBool("Walk", false);

            // disable sprite mask
            spritemask.SetActive(false);

            // storing stop values
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
            // making player stop no no motion while jumping
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        // player is landing after vertical jump
        else if (transform.position.y - overlord.transform.position.y < 0f && !isGrounded && vertical) {
            // player is grounded
            isGrounded = true;
            vertical = false;
            // zero velocity and returning to kinematic
            myRb.velocity = Vector2.zero;
            myRb.isKinematic = true; 
            // going back to original position before jump   
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            // re-enabling sprite mask
            spritemask.SetActive(true);
            Debug.Log("I'm grounded");
            // re-enabling movement 
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
        // player is jumping left (player is between 45 and 135 degrees)
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > 0.382 && transform.rotation.z < 0.707) {
            // same as above
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
            Debug.Log("Jump left");
            // setting gravity to 0 to allow for horizontal movement
            myRb.gravityScale = 0;
            animatorif();
            animator.SetBool("Walk", false);
            left = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        // player is landing after jumping left
        else if (transform.position.x - overlord.transform.position.x > 0 && !isGrounded && left) {
            // same as above
            isGrounded = true;
            myRb.isKinematic = true;  
            myRb.velocity = Vector2.zero;
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            left = false;
            // re enable gravity for next jump
            myRb.gravityScale = 1;
            Debug.Log("I'm grounded");
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
        // player is jumping right (player is between -45 and -135 degrees)
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z < -0.382 && transform.rotation.z > -0.707) {
            // same as above
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
            // setting gravity to 0 to allow for horizontal movement
            myRb.gravityScale = 0;
            animatorif();
            Debug.Log("Jump right");
            animator.SetBool("Walk", false);
            right = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        // player is landing after jumping right
        else if (transform.position.x - overlord.transform.position.x < 0 && !isGrounded && right) {
            // same as above
            isGrounded = true;
            myRb.isKinematic = true;
            myRb.velocity = Vector2.zero;
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            right = false;
            // re enable gravity for next jump
            myRb.gravityScale = 1;
            Debug.Log("I'm grounded");
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
        // player is dropping down (has to be at same angles as vertical jump)
        else if (playerscript.active && isGrounded && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            // same as above, just drops
            isGrounded = false;
            myRb.isKinematic = false;
            animatorif();
            animator.SetBool("Walk", false);
            spritemask.SetActive(false);
            down = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        // player falls down 20 units without hitting new line
        else if (overlord.transform.position.y - transform.position.y > 20 && !isGrounded && down) {

            isGrounded = true;
            myRb.velocity = Vector2.zero;
            myRb.isKinematic = true;    
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            down = false;
            spritemask.SetActive(true); 
            Debug.Log("I'm grounded");
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
    }

    // function to determine which animation to play when jumping
    void animatorif() {
        if (playerscript.fire || playerscript.onFire) {
            playerscript.fireAnim.SetTrigger("Jump");
        }
        else if (playerscript.ice || playerscript.onIce) {
            playerscript.iceAnim.SetTrigger("Jump");
        }
        else if (playerscript.electric || playerscript.onElectric) {
            playerscript.electricAnim.SetTrigger("Jump");
        }
        else {
            playerscript.animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate() {
        // simulating gravity for horizontal jumps
        // 0.196 is 9.8 m/s squared divided by 50 (50 fixed updates per second)
        if (left) {
            Vector2 newv = new Vector2(myRb.velocity.x + 0.196f, 0);
            myRb.velocity = newv;
        }
        else if (right) {
            Vector2 newv = new Vector2(myRb.velocity.x - 0.196f, 0);
            myRb.velocity = newv;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("hello");
        // if colliding with a node (or fire node) that is on a new line and the player is active and in a jump
        if ((other.gameObject.tag == "Node" || other.gameObject.tag == "Fire Node") && other.gameObject.transform.parent.gameObject != path && playerscript.active && !isGrounded) {
            Debug.Log(other.gameObject.transform.parent + " " + path);
            // setting variables for new path and player
            node = other.gameObject.GetComponent<Node>();
            otherpath = other.gameObject.transform.parent.GetComponent<Path>();
            otherplayer = otherpath.player.GetComponent<Player>();

            // hides current player and sets camera to new player
            playerscript.disappear();
            mainCamera.SetTarget(otherplayer.gameObject);
            // sets new player to active and sets new player's abilities to old player's abilities
            otherplayer.appear();
            playerscript.active = false;
            otherplayer.active = true;
            otherplayer.fire = playerscript.fire;
            otherplayer.ice = playerscript.ice;
            otherplayer.electric = playerscript.electric;
            otherplayer.fireTimer = playerscript.fireTimer;
            otherplayer.iceTimer = playerscript.iceTimer;
            otherplayer.electricTimer = playerscript.electricTimer;
            playerscript.fire = false;
            playerscript.ice = false;
            playerscript.electric = false;
            // snaps new player to the node (see path script for more info on snapping)
            otherpath.SnapPlayer(other.gameObject);

            // if jumping from within a element, sets timer for new player to have that element
            if (playerscript.onFire) {
                otherplayer.fire = true;
                otherplayer.fireTimer = 2;
                playerscript.onFire = false;
            }
            else if (playerscript.onIce) {
                otherplayer.ice = true;
                otherplayer.iceTimer = 2;
                playerscript.onIce = false;
            }
            else if (playerscript.onElectric) {
                otherplayer.electric = true;
                otherplayer.electricTimer = 2;
                playerscript.onElectric = false;
            }

            node.ping();
        }
        if (other.gameObject.tag == "Wall") {
            // if hitting a wall while dropping down
            if (down) {
                // returns back to line
                isGrounded = true;
                myRb.velocity = Vector2.zero;
                myRb.isKinematic = true;    
                transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
                down = false;
                spritemask.SetActive(true); 
                Debug.Log("I'm grounded");
                pathscript.stopLeft = prevleft;
                pathscript.stopRight = prevright;
            }
            // stop momentum if hitting wall while jumping
            else {
                myRb.velocity = Vector2.zero;
                Debug.Log("Wall");
            }
            wall = other.gameObject.GetComponent<Box>();
            // same as player script
            // if hitting ice wall while on fire, melts wall
            // if hitting fire wall while on ice, destroys wall
            if (wall.isIce && (playerscript.onFire || playerscript.fire) && playerscript.active) {
                StartCoroutine(wall.melt());
                //other.gameObject.SetActive(false);
            }

            if (wall.isFire && (playerscript.onIce || playerscript.ice)) {
                other.gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Wall");
    }


}
