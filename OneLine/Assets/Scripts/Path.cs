using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// path script
// but on game object with all nodes as children
// should only have nodes as immediate children
// nodes should be in order you want to move in
public class Path : MonoBehaviour
{
    // array of nodes
    private Node [] PathNode;
    // player object
    public GameObject player;

    public GameObject levelCompleteMenuUI;
    public float MoveSpeed;
    LineRenderer lineRenderer;

    // index in array of current node
    int CurrentNode = 0;

    // position between nodes
    float pos;

    // node moving from
    Vector3 startPosition;
    // node moving towards
    private Vector3 CurrentPositionHolder;

    // whether the player is stopped to the right
    public bool stopRight = false;
    // whether the player is stopped to the left
    public bool stopLeft = false;


    // line renderer colors (not used)
    public Color defaultColor = Color.black;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    // previous position (not used)
    float previous;

    // max distance between nodes
    float moveRadius = 3;

    GameObject gameHandler;

    GameHandler gameHandlerScript;

    // min distance between nodes
    float minRadius = 0.64f;

    Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        // gets components
        PathNode = GetComponentsInChildren<Node>();
        playerScript = player.GetComponent<Player>();
                                                       
        player.transform.position = PathNode[0].transform.position;
        CheckNode();
        // Code for setting line renderer for line view (copied from Unity Documentation)
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.08f;
        lineRenderer.positionCount = PathNode.Length;
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        /* SetupLineColors(); */

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        gameHandler = GameObject.FindGameObjectWithTag("GameController");
        gameHandlerScript = gameHandler.GetComponent<GameHandler>();
    }


    // called when moving forward and hitting a new node
    public void CheckNode() {
        // if within array
        if (CurrentNode < PathNode.Length - 1 && CurrentNode >= 0) {
            pos = 0;
            // current position is node moving towards
            CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
            // start position is node moving from
            startPosition = PathNode[CurrentNode].transform.position;
            // between two fire nodes
            if (PathNode[CurrentNode].tag == "Fire Node" && PathNode[CurrentNode + 1].tag == "Fire Node") {
                playerScript.ice = false;
                playerScript.onFire = true;
                playerScript.fire = false;
                if (playerScript.onElectric || playerScript.electric) {
                    playerScript.runExplode = true;
                }
            }
            // going off a fire node
            else if (PathNode[CurrentNode].tag == "Fire Node" && PathNode[CurrentNode + 1].tag != "Fire Node") {
                playerScript.ice = false;
                playerScript.onFire = false;
                playerScript.fire = true;
                playerScript.fireTimer = 2;
            }
            // between two ice nodes
            else if (PathNode[CurrentNode].tag == "Ice Node" && PathNode[CurrentNode + 1].tag == "Ice Node") {
                playerScript.fire = false;
                playerScript.onIce = true;
                playerScript.ice = false;

            }
            // going off an ice node
            else if (PathNode[CurrentNode].tag == "Ice Node" && PathNode[CurrentNode + 1].tag != "Ice Node") {
                playerScript.fire = false;
                playerScript.onIce = false;
                playerScript.ice = true;
                playerScript.iceTimer = 2;
            }
            // not on a fire or ice node
            else {
                playerScript.onFire = false;
                playerScript.onIce = false;
            }


            // rotation
            Quaternion rotation = Quaternion.LookRotation(CurrentPositionHolder - player.transform.position, transform.TransformDirection(Vector3.up));
            if (rotation.z != 0) {
                player.transform.rotation = new Quaternion( 0 , 0 , rotation.z , rotation.w);
            }
            // hard set for bug
            else if (rotation.z == 0 && rotation.x == 0) {
                player.transform.rotation = new Quaternion();
            }
            else {
            }


        }
    }
    // called when moving back and hitting a new node
    void backNode() {
        // if within array
        if (CurrentNode >= 0) {
            // current position is node moving towards (forward)
            CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
            // start position is node moving from (back)
            startPosition = PathNode[CurrentNode].transform.position;
            // position is total distance between nodes (at next node)
            pos = Vector3.Distance(startPosition, CurrentPositionHolder);
            // same as check node but in reverse
            if (PathNode[CurrentNode].tag == "Fire Node" && PathNode[CurrentNode + 1].tag == "Fire Node") {
                playerScript.ice = false;
                playerScript.onFire = true;
                playerScript.fire = false;
                if (playerScript.onElectric || playerScript.electric) {
                    playerScript.runExplode = true;
                }
            }
            else if (PathNode[CurrentNode].tag != "Fire Node" && PathNode[CurrentNode + 1].tag == "Fire Node") {
                playerScript.ice = false;
                playerScript.fire = true;
                playerScript.fireTimer = 2;
            }
            else if (PathNode[CurrentNode].tag == "Ice Node" && PathNode[CurrentNode + 1].tag == "Ice Node") {
                playerScript.fire = false;
                playerScript.onIce = true;
                playerScript.ice = false;

            }
            else if (PathNode[CurrentNode].tag != "Ice Node" && PathNode[CurrentNode + 1].tag == "Ice Node") {
                playerScript.fire = false;
                playerScript.ice = true;
                playerScript.iceTimer = 2;
            }
            else {
                playerScript.onFire = false;
            }

            // rotation
            Quaternion rotation = Quaternion.LookRotation(startPosition - player.transform.position, transform.TransformDirection(Vector3.up));
            if (rotation.z != 0) {
                player.transform.rotation = new Quaternion( 0 , 0 , rotation.z , rotation.w);
            }
            // hard set for bug
            else if (rotation.z == 0 && rotation.y == 0) {
                player.transform.rotation = new Quaternion( 0 , 0 , -0.49531f , 0.50465f);
            }
            // hard set for bug
            else if (rotation.z == 0 && rotation.x == 0) {
                player.transform.rotation = new Quaternion();
            }
            /* Debug.Log(rotation); */
            if (rotation.x == 0) {
            }
            
        }
    }

    // function for updating all nodes
    // does it for cleaner line renderer
    public void updateall() {
        for (int i = 0; i < PathNode.Length; i++) {
            Node node = PathNode[i].GetComponent<Node>();
            node.simulateDrag();
        }
    }

    // function for dragging nodes
    // if node is dragged within radius of prev and post node, it moves to that position
    // if node is dragged outside radius of prev and post node, it moves to the edge of the radius
    public Vector3 checkDrag(Node node) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        player.transform.position = Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos);
        // searches for node in array
        int index = 0;
        for (int i = 0; i < PathNode.Length; i++) {
            Node node1 = PathNode[i].GetComponent<Node>();
            if (node == node1) {
                index = i;
                i = PathNode.Length;
            }
        }


        // if node is one of the nodes player is moving between
        if (index == CurrentNode || index == CurrentNode + 1) {
            // if player is moving on a wall while moving line move away
            if (stopRight && index == CurrentNode + 1) {
                pos -= MoveSpeed;
            }
            else if (stopLeft && index == CurrentNode) {
                pos += MoveSpeed;
            }
            else if (stopRight && index == CurrentNode) {
                pos += MoveSpeed;
            }
            else if (stopLeft && index == CurrentNode + 1) {
                pos -= MoveSpeed;
            }
            movePlayer();
        }

        Vector3 prevNode = new Vector3(0, 0, 0);
        // if moving to first or last node
        if (index == 0 || index == PathNode.Length - 1) {
            if (index == 0) {
                prevNode = PathNode[index + 1].transform.position;
            }
            else if (index == PathNode.Length -1) {
                prevNode = PathNode[index - 1].transform.position;
            }
            // distance between mouse and node before dragged node
            float dist = Vector3.Distance(mousePos, prevNode);
            Vector3 vdist = mousePos - prevNode;
            // if in radius, move to mouse position (all good)
            if (dist < moveRadius && dist > minRadius) {
                return mousePos;
            }
            // if outside radius, move to edge of radius
            else if (dist > moveRadius) {
                return prevNode + OnePointNormalize(vdist);
            }
            // if inside min radius push outside radius
            else if (dist < minRadius){
                return prevNode + OnePointMinNormalize(vdist);
            }
            // something went wrong
            else {
                return node.transform.position;
            
            }
        }
        // moving node between two nodes
        else {
            // node before and node after node being dragged
            Vector3 posNode = PathNode[index + 1].transform.position;
            prevNode = PathNode[index - 1].transform.position;

            // distance between mouse and nodes around it
            float posDist = Vector3.Distance(mousePos, posNode);
            float prevDist = Vector3.Distance(mousePos, prevNode);
            // if in radius of both nodes, move to mouse position (all good)
            if (prevDist < moveRadius && posDist < moveRadius && prevDist > minRadius && posDist > minRadius) {
                return mousePos;
            }
            // if outside radius of next node, move to edge of radius of next node
            else if (posDist > moveRadius && prevDist < moveRadius) {
                Vector3 vdist = mousePos - posNode;
                return posNode + OnePointNormalize(vdist);
            }
            // if outside radius of prev node, move to edge of radius of prev node
            else if (prevDist > moveRadius && posDist < moveRadius) {
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            // if inside both min radius, do not move (error case)
            else if (prevDist < minRadius && posDist < minRadius) {
                return node.transform.position;
            }
            // if inside min radius of next node, push to edge of min radius of next node
            else if (posDist < minRadius) {
                Vector3 vdist = mousePos - posNode;
                return posNode + OnePointMinNormalize(vdist);
            }
            // if inside min radius of prev node, push to edge of min radius of prev node
            else if (prevDist < minRadius) {
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointMinNormalize(vdist);
            }
            // since all other cases have been checked, this is when outside of both radius
            // push to edge of radius of closest node
            else if (prevDist > posDist) {
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            // push to edge of radius of closest node
            else if (posDist > prevDist) {
                Vector3 vdist = mousePos - posNode;
                return posNode + OnePointNormalize(vdist);
            }
            else {
                return node.transform.position;
            }

        }
        
    }

    Vector3 OnePointNormalize(Vector3 dist) {
        Vector3 newPos = dist.normalized;
        return newPos * moveRadius;
    }

    Vector3 OnePointMinNormalize(Vector3 dist) {
        Vector3 newPos = dist.normalized;
        return newPos * minRadius;
    }
    // moves player to pos between start and end position
    void movePlayer() {
        CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
        //Debug.Log(CurrentNode);
        startPosition = PathNode[CurrentNode].transform.position;
        player.transform.position = Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos);

        Quaternion rotation = Quaternion.LookRotation(CurrentPositionHolder - player.transform.position, transform.TransformDirection(Vector3.up));
        player.transform.rotation = new Quaternion( 0 , 0 , rotation.z , rotation.w );
    }
    // bug check for moing before first nodeS
    public void cap() {
        if (CurrentNode == 0 && pos <= 0) {
            player.transform.position = PathNode[0].transform.position;
        }
    }
    // when player changes line, snaps player to node
    public void SnapPlayer(GameObject node) {
        // finds node in array
        int index = 0;
        for (int i = 0; i < PathNode.Length; i++) {
            if (PathNode[i].transform.gameObject == node) {
                index = i;
            }
        }
        Debug.Log("Snappong");
        CurrentNode = index;
        // setting position
        CheckNode();
        // moving player
        playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
        // rotation
        CheckNode();

    }



    // Draw Line between nodes
    void DrawLine() {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var t = Time.time;
        for (int i = 0; i < PathNode.Length; i++)
        {
            lineRenderer.SetPosition(i, PathNode[i].transform.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Vector3.Distance(PathNode[0].transform.position, PathNode[1].transform.position));
        //float move = Input.GetAxis("Horizontal");

        DrawLine();
        // dashing
        float currentMoveSpeed = MoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) {
            currentMoveSpeed *= 2;
        }

        // go forward direction
        if ((Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D))) && !stopRight) {
            // turn around (if facing left, turn)
            if (!playerScript.FaceRight) {
                playerScript.turn();
            }
            // pos is always incrimented
            pos += currentMoveSpeed;
            // checks if not at next node
            if (pos < Vector3.Distance(startPosition, CurrentPositionHolder)) {
                // linear transform (goes to position pos between start and end position)
                playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                // updates position
                pos += currentMoveSpeed;
            } 
            else {
                // here means hit next node
                if (CurrentNode < PathNode.Length - 2 && CurrentNode >= 0) {
                    if (playerScript.active) {
                        Debug.Log("next " + CurrentNode);
                        Debug.Log(PathNode.Length);
                    }
                    CurrentNode++;
                    CheckNode();
                    // updates position
                    pos += currentMoveSpeed;
                    // moves a bit more
                    playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                    // updates position
                    pos += currentMoveSpeed;
                }
                // hit finish node of level
                else if (CurrentNode == PathNode.Length - 2 && PathNode[CurrentNode + 1].tag == "Finish" && playerScript.active) {
                    Debug.Log(PathNode[CurrentNode].tag);
                    if (gameHandlerScript.nextLevelName == "END") {
                        stopRight = true;
                        stopLeft = true;
                        playerScript.Victory();
                    }
                    else {
                        levelCompleteMenuUI.SetActive(true);
                        Time.timeScale = 0f;
                    }
                }
            }
            // removing offset put on at start
            pos -= currentMoveSpeed;
        }

        // go back direction (same as above but in reverse direction)
        else if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)) && !stopLeft) {
            Debug.Log("left");
            // turn around (if facing right, turn)
            if (playerScript.FaceRight) {
                playerScript.turn();
            }
            // same structure as above
            // excepts decrementing pos
            // calls back node instead of check node
            pos -= currentMoveSpeed;
            // checks if not hit previous node
            if (pos > 0) {
                playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                pos -= currentMoveSpeed;
            } 
            else {
                if (CurrentNode > 0) {
                    if (playerScript.active) {
                        Debug.Log("back" + CurrentNode);
                    }
                    CurrentNode--;
                    backNode();
                    pos -= currentMoveSpeed;
                    playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                    pos -= currentMoveSpeed;
                }
            }
            pos += currentMoveSpeed;
        }
        // if not moving, stop player
        else {
            playerScript.Stop();
        }
    
    }
}
