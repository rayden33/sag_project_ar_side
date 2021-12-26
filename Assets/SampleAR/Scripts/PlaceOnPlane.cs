using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class PlaceOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;
        [SerializeField]
        GameObject carpetShadowPrefab;
        [SerializeField]
        GameObject placeBtn;
        [SerializeField]
        GameObject unPlaceBtn;
        [SerializeField]
        Text debugText;
        [SerializeField]
        float waitSecondsForMove = 0.3f;

        private bool isBlockTouchPosition = false;
        private Vector3 objectHitPositin;

        [HideInInspector]
        public float initialFingersDistance;
        [HideInInspector]
        public Vector3 initialScale;
        private static Transform ScaleTransform;
        private float timerF = 0;
        private bool isVibrated = false;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }
        public GameObject shadowObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }
        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {




            if (isBlockTouchPosition)
                return;

            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                timerF = 0f;
                isVibrated = false;
                return;
            }

            if (touchPosition.y < Screen.height * 0.1f)
                return;



            //// For scaling and rotation
            if (placeBtn.activeSelf)
            {
                int fingersOnScreen = 0;
                float firstYRotationPos = 0;

                foreach (Touch touch in Input.touches)
                {
                    fingersOnScreen++; //Count fingers (or rather touches) on screen as you iterate through all screen touches.

                    //You need two fingers on screen to pinch.
                    if (fingersOnScreen == 2)
                    {

                        //First set the initial distance between fingers so you can compare.
                        if (touch.phase == TouchPhase.Began)
                        {
                            initialFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                            initialScale = spawnedObject.transform.localScale;

                            firstYRotationPos = spawnedObject.transform.rotation.y;
                        }
                        else
                        {
                            float currentFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);

                            float scaleFactor = currentFingersDistance / initialFingersDistance;

                            //transform.localScale = initialScale * scaleFactor;
                            spawnedObject.transform.localScale = initialScale * scaleFactor;


                            //// rotation 
                            Vector3 diff = Input.touches[1].position - Input.touches[0].position;
                            float angle = (Mathf.Atan2(diff.y, diff.x));
                            spawnedObject.transform.rotation = Quaternion.Euler(0f, firstYRotationPos + (-1.0f * Mathf.Rad2Deg * angle), 0f);
                            //spawnedObject.transform.Rotate(0f, -1.0f * Mathf.Rad2Deg * angle, 0f);
                        }
                        return;
                    }
                }
            }



            
            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon ))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                    spawnedObject.transform.position += spawnedObject.transform.up * .1f;
                    placeBtn.SetActive(true);
                    objectHitPositin = hitPose.position;
                    shadowObject = Instantiate(carpetShadowPrefab, hitPose.position, hitPose.rotation);

                }
                else
                {
                    if(timerF < waitSecondsForMove)
                    {
                        timerF += Time.deltaTime;
                        return;
                    }
                    if(!isVibrated)
                    {
                        AndroidManager.HapticFeedback();
                        isVibrated = true;
                    }
                    spawnedObject.transform.position = hitPose.position;
                    spawnedObject.transform.position += spawnedObject.transform.up * .1f;
                    objectHitPositin = hitPose.position;
                    shadowObject.transform.position = hitPose.position;
                }
            }
        }

        public void PlaceObject()
        {
            
            isBlockTouchPosition = true;
            spawnedObject.GetComponentInChildren<CarpetManager>().PlaceObject();
            spawnedObject.GetComponentInChildren<CarpetShadowController>().PlaceCarpet();
            spawnedObject.transform.position = objectHitPositin;
            placeBtn.SetActive(false);
            unPlaceBtn.SetActive(true);

            //Debug.Log(spawnedObject.GetComponentInChildren<Transform>().localPosition.ToString());
            //debugText.text = myLog;

        }

        public void UnPlaceObject()
        {
            isBlockTouchPosition = false;
            spawnedObject.GetComponentInChildren<CarpetManager>().ChangePlacedStatus();
            spawnedObject.GetComponentInChildren<CarpetShadowController>().UnPlaceCarpet();
            spawnedObject.transform.position += spawnedObject.transform.up * .1f;
            placeBtn.SetActive(true);
            unPlaceBtn.SetActive(false);

        }

        static string myLog = "";
        private string output;
        private string stack;
        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = output + "\n" + myLog;
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4000);
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
    }
}
