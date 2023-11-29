namespace FlipnoteDotNet.Data.Entities
{
    public interface IEntityReference<out E> where E : Entity 
    {
        EntityDatabase Database { get; }
        int Id { get; }
        int Timestamp { get; }        
        E Entity { get; }        

        /// <summary>
        /// Write changes to the database
        /// </summary>
        void Commit();

        /// <summary>
        /// Load the state of the Entity at a given timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        void SwitchTimestamp(int timestamp);

        /// <summary>
        /// Move all the properties' timestamps back or forward by a certain amount.
        /// </summary>
        /// <param name="duration"></param>
        void MoveInTime(int duration);

        /// <summary>
        /// Move all the properties' timestamps at a certain value in time
        /// </summary>
        /// <param name="timestamp"></param>
        void SetInTime(int timestamp);
    }

    internal class EntityReference<E> : IEntityReference<E> where E : Entity
    {
        public EntityDatabase Database { get; }
        public int Id { get; }        
        public int Timestamp { get; private set; }        
        public E Entity { get; private set; }

        public EntityReference(EntityDatabase database, int id, int timestamp, E entity)
        {
            Database = database;
            Id = id;            
            Timestamp = timestamp;
            Entity = entity;
        }

        public void SwitchTimestamp(int timestamp)
        {
            var eRef = Database.FindById(Id, timestamp);
            Timestamp = timestamp;
            Entity = eRef.Entity as E;
        }

        public void Commit()
        {
            Database.Commit(this);
        }

        public void MoveInTime(int duration)
        {
            Database.MoveInTime(Id, duration);
            var eRef = Database.FindById(Id, Timestamp);
            Entity = eRef.Entity as E;
        }

        public void SetInTime(int timestamp) => MoveInTime(timestamp - Timestamp);

        public override string ToString()
        {
            return $"eRef {Id}:{Timestamp}({Entity?.ToString() ?? "null"})";
        }
    }
}
