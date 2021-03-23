using System.Collections.Generic;
using UnityEngine;

public enum States { Normal, Slow, Root, Charm, Stun, KnokUp } //Mover todo los estados
public struct FullState
{
    public States state;
    public float time;
    public Vector3 dir;

    public FullState(States state, float time, Vector3 v)
    {
        this.state = state;
        this.time = time;
        this.dir = v;
    }

    public static bool operator <=(FullState a, FullState b)
    {
        return a.state <= b.state;
    }
    public static bool operator >=(FullState a, FullState b)
    {
        return a.state >= b.state;
    }


    public static bool operator ==(FullState a, FullState b)
    {
        return a.state == b.state;
    }

    public static bool operator !=(FullState a, FullState b)
    {
        return a.state != b.state;
    }
}
public class StateMachine : MonoBehaviour
{
    private FullState actualState;
    private FullState normalState = new FullState(States.Normal, 0.0f, new Vector3(0.0f, 0.0f, 0.0f));


    private Queue<FullState> queue;

    private void Start()
    {
        queue = new Queue<FullState>();
    }
    private void Update()
    {
        //Update the states times 
        if (actualState.state != States.Normal)
        {
            actualState.time -= Time.deltaTime;
            Debug.Log(queue.Count);
            for(int i = 0; i < queue.Count; i++)
            {
                queue.ToArray()[i].time -= Time.deltaTime;
            }
        }

        //Change the actual state
        if (actualState.time < 0)
        {
            if (queue.Count != 0) {
                Debug.Log(actualState.time);
                while (queue.Count !=0 && queue.Peek().time < 0)
                {
                    queue.Dequeue();
                }
                actualState = queue.Peek();
            }
            else
                Clean();    
        }
    }

    //Set the state to a normal state
    public void Clean()
    {
        actualState = normalState;
    }

    public FullState GetState()
    {
        return actualState;
    }

    public void Charm(float time, Vector3 v)
    {
        if(actualState.state <= States.Charm)
        {
            actualState = new FullState(States.Charm, time, v);
        }
        else
        {
            int c = ChangePosition(States.Charm);
            if (c == -1 || queue.ToArray()[c].state == States.Charm)
            {
                queue.Enqueue(new FullState(States.Charm, time, v));
            }
            else
            {
                FullState last = queue.ToArray()[c];
                queue.ToArray()[c] = new FullState(States.Charm, time, v);
                queue.Enqueue(last);
            }
        }
    }

    //Set a new State from the 
    public void SetState(States state, float time, Vector3 v)
    {
        if (actualState.state <= state)
        {
            actualState = new FullState(state, time, v);
        }
        else
        {
            int c = ChangePosition(state);
            if (c == -1 || queue.ToArray()[c].state == state)
            {
                queue.Enqueue(new FullState(state, time, v));
            }
            else
            {
                FullState last = queue.ToArray()[c];
                queue.ToArray()[c] = new FullState(state, time, v);
                queue.Enqueue(last);
            }
        }
    }

    private int ChangePosition(States state)
    {
        int i = 0;
        bool found = false;
        while (i < queue.Count && !found)
        {
            found = queue.ToArray()[i].state <= state;
            i++;
        }
        if (found)
            return (i-1);
        else
            return -1;
    }

    private float GetFullQueueTime()
    {
        float t = actualState.time;
        foreach (FullState item in queue)
        {
            t += item.time;
        }

        return t;
    }


}