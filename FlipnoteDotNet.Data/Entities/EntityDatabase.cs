using FlipnoteDotNet.Commons.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace FlipnoteDotNet.Data.Entities
{
    public class EntityDatabase
    {
        private readonly List<EntityData> Entities = new List<EntityData>();

        public IEnumerable<IEntityReference<Entity>> EnumerateEntities(int timestamp = -1)
            => Entities.Select(_ => _.BuildEntity(this, timestamp));

        public IEntityReference<E> FindFirst<E>(int timestamp = -1) where E : Entity
            => Entities.Where(e => e.EntityType == typeof(E))
                .Select(e => e.BuildEntity(this, timestamp) as IEntityReference<E>).FirstOrDefault();

        public IEnumerable<IEntityReference<E>> FindAll<E>(int timestamp = -1) where E : Entity
            => Entities.Where(e => e.EntityType == typeof(E))
            .Select(e => e.BuildEntity(this, timestamp) as IEntityReference<E>);

        public IEntityReference<E> Create<E>() where E : Entity
        {
            var eId = Entities.Count;
            var entityData = new EntityData(eId, typeof(E));
            Entities.Add(entityData);
            return FindById<E>(eId);
        }

        public IEntityReference<E> Create<E>(int id) where E : Entity
        {
            while (id >= Entities.Count) Entities.Add(null);
            if (id < 0)
                throw new InvalidOperationException($"Attempting to create entity with id {id}");
            if (Entities[id] != null)
                throw new InvalidOperationException($"Entity with id {id} already exists");

            var entityData = new EntityData(id, typeof(E));
            Entities[id] = entityData;
            return FindById<E>(id);
        }

        public void RemoveById(int id)
        {
            Entities[id] = null;
        }

        public IEntityReference<Entity> FindById(int id, int timestamp = -1)
        {
            return GetEntityDataById(id).BuildEntity(this, timestamp);
        }

        public IEntityReference<E> FindById<E>(int id, int timestamp=-1) where E : Entity
        {
            return GetEntityDataById(id).BuildEntity(this, timestamp, typeof(E)) as EntityReference<E>;
        }

        public void Commit<E>(EntityReference<E> eRef) where E:Entity
        {
            var eData = GetEntityDataById(eRef.Id);
            eData.CommitEntity(eRef.Entity, eRef.Timestamp);
        }

        public void MoveInTime(int id, int duration)
        {
            var eData = GetEntityDataById(id);
            eData.MoveInTime(duration);            
        }

        private EntityData GetEntityDataById(int id)
        {
            if (id < 0) return null;
            if (id >= Entities.Count)
                throw new IndexOutOfRangeException($"Invalid entity id: {id}. Entities count = {Entities.Count}");
            var eData = Entities[id];
            if (eData == null)
                throw new InvalidOperationException($"Referenced entity with id {id} had been deleted.");
            return eData;
        }

        private class EntityData
        {
            public int Id;
            public string TypeSignature;
            public Type EntityType;
            public Dictionary<string, IEntityProperty> Properties = new Dictionary<string, IEntityProperty>();

            public EntityData(int id, Type type)
            {
                Id = id;
                TypeSignature = type.FullName;
                EntityType = type;

                foreach (var property in type.GetProperties())
                {
                    var propName = property.Name;
                    var propType = property.PropertyType;

                    if(IsEntityList(propType, out var itemType) && itemType.IsSubclassOf(typeof(Entity)))
                    {
                        Properties[propName] = new EntityListProperty { EntityType = itemType };
                    }
                    else
                    {
                        Properties[propName] = new ValueProperty
                        {
                            Value = GetDefaultTypeValue(propType),
                            IsTemporal = property.GetCustomAttribute<TemporalAttribute>() != null
                        };
                    }
                }                
            }

            public IEntityReference<Entity> BuildEntity(EntityDatabase db, int timestamp, Type entityType = null)
            {
                entityType = entityType ?? AssemblyScanner.GetTypeByFullName(TypeSignature);                
                ValidateEntityType(entityType);
                var entity = Activator.CreateInstance(entityType) as Entity;

                foreach(var propEntry in Properties)
                {
                    var eProp = entityType.GetProperty(propEntry.Key);
                    var prop = propEntry.Value;

                    if(prop is ValueProperty valueProperty)
                    {
                        eProp.SetValue(entity, valueProperty.GetValue(timestamp));
                    }
                    else if(prop is EntityListProperty entityListProperty)
                    {
                        var itemType = entityListProperty.EntityType;
                        var listType = typeof(EntityList<>).MakeGenericType(itemType);
                        var list = Activator.CreateInstance(listType) as IEntityList;

                        foreach (var id in entityListProperty.EntityIds)
                            list.AddEntity(db.GetEntityDataById(id).BuildEntity(db, timestamp, itemType));
                        eProp.SetValue(entity, list);
                    }
                }

                var eRefType = typeof(EntityReference<>).MakeGenericType(entityType);
                var eRef = Activator.CreateInstance(eRefType, db, Id, /*timestamp:*/ -1, entity) as IEntityReference<Entity>;
                return eRef; 
            }

            public void CommitEntity(Entity entity, int timestamp)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                var entityType = entity.GetType();
                ValidateEntityType(entityType);
                
                foreach(var propEntry in Properties)
                {
                    var eProp = entityType.GetProperty(propEntry.Key);
                    var prop = propEntry.Value;

                    if (prop is ValueProperty valueProperty)
                    {
                        valueProperty.SetValue(eProp.GetValue(entity), timestamp);
                    }
                    else if (prop is EntityListProperty entityListProperty)
                    {
                        var list = eProp.GetValue(entity) as IEntityList;                        
                        entityListProperty.EntityIds = list.AsEnumerable().Select(_ => _.Id).ToList();                        
                    }
                }

            }

            private void ValidateEntityType(Type entityType)
            {
                if (!entityType.IsSubclassOf(typeof(Entity)))
                    throw new ArgumentException($"Not an entity type: {entityType}");
                if (entityType.FullName != TypeSignature)
                    throw new InvalidOperationException($"Type signature mismatch: expected {TypeSignature}, got {entityType.FullName}");
            }

            public void MoveInTime(int duration)
            {
                foreach (var propEntry in Properties)
                {
                    var prop = propEntry.Value;
                    if (prop is ValueProperty valueProperty && valueProperty.IsTemporal)
                    {
                        valueProperty.MoveInTime(duration);
                    }
                }
            }

            private static object GetDefaultTypeValue(Type target)
            {
                return target.IsValueType ? Activator.CreateInstance(target) : null;
            }

            private static bool IsEntityList(Type target, out Type itemType)
            {
                bool isList = target.IsConstructedGenericType
                  && target.GetGenericTypeDefinition() == typeof(EntityList<>);
                itemType = isList ? target.GetGenericArguments()[0] : null;
                return isList;
            }

            private void BinaryWrite(BinaryWriter bw)
            {                
                bw.Write(Id);
                bw.Write(TypeSignature);
                bw.Write(Properties.Count);
                foreach(var kv in Properties)
                {
                    var key = kv.Key;
                    var val = kv.Value;                    
                    bw.Write(key);

                    if (val is ValueProperty) bw.Write((byte)1);
                    else if (val is EntityListProperty) bw.Write((byte)2);
                    else throw new InvalidOperationException($"Cannot write property value {val?.GetType()?.Name ?? "<null>"}");
                    //Debug.WriteLine(key);                    
                    val.BinaryWrite(bw);
                }
            }

            public static void BinaryWrite(BinaryWriter bw, EntityData ed)
            {
                bw.Write((byte)(ed == null ? 0 : 1));
                ed?.BinaryWrite(bw);
            }

            public static EntityData BinaryRead(BinaryReader br)
            {
                var existsFlag = br.ReadByte();
                if (existsFlag == 0) return null;

                var id = br.ReadInt32();
                var typeName = br.ReadString();
                var typeSgn = AssemblyScanner.GetTypeByFullName(typeName);
                var eData = new EntityData(id, typeSgn);                
                for (int pCount = br.ReadInt32(); pCount > 0; pCount--)
                {                    
                    var key = br.ReadString();
                    var vDesc = br.ReadByte();
                    
                    IEntityProperty vProp;
                    switch (vDesc)
                    {
                        case 1:                            
                            vProp = new ValueProperty();                                
                            break;                            
                        case 2:                            
                            vProp = new EntityListProperty();                                
                            break;                            
                        default:
                            throw new InvalidOperationException("Invalid property value type descriptor");
                    }
                    vProp.BinaryRead(br);
                    eData.Properties[key] = vProp;

                }
                return eData;
            }            
        }

        #region IEntityProperty        

        private interface IEntityProperty 
        {
            void BinaryWrite(BinaryWriter bw);
            void BinaryRead(BinaryReader br);
        }        

        private class ValueProperty : IEntityProperty
        {
            public object Value;

            public bool IsTemporal = false;

            public int HeadTimestamp = 0;
            public SortedDictionary<int, object> Timestamps = new SortedDictionary<int, object>();

            public void BinaryWrite(BinaryWriter bw)
            {
                BinarySerializer.Serialize(bw, Value);
                bw.Write((byte)(IsTemporal ? 1 : 0));
                bw.Write(HeadTimestamp);
                bw.Write(Timestamps.Count);
                foreach(var t in Timestamps)
                {
                    bw.Write(t.Key);
                    BinarySerializer.Serialize(bw, t.Value);
                }
            }

            public void BinaryRead(BinaryReader br)
            {                
                Value = BinarySerializer.Deserialize(br);                
                IsTemporal = br.ReadByte() != 0;                
                HeadTimestamp = br.ReadInt32();               

                var tsCnt = br.ReadInt32();                

                Timestamps.Clear();

                for(int i=0;i<tsCnt;i++)
                {
                    int key = br.ReadInt32();
                    var value = BinarySerializer.Deserialize(br);
                    Timestamps.Add(key, value);
                }                               
            }

            public object GetValue(int timestamp = -1)
            {
                timestamp -= HeadTimestamp;
                if (!IsTemporal) return Value;

                var keys = Timestamps.Keys;
                if(keys.Count == 0 || timestamp<keys.First()) return Value;
                foreach (var key in keys) 
                {
                    if (timestamp < key) continue;
                    return Timestamps[key];
                }
                return Timestamps[keys.Last()];
            }

            public void SetValue(object value, int timestamp = -1)
            {
                timestamp -= HeadTimestamp;
                if (!IsTemporal || timestamp < 0) 
                {
                    Value = value;
                    return;
                }
                Timestamps[timestamp] = value;
            }
        
            public void MoveInTime(int duration)
            {
                HeadTimestamp = Math.Max(0, HeadTimestamp + duration);
            }

            public override string ToString()
            {
                return $"Value property default={Value}, ht={HeadTimestamp}, tsCount={Timestamps.Count}";
            }

        }
      
        private class EntityListProperty : IEntityProperty
        {
            public Type EntityType;
            public List<int> EntityIds = new List<int>();

            public void BinaryWrite(BinaryWriter bw)
            {
                //Debug.WriteLine($"Writing list {EntityType}");
                bw.Write(EntityType.FullName);
                bw.Write(EntityIds.Count);
                foreach (var id in EntityIds)
                    bw.Write(id);
            }

            public void BinaryRead(BinaryReader br)
            {
                EntityType = AssemblyScanner.GetTypeByFullName(br.ReadString());
                EntityIds.Clear();
                var cnt = br.ReadInt32();                
                for(int i=0;i<cnt;i++)
                {
                    EntityIds.Add(br.ReadInt32());
                }
            }
        }
        #endregion


        public void BinaryWrite(BinaryWriter bw)
        {
            bw.Write(Entities.Count);
            for(int i=0; i<Entities.Count; i++)
            {
                EntityData.BinaryWrite(bw, Entities[i]);
            }
        }

        public static EntityDatabase BinaryRead(BinaryReader br)
        {
            var db = new EntityDatabase();            

            var entitiesData = new Dictionary<int, EntityData>();
            for (int i = br.ReadInt32(); i > 0; i--) 
            {
                var eData = EntityData.BinaryRead(br);
                if (eData != null)
                    entitiesData.Add(eData.Id, eData);
            }

            var eCount = entitiesData.Keys.Max() + 1;

            for (int i = 0; i < eCount; i++) db.Entities.Add(null);

            foreach (var kv in entitiesData)
                db.Entities[kv.Key] = kv.Value;
            return db;
        }
    }
}
