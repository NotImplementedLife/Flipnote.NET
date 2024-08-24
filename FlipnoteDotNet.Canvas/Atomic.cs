namespace FlipnoteDotNet.Canvas
{
    public class Atomic<T>
    {
        private T fValue;
        public T Value
        {
            get
            {
                lock (this) return fValue;
            }
            set
            {
                lock (this) { fValue = value; }
            }
        }

        public Atomic(T value)
        {
            fValue = value;
        }
    }
}
