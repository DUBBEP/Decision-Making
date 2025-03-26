using System.Collections.Generic;

public abstract class Task
{
    public abstract bool Run();
}

public class Selector: Task
{
    List<Task> tasks;

    public Selector(List<Task> tasks)
    {
        this.tasks = tasks;
    }

    public override bool Run()
    {
        foreach (Task t in tasks)
        {
            if (t.Run())
                return true;
        }
        return false;
    }
}

public class Sequence: Task
{
    List<Task> tasks;

    public Sequence(List<Task> tasks)
    {
        this.tasks = tasks;
    }

    public override bool Run()
    {
        foreach (Task t in tasks)
        {
            if (!t.Run())
                return false;
        }
        return true;
    }
}
