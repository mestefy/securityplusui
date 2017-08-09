using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityPlusUI.Model
{
    public sealed class FifoObservableCollection<T> : IEnumerable<T>
    {
        private readonly int capacity;

        public ObservableCollection<T> Collection { get; set; }

        public FifoObservableCollection(int capacity)
        {
            this.capacity = capacity;
            this.Collection = new ObservableCollection<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Collection.GetEnumerator();
        }

        public void Add(T item)
        {
            if (this.capacity < this.Collection.Count + 1)
            {
                this.Collection.RemoveAt(0);
            }

            this.Collection.Add(item);
        }
    }
}
