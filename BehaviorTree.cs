
/// <summary>
/// Behavior Trees
/// </summary>
public enum TaskResult
{
    SUCCEEDED,
    FAILED,
    RUNNING,
}

public interface ITask
{
    TaskResult DoAction();
}

public abstract class LeafTask : ITask
{
    public abstract TaskResult DoAction();
}

public class Conditional : LeafTask
{
    Func<bool> Functor;
    public Conditional( Func<bool> _func )
    {
        Functor = _func;
    }

    public override TaskResult DoAction()
    {
        bool result = Functor();
        if( result )
            return TaskResult.SUCCEEDED;
        else
            return TaskResult.FAILED;
    }
}

public class Inverter : LeafTask
{
    Func<bool> Functor;
    public Inverter( Func<bool> _func )
    {
        Functor = _func;
    }

    public override TaskResult DoAction()
    {
        bool result = Functor();
        if( result )
            return TaskResult.FAILED;
        else
            return TaskResult.SUCCEEDED;
    }
}


public class Failure : LeafTask
{
    public override TaskResult DoAction()
    {
        return TaskResult.FAILED;
    }
}

public class Success : LeafTask
{
    public override TaskResult DoAction()
    {
        return TaskResult.SUCCEEDED;
    }
}

public class BehaviorAction : LeafTask
{
    Action Actor;
    public BehaviorAction( Action _act )
    {
        Actor = _act;
    }
    public override TaskResult DoAction()
    {
        Actor();
        return TaskResult.SUCCEEDED;
    }
}

public class BehaviorFunction : LeafTask
{
    Func<TaskResult> Functor;

    public BehaviorFunction( Func<TaskResult> _func )
    {
        Functor = _func;
    }

    public override TaskResult DoAction()
    {
        return Functor();
    }
}

public abstract class CompositeTask : ITask
{
    protected List<ITask> Tasks = new List<ITask>();
    public abstract TaskResult DoAction();

    public CompositeTask( params ITask[] children )
    {
        Tasks.AddRange( children );
    }
}

public class Sequencer : CompositeTask
{
    public Sequencer( params ITask[] children )
        : base( children )
    {
    }

    public override TaskResult DoAction()
    {
        foreach( ITask task in Tasks )
        {
            TaskResult result = task.DoAction();
            if( result == TaskResult.RUNNING )
                return TaskResult.RUNNING;
            else if( result == TaskResult.FAILED )
                return TaskResult.FAILED;
        }
        return TaskResult.SUCCEEDED;
    }
}

public class Selector : CompositeTask
{
    public Selector( params ITask[] children )
        : base( children )
    {
    }

    public override TaskResult DoAction()
    {
        foreach( ITask task in Tasks )
        {
            TaskResult result = task.DoAction();
            if( result == TaskResult.RUNNING )
                return TaskResult.RUNNING;
            else if( result == TaskResult.SUCCEEDED )
                return TaskResult.SUCCEEDED;
        }
        return TaskResult.FAILED;
    }
}

public static class BlackBoard
{
    static Dictionary<string, object> treeData = new Dictionary<string, object>();

    public static object GetValue( string key )
    {
        object value;
        if( treeData.TryGetValue( key, out value ) )
        {
            return value;
        }
        return null;
    }

    public static void SetValue( string key, object value )
    {
        treeData[key] = value;
    }
}