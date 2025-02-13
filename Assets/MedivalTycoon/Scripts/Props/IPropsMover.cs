using System.Collections;
using System.Collections.Generic;

public interface IPropsMover
{
    public void RegisterProps(Queue<Props> props);
    public IEnumerator FillingPoints();
    public Queue<Props> GetTo(int amount);
}