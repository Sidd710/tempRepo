using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonPriorityQueue<IEvent>
    {
        internal SkeletonPriorityQueue()
        {
            _heap = new IEvent[0];
        }
        
        #region internal members


        /// <summary>
        /// Gets the first or topmost object in the priority queue, which is the
        /// object with the minimum value.
        /// </summary>
        internal IEvent Top
        {
            get
            {
                return _heap[0];
            }
        }

        internal int Count
        {
            get
            {
                return this._heap.Length;
            }
        }

        /// <summary>
        /// Adds an object to the priority queue.
        /// </summary>
        internal void Push(IEvent value)
        {

            // Increase the size of the array if necessary.
            IEvent[] temp = new IEvent[Count + 1];
            for (int i = 0; i < Count; ++i)
            {
                temp[i] = _heap[i];
            }
            _heap = temp;



            _heap[_heap.Length - 1] = value;
        }

        internal void Siftdown(int startpos, int pos)
        {
            var item = _heap[pos];
            _EdgeEvent itemAsEdge = null;
            _SplitEvent itemAsSplit = null;
            if (item is _EdgeEvent)
            {
                itemAsEdge = item as _EdgeEvent;
            }
            else if (item is _SplitEvent)
            {
                itemAsSplit = item as _SplitEvent;
            }

            while (pos > startpos)
            {
                var parentpos = (pos - 1) >> 1;
                var parent = _heap[parentpos];
                _EdgeEvent parentAsEdge = null;
                _SplitEvent parentAsSplit = null;

                if (parent is _EdgeEvent)
                {
                    parentAsEdge = parent as _EdgeEvent;
                }
                else if (parent is _SplitEvent)
                {
                    parentAsSplit = parent as _SplitEvent;
                }

                if (itemAsEdge != null)
                {
                    if (parentAsEdge != null)
                    {
                        if (itemAsEdge.Distance < parentAsEdge.Distance)
                        {
                            _heap[pos] = parent;
                            pos = parentpos;
                            continue;
                        }
                    }
                    else if (parentAsSplit != null)
                    {
                        if (itemAsEdge.Distance < parentAsSplit.Distance)
                        {
                            _heap[pos] = parent;
                            pos = parentpos;
                            continue;
                        }
                    }
                }
                else if (itemAsSplit != null)
                {
                    if (parentAsEdge != null)
                    {
                        if (itemAsSplit.Distance < parentAsEdge.Distance)
                        {
                            _heap[pos] = parent;
                            pos = parentpos;
                            continue;
                        }
                    }
                    else if (parentAsSplit != null)
                    {
                        if (itemAsSplit.Distance < parentAsSplit.Distance)
                        {
                            _heap[pos] = parent;
                            pos = parentpos;
                            continue;
                        }
                    }
                }

                break;
            }
            _heap[pos] = item;
        }

        void SiftUp(int pos)
        {
            var endpos = this._heap.Length;
            var startpos = pos;
            var newitem = _heap[pos];
            var childpos = 2 * pos + 1;    // leftmost child position
            while (childpos < endpos)
            {
                var cmd_gt = false;
                var rightpos = childpos + 1;
                if (rightpos < endpos)
                {
                    _EdgeEvent rightItemAsEdge = null;
                    _SplitEvent rightItemAsSplit = null;
                    if (this._heap[rightpos] is _EdgeEvent)
                    {
                        rightItemAsEdge = this._heap[rightpos] as _EdgeEvent;
                    }
                    else if (this._heap[rightpos] is _SplitEvent)
                    {
                        rightItemAsSplit = this._heap[rightpos] as _SplitEvent;
                    }

                    _EdgeEvent childItemAsEdge = null;
                    _SplitEvent childItemAsSplit = null;
                    if (this._heap[childpos] is _EdgeEvent)
                    {
                        childItemAsEdge = this._heap[childpos] as _EdgeEvent;
                    }
                    else if (this._heap[rightpos] is _SplitEvent)
                    {
                        childItemAsSplit = this._heap[childpos] as _SplitEvent;
                    }


                    if (rightItemAsEdge != null)
                    {
                        if (childItemAsEdge != null)
                        {
                            if (childItemAsEdge.Distance >= rightItemAsEdge.Distance)
                            {
                                cmd_gt = true;
                            }
                        }
                        else if (childItemAsSplit != null)
                        {
                            if (childItemAsSplit.Distance >= rightItemAsEdge.Distance)
                            {
                                cmd_gt = true;
                            }
                        }
                    }
                    else if (rightItemAsSplit != null)
                    {
                        if (childItemAsEdge != null)
                        {
                            if (childItemAsEdge.Distance >= rightItemAsSplit.Distance)
                            {
                                cmd_gt = true;
                            }
                        }
                        else if (childItemAsSplit != null)
                        {
                            if (childItemAsSplit.Distance >= rightItemAsSplit.Distance)
                            {
                                cmd_gt = true;
                            }
                        }
                    }
                }
                if (cmd_gt)
                {
                    childpos = rightpos;
                }


                this._heap[pos] = this._heap[childpos];
                pos = childpos;
                childpos = 2 * pos + 1;
            }
            this._heap[pos] = newitem;
            Siftdown(startpos, pos);
        }

        /// <summary>
        /// Removes the first node (i.e., the logical root) from the heap.
        /// </summary>
        internal IEvent Pop()
        {
            if (this._heap.Length > 2)
            {
                var result = this._heap[0];
                var heapLast = _heap[_heap.Length - 1];


                IEvent[] temp = new IEvent[this.Count - 1];
                for (int i = 0; i < this.Count - 1; ++i)
                {
                    temp[i] = _heap[i];
                }
                _heap = temp;
                _heap[0] = heapLast;

                if (this._heap.Length > 1)
                {
                    SiftUp(0);
                }
                return result;
            }
            else if (this._heap.Length == 2)
            {
                this._heap = new IEvent[] { this._heap[1] };
                return this._heap[0];
            }
            else if (this._heap.Length == 1)
            {
                this._heap = new IEvent[0];
                return default(IEvent);
            }
            else
            {
                return default(IEvent);
            }

            //if (_count > 1)
            //{
            //    // Loop invariants:
            //    //
            //    //  1.  parent is the index of a gap in the logical tree
            //    //  2.  leftChild is
            //    //      (a) the index of parent's left child if it has one, or
            //    //      (b) a value >= _count if parent is a leaf node
            //    //
            //    //int parent = 0;
            //    //int leftChild = HeapLeftChild(parent);

            //    //while (leftChild < _count)
            //    //{
            //    //    int rightChild = HeapRightFromLeft(leftChild);
            //    //    int bestChild =
            //    //        (rightChild < _count && _comparer.Compare(_heap[rightChild], _heap[leftChild]) < 0) ?
            //    //        rightChild : leftChild;

            //    //    // Promote bestChild to fill the gap left by parent.
            //    //    _heap[parent] = _heap[bestChild];

            //    //    // Restore invariants, i.e., let parent point to the gap.
            //    //    parent = bestChild;
            //    //    leftChild = HeapLeftChild(parent);
            //    //}

            //    // Fill the last gap by moving the last (i.e., bottom-rightmost) node.
            //    _heap[parent] = _heap[_count - 1];
            //}

        }

        #endregion

        #region private members

        /// <summary>
        /// Calculate the parent node index given a child node's index, taking advantage
        /// of the "shape" property.
        /// </summary>
        private static int HeapParent(int i)
        {
            return (i - 1) / 2;
        }

        /// <summary>
        /// Calculate the left child's index given the parent's index, taking advantage of
        /// the "shape" property. If there is no left child, the return value is >= _count.
        /// </summary>
        private static int HeapLeftChild(int i)
        {
            return (i * 2) + 1;
        }

        /// <summary>
        /// Calculate the right child's index from the left child's index, taking advantage
        /// of the "shape" property (i.e., sibling nodes are always adjacent). If there is
        /// no right child, the return value >= _count.
        /// </summary>
        private static int HeapRightFromLeft(int i)
        {
            return i + 1;
        }

        private IEvent[] _heap;

        #endregion
    }
}

