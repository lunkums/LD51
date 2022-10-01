using System;
using System.Collections;
using System.Collections.Generic;

namespace LD51
{
    public class EntityContainer<T> where T : IEntity
    {
        private Dictionary<uint, T> entities;
        private uint lastInstanceNum;

        private Queue<Action> actionQueue;

        public EntityContainer()
        {
            entities = new Dictionary<uint, T>();
            lastInstanceNum = 0;
            actionQueue = new Queue<Action>();

            Main.OnUpdateEnd += ClearActionQueue;
        }

        public IEnumerable List => entities.Values;

        public uint Spawn(T entity)
        {
            entities.Add(lastInstanceNum, entity);
            return lastInstanceNum++;
        }

        public void Despawn(T entity)
        {
            actionQueue.Enqueue(() => entities.Remove(entity.Id));
        }

        // Use this to queue actions that can't be invoked immediately because it would modify an iterator while
        // iterating
        private void ClearActionQueue()
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }
}
