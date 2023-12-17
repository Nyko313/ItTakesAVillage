using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DefaultNamespace;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public enum MotionDetected
{
    INITLIALIZING,
    NO_MOTION,
    BACK_FRONT,
    LEFT_RIGHT,
    FLIP_LEFT_RIGHT,
}

public enum MagnitudeLevel
{
    LEVEL0,
    LEVEL1,
    LEVEL2,
    LEVEL3,
    LEVEL4,
}

[Serializable]
public class MovementMapping
{
    public MotionDetected movement;
    public MovementParameters xAxis;
    public MovementParameters yAxis;
    public MovementParameters zAxis;

    public MovementMapping(MotionDetected motion, MovementParameters x, MovementParameters y, MovementParameters z)
    {
        this.movement = motion;
        this.xAxis = x;
        this.yAxis = y;
        this.zAxis = z;
    }
}

[Serializable]
public class MovementParameters
{
    public float threshold;
    public int count;
    public bool invert;

    public MovementParameters(float threshold, int count, bool invert)
    {
        this.threshold = threshold;
        this.count = count;
        this.invert = invert;
    }
    
    public bool analyseFFT(FFTHelper fftHelper, string prefix = "")
    {
        bool result = fftHelper.analyse(this.threshold, this.count, prefix);
        if (invert)
        {
            return !result;
        }

        return result;
    }
}

public class MotionDetection : MonoBehaviour
{
    [Header("Configuration Properties")]
    public int fftSize = 32;
    public float evaluateMovementEveryNSeconds = 2.0f;
    public float keepMovementForSeconds = 1.5f;

    public MovementMapping[] movementMappings = new MovementMapping[]
    {
        new MovementMapping(
            MotionDetected.BACK_FRONT,
            new MovementParameters(10, 1, true),
            new MovementParameters(5, 1, false), 
            new MovementParameters(5, 1, true)
        ),
        new MovementMapping(
            MotionDetected.LEFT_RIGHT,
            new MovementParameters(5, 1, false),
            new MovementParameters(10, 1, true), 
            new MovementParameters(5, 1, true)
        ),
        new MovementMapping(
            MotionDetected.FLIP_LEFT_RIGHT,
            new MovementParameters(5, 1, true),
            new MovementParameters(10, 1, true), 
            new MovementParameters(10, 2, false)
        )
    };
    [Range(0, 10f)]
    public float level0Threshold = 0.0f;
    [Range(0, 10f)]
    public float level1Threshold = 0.0f;
    [Range(0, 10f)]
    public float level2Threshold = 0.0f;
    [Range(0, 10f)]
    public float level3Threshold = 0.0f;
    
    [Header("Values to read for evaluation")]
    public MotionDetected currentMotion = MotionDetected.INITLIALIZING;
    public MagnitudeLevel currentMagnitudeLevel = MagnitudeLevel.LEVEL0;
    public float currentMovementMagnitude = 0.0f;
    
    [Header("Debug information")]
    public float lastFound = 0.0f;
    public float lastEvaluated = 0.0f;
    
    [Header("Game objects for visual debugging, not needed.")]
    private Transform toMoveX;
    private Transform toMoveY;
    private Transform toMoveZ;
    private Transform toMoveMagnitude;
    public TextMeshProUGUI tmpro;
    private List<List<float>> data = new List<List<float>>();
    // Start is called before the first frame update
    private FFTHelper fftHelperX;
    private FFTHelper fftHelperY;
    private FFTHelper fftHelperZ;
    private StreamWriter writer;
    
    public void Start()
    {
        if (tmpro != null)
        {
            tmpro.text = "Initializing...";
        }

        fftHelperX = new FFTHelper(fftSize, false);
        fftHelperY = new FFTHelper(fftSize, false);
        fftHelperZ = new FFTHelper(fftSize, false);

        toMoveX = transform.Find("X");
        toMoveY = transform.Find("Y");
        toMoveZ = transform.Find("Z");
        toMoveMagnitude = transform.Find("Magnitude");
        
        InputSystem.EnableDevice(Accelerometer.current);
        // InputSystem.EnableDevice(AttitudeSensor.current);
        // InputSystem.EnableDevice(Gyroscope.current);
        // InputSystem.EnableDevice(LinearAccelerationSensor.current);
        // InputSystem.EnableDevice(GravitySensor.current);
    }
    public void StartAcc()
    {
        InputSystem.EnableDevice(Accelerometer.current);
        // InputSystem.EnableDevice(AttitudeSensor.current);
        // InputSystem.EnableDevice(Gyroscope.current);
        // InputSystem.EnableDevice(LinearAccelerationSensor.current);
        // InputSystem.EnableDevice(GravitySensor.current);
    }
    
    // Update is called once per frame
    void Update()
    {
        var acceleration = Accelerometer.current.acceleration.ReadValue();
        fftHelperX.insertValue(acceleration.x);
        fftHelperY.insertValue(acceleration.y);
        fftHelperZ.insertValue(acceleration.z);

        currentMovementMagnitude = acceleration.magnitude;

        if (currentMovementMagnitude > level3Threshold)
        {
            currentMagnitudeLevel = MagnitudeLevel.LEVEL4;
        }
        if (currentMovementMagnitude < level2Threshold)
        {
            currentMagnitudeLevel = MagnitudeLevel.LEVEL2;
        }
        if (currentMovementMagnitude < level1Threshold)
        {
            currentMagnitudeLevel = MagnitudeLevel.LEVEL1;
        }
        if (currentMovementMagnitude < level0Threshold)
        {
            currentMagnitudeLevel = MagnitudeLevel.LEVEL0;
        }
        if (Time.time - evaluateMovementEveryNSeconds > lastEvaluated)
        {
            MotionDetected newState = MotionDetected.NO_MOTION;
            foreach (MovementMapping movementMapping in movementMappings)
            {
                bool xMatch = movementMapping.xAxis.analyseFFT(fftHelperX, "X");
                bool yMatch = movementMapping.yAxis.analyseFFT(fftHelperY, "Y");
                bool zMatch = movementMapping.zAxis.analyseFFT(fftHelperZ, "Z");

                if (xMatch && yMatch && zMatch)
                {
                    newState = movementMapping.movement;
                }
            }
            
            if (currentMotion != newState && (newState != MotionDetected.NO_MOTION || Time.time - keepMovementForSeconds > lastFound))
            {
                currentMotion = newState;
                if (tmpro != null)
                {
                    tmpro.text = currentMotion.ToString();
                }

                lastFound = Time.time;
            }

            lastEvaluated = Time.time;
        }

        if (toMoveX != null && toMoveX != null && toMoveX != null && toMoveMagnitude != null)
        {
            toMoveX.position = new Vector3(toMoveX.position.x, acceleration.x, toMoveX.position.z);
            toMoveY.position = new Vector3(toMoveY.position.x, acceleration.y, toMoveY.position.z);
            toMoveZ.position = new Vector3(toMoveZ.position.x, acceleration.z, toMoveZ.position.z);
            toMoveMagnitude.position = new Vector3(toMoveMagnitude.position.x, acceleration.magnitude,
                toMoveMagnitude.position.z);
        }
    }
}
