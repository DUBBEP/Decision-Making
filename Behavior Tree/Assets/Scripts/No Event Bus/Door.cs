using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked { get; private set; }
    public bool isOpen {  get; private set; }

    private void Start()
    {
        isLocked = false;
        isOpen = false;
    }

    public void SetOpen(bool toggle)
    {
        isOpen = toggle;
    }

    public void SetLock(bool toggle)
    {
        isLocked = toggle;
    }

    public void OpenDoor()
    {
        Debug.Log("Opening The door");
        if (!isOpen && !isLocked)
        {
            Debug.Log("Open Door successful");
            isOpen = true;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.eulerAngles = new Vector3(0, -135, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
