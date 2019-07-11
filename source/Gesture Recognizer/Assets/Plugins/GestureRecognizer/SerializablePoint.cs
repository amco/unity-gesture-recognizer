using System;

using PDollar = PDollarGestureRecognizer;

namespace GestureRecognizer
{
    [Serializable]
    public class SerializablePoint : PDollar.Point
    {
        #region CONSTRUCTORS

        public SerializablePoint(float x, float y, int strokeId) : base(x, y, strokeId) { }

        #endregion
    }
}
