using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraFollow2DLERP : MonoBehaviour {

      public GameObject target;
      public float camSpeed = 4.0f;
      public bool follow = true;
      void Start(){
      }

      void FixedUpdate () {
            if (!Input.GetMouseButton(0) && follow) {
                  Vector2 pos = Vector2.Lerp ((Vector2)transform.position, (Vector2)target.transform.position, camSpeed * Time.fixedDeltaTime);
                  transform.position = new Vector3 (pos.x, pos.y, transform.position.z);
            }
      }

      public void SetTarget(GameObject newTarget) {
            target = newTarget;
      }
}