using System.Collections.Generic;
using UnityEngine;

public class AIGuy : MonoBehaviour
{
    public Door door;
    public GameObject goal;
    Task currentTask;

    public void RunTask()
    {
        currentTask = BuildTask();
        currentTask.Run();
    }

    Task BuildTask()
    {
        // Time to build the Behavior Tree
        List<Task> taskList = new List<Task>();

        // Open Locked Door
        Task checkLockedDoor = new IsDoorUnlocked(door.isLocked);
        Task openDoor = new OpenDoor(door);
        taskList.Add(checkLockedDoor);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        // Barge locked door
        taskList = new List<Task>();
        Task isDoorClosed = new IsDoorClosed(door.isOpen);
        Task bargeTheDoor = new BargeDoor(door.GetComponentInChildren<Rigidbody>());
        taskList.Add(isDoorClosed);
        taskList.Add(bargeTheDoor);
        Sequence bargeLockedDoor = new Sequence(taskList);

        // Open or destroy closed door
        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeLockedDoor);
        Selector openTheDoor = new Selector(taskList);

        // walk through the door.
        taskList = new List<Task>();
        Task MoveToDoor = new MoveTo(GetComponent<Kinematic>(), door.gameObject);
        Task MoveToGoal = new MoveTo(GetComponent<Kinematic>(), goal);
        taskList.Add(MoveToDoor);
        taskList.Add(openTheDoor);
        taskList.Add(MoveToGoal);
        Sequence passDoorToGoal = new Sequence(taskList);

        // Pass open door
        taskList = new List<Task>();
        Task isDoorOpen = new IsDoorOpen(door.isOpen);
        taskList.Add(isDoorOpen);
        taskList.Add(MoveToGoal);
        Sequence walkThroughOpenDoor = new Sequence(taskList);

        // Build final Selector
        taskList = new List<Task>();
        taskList.Add(walkThroughOpenDoor);
        taskList.Add(passDoorToGoal);
        Selector WalkToGoal = new Selector(taskList);

        return WalkToGoal;

    }
}
