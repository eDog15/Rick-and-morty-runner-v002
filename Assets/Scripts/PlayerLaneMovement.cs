using UnityEngine;

// This script handles the player's movement between pre-defined lanes.
// It supports both keyboard input for editor testing and swipe controls for mobile.
public class PlayerLaneMovement : MonoBehaviour
{
    [Header("Lane Settings")]
    public float laneDistance = 2.5f; // Distance between the centers of two adjacent lanes
    public float moveSpeed = 15f;     // How fast the player switches between lanes

    [Header("Swipe Controls")]
    public float minSwipeDistance = 50f; // Minimum distance in pixels for a swipe to be registered

    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    private Vector3 targetPosition;

    private Vector2 touchStartPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        // Handle platform-specific input
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleKeyboardInput();
#endif

#if UNITY_IOS || UNITY_ANDROID
        HandleSwipeInput();
#endif

        // Smoothly move the player towards the target lane position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(false); // Move Left
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(true); // Move Right
        }
    }

    private void HandleSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 touchEndPosition = touch.position;
                float swipeDistanceX = touchEndPosition.x - touchStartPosition.x;
                float swipeDistanceY = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(swipeDistanceX) > minSwipeDistance && Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY))
                {
                    if (swipeDistanceX > 0)
                    {
                        MoveLane(true); // Swipe Right
                    }
                    else
                    {
                        MoveLane(false); // Swipe Left
                    }
                }
            }
        }
    }

    public void MoveLane(bool goingRight)
    {
        currentLane += goingRight ? 1 : -1;
        currentLane = Mathf.Clamp(currentLane, 0, 2);

        float targetX = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
