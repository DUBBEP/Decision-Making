namespace EventBusImplementation
{
    using Unity.VisualScripting.Antlr3.Runtime;
    using UnityEngine;

    public class Door : MonoBehaviour
    {
        public bool isClosed = false;
        public bool isLocked = false;
        public GameObject lockVisual;


        Vector3 closedRotation = new Vector3(0, 0, 0);
        Vector3 openRotation = new Vector3(0, -135, 0);

        // Start is called before the first frame update
        void Start()
        {
            UpdateDoor();
        }

        public void SetClosed(bool toggle)
        {
            isClosed = toggle;
            UpdateDoor();
        }

        public void SetLock(bool toggle)
        {
            isLocked = toggle;
            UpdateDoor();
        }

        void UpdateDoor()
        {
            if (isClosed)
            {
                transform.eulerAngles = closedRotation;
            }
            else
            {
                transform.eulerAngles = openRotation;
            }

            if (isLocked && isClosed)
            {
                lockVisual.SetActive(true);
            }
            else
            {
                lockVisual.SetActive(false);
            }
        }


        public bool Open()
        {
            if (isClosed && !isLocked)
            {
                Debug.Log("door is now open");
                isClosed = false;
                transform.eulerAngles = openRotation;
                return true;
            }

            Debug.Log("door was either locked or already open");
            return false;
        }

        public bool Close()
        {
            if (!isClosed)
            {
                Debug.Log("door is now closed");
                transform.eulerAngles = closedRotation;
                isClosed = true;
            }
            return true;
        }

        public bool UnlockDoor()
        {
            if (isClosed && isLocked)
            {
                isLocked = false;
                lockVisual.SetActive(false);
                return true;
            }
            return false;
        }
    }
}