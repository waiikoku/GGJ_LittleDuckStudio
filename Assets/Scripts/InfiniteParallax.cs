using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteParallax : MonoBehaviour
{
    public Transform[] backgrounds; // Array of all the backgrounds to be scrolled
    private float[] parallaxScales; // Proportion of the camera's movement to move the backgrounds
    public float smoothing = 1f; // How smooth the parallax effect should be

    private Transform cam; // reference to the camera transform
    private Vector3 previousCamPos; // store the position of the camera in the previous frame

    void Awake ()
    {
        // set up the camera reference
        cam = Camera.main.transform;
    }

    void Start ()
    {
        // The previous frame had the current frame's camera position
        previousCamPos = cam.position;
        
        // asigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    void Update ()
    {
        // for each background
        for (int i = 0; i < backgrounds.Length; i++) {
            // the parallax is the opposite of the camera movement since the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        // set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
