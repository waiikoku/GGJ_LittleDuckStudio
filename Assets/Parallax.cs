using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;     // Array of all the backgrounds to be parallaxed.
    public float smoothing = 1f;        // How smooth the parallax effect should be.

 public float[] parallaxScales;     // Proportion of the camera's movement to move the backgrounds by.
    private Transform cam;              // Reference to the camera's transform.
    private Vector3 previousCamPos;     // Position of the camera in the previous frame.

    void Awake()
    {
        // Set up camera reference
        cam = Camera.main.transform;
    }

    void Start()
    {
        // The previous frame had the current frame's camera position
        previousCamPos = cam.position;

        // Assigning corresponding parallaxScales
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    void Update()
    {
        // For each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // Set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a target position which is the background's current position with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of this frame
        previousCamPos = cam.position;
    }
}

