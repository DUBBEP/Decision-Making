using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

// Task to check if door is open
public class IsDoorOpen : Task
{
    private bool openState;

    public IsDoorOpen(bool state)
    {
        openState = state;
    }

    public override bool Run()
    {
        Debug.Log("checking if door is open returned: " + openState);

        return openState;
    }
}

// check if door is closed
public class IsDoorClosed : Task
{
    private bool openState;

    public IsDoorClosed(bool state)
    {
        openState = state;
    }

    public override bool Run()
    {
        Debug.Log("checking if door is closed returned: " + openState);
        return !openState;
    }
}

// Task to check if the door is locked
public class IsDoorUnlocked: Task
{
    private bool lockState;

    public IsDoorUnlocked(bool state)
    {
        lockState = state;
    }

    public override bool Run()
    {
        Debug.Log("checking if door is locked returned: " + lockState);
        return !lockState;
    }
}

// Task to open door
public class OpenDoor: Task
{
    Door doorToOpen;

    public OpenDoor(Door doorToOpen)
    {
        this.doorToOpen = doorToOpen;
    }

    public override bool Run()
    {
        Debug.Log("Opening the door");

        doorToOpen.OpenDoor();
        return doorToOpen.isOpen;
    }
}

public class BargeDoor: Task
{
    Rigidbody doorToBarge;

    public BargeDoor(Rigidbody doorToBarge)
    {
        this.doorToBarge = doorToBarge;
    }

    public override bool Run()
    {
        Debug.Log("Barging the door");
        doorToBarge.AddForce(Vector3.up * 10, ForceMode.Impulse);
        return true;
    }
}

public class MoveTo: Task
{
    Arriver mover;
    GameObject targetPosition;

    public MoveTo(Kinematic mover, GameObject target)
    {
        this.mover = mover as Arriver;
        targetPosition = target;
    }

    public override bool Run()
    {
        Debug.Log("Moving to: " + targetPosition.gameObject.name);
        mover.SetTarget(targetPosition);
        return true;
    }
}



