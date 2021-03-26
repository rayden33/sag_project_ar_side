using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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
        GameObject placeBtn;
        [SerializeField]
        GameObject unPlaceBtn;

        private bool isBlockTouchPosition = false;
        private Vector3 objectHitPositin;

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

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
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
                return;

            if (touchPosition.y < Screen.height * 0.1f)
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
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
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                    spawnedObject.transform.position += spawnedObject.transform.up * .1f;
                    objectHitPositin = hitPose.position;
                }
            }
        }

        public void PlaceObject()
        {
            isBlockTouchPosition = true;
            spawnedObject.transform.position = objectHitPositin;
            spawnedObject.GetComponentInChildren<CarpetManager>().PlaceObject();
            placeBtn.SetActive(false);
            unPlaceBtn.SetActive(true);
        }

        public void UnPlaceObject()
        {
            isBlockTouchPosition = false;
            spawnedObject.transform.position += spawnedObject.transform.up * .1f;
            spawnedObject.GetComponentInChildren<CarpetManager>().ChangePlacedStatus();
            placeBtn.SetActive(true);
            unPlaceBtn.SetActive(false);

        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
    }
}
