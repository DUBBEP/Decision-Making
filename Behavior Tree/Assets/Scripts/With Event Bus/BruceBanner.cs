namespace EventBusImplementation
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BruceBanner : MonoBehaviour
    {
        public Door theDoor;
        public GameObject theTreasure;
        public GameObject doorStandingSpace;
        public GameObject houseLocation;
        public GameObject keys;
        public bool rememberedKeys = false;
        bool executingBehavior = false;
        Task myCurrentTask;

        public void ToggleRememberKeys(bool toggle)
        {
            rememberedKeys = toggle;
        }

        public void ExecuteBehavior()
        {
            if (!executingBehavior)
            {
                executingBehavior = true;
                myCurrentTask = BuildTask_GetTreasue();

                EventBus.StartListening(myCurrentTask.TaskFinished, OnTaskFinished);
                myCurrentTask.run();
            }
        }

        void OnTaskFinished()
        {
            EventBus.StopListening(myCurrentTask.TaskFinished, OnTaskFinished);
            //Debug.Log("Behavior complete! Success = " + myCurrentTask.succeeded);
            executingBehavior = false;
        }

        Task BuildTask_GetTreasue()
        {
            // create our behavior tree based on Millington pg. 344
            // building from the bottom up
            List<Task> taskList = new List<Task>();

            // if door isn't locked, open it
            Task isDoorNotLocked = new IsFalse(theDoor.isLocked);
            Task waitABeat = new Wait(0.5f);
            Task openDoor = new OpenDoor(theDoor);
            taskList.Add(isDoorNotLocked);
            taskList.Add(waitABeat);
            taskList.Add(openDoor);
            Sequence openUnlockedDoor = new Sequence(taskList);

            // did we remember where the keys are
            taskList = new List<Task>();
            Task rememberedWhereKeysAre = new IsTrue(rememberedKeys);
            Task moveToHouse = new MoveKinematicToObject(this.GetComponent<Kinematic>(), houseLocation);
            Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), doorStandingSpace);
            Task unlockTheDoor = new UnlockDoor(theDoor);
            Task pullOutKeys = new SetObjectActive(keys);
            Task putAwayKeys = new SetObjectInactive(keys);
            taskList.Add(rememberedWhereKeysAre);
            taskList.Add(waitABeat);
            taskList.Add(moveToHouse);
            taskList.Add(waitABeat);
            taskList.Add(pullOutKeys);
            taskList.Add(moveToDoor);
            taskList.Add(waitABeat);
            taskList.Add(unlockTheDoor);
            taskList.Add(waitABeat);
            taskList.Add(putAwayKeys);
            taskList.Add(waitABeat);
            taskList.Add(openDoor);
            Sequence rememberKeys = new Sequence(taskList);

            // barge a closed door
            taskList = new List<Task>();
            Task isDoorClosed = new IsTrue(theDoor.isClosed);
            Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
            Task removeLockVisual = new SetObjectInactive(theDoor.lockVisual);
            taskList.Add(isDoorClosed);
            taskList.Add(waitABeat);
            taskList.Add(removeLockVisual);
            taskList.Add(bargeDoor);
            Sequence bargeClosedDoor = new Sequence(taskList);

            // open a closed door, one way or another
            taskList = new List<Task>();
            taskList.Add(openUnlockedDoor);
            taskList.Add(rememberKeys);
            taskList.Add(bargeClosedDoor);
            Selector openTheDoor = new Selector(taskList);

            // get the treasure when the door is closed
            taskList = new List<Task>();
            Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theTreasure.gameObject);
            taskList.Add(moveToDoor);
            taskList.Add(waitABeat);
            taskList.Add(openTheDoor); // one way or another
            taskList.Add(waitABeat);
            taskList.Add(moveToTreasure);
            Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

            // get the treasure when the door is open 
            taskList = new List<Task>();
            Task isDoorOpen = new IsFalse(theDoor.isClosed);
            taskList.Add(isDoorOpen);
            taskList.Add(moveToTreasure);
            Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

            // get the treasure, one way or another
            taskList = new List<Task>();
            taskList.Add(getTreasureBehindOpenDoor);
            taskList.Add(getTreasureBehindClosedDoor);
            Selector getTreasure = new Selector(taskList);

            return getTreasure;
        }
    }

}