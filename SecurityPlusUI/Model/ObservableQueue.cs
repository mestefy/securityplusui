using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityPlusUI.Model
{
    public sealed class ObservableQueue<T> : ObservableCollection<T>
    {
        private int capacity;

        public ObservableQueue(int capacity = 100)
        {
            this.capacity = capacity;
        }

        public void Enqueue(T item)
        {
            base.Insert(0, item);
            if (base.Count > this.capacity)
            {
                this.Dequeue();
            }
        }

        public T Dequeue()
        {
            var position = base.Count - 1;
            var item = base[position];
            base.RemoveAt(position);

            return item;
        }
    }
}
