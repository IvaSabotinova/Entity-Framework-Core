using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiniORM
{
    public class DbSet<TEntity> : ICollection<TEntity>
    where TEntity : class, new()
    {
        internal ChangeTracker<TEntity> ChangeTracker { get; set; }
        internal IList<TEntity> Entities { get; set; }

        public int Count => this.Entities.Count;

        public bool IsReadOnly => Entities.IsReadOnly;

        public DbSet(IEnumerable<TEntity> entities)
        {
            Entities = entities.ToList();
            ChangeTracker = new ChangeTracker<TEntity>(entities);
        }
        public void Add(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item), "Item cannot be null!");
            }
            Entities.Add(item);
            ChangeTracker.Add(item);
        }

        public void Clear()
        {
            while (Entities.Any())
            {
                TEntity entity = Entities.First();
                this.Remove(entity);
            }
        }

        public bool Contains(TEntity item) => Entities.Contains(item);


        public void CopyTo(TEntity[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);

        public bool Remove(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item), "Item cannot be null!");
            }
            bool removedSuccessFully = Entities.Remove(item);

            if (removedSuccessFully)
            {
                ChangeTracker.Remove(item);
            }
            return removedSuccessFully;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities.ToArray())
            {
                this.Remove(entity);
            }

        }
    }
}

