using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dbtgaming.Game.Library.Magic
{
    /// <summary>
    /// Defined ShapeTemplate
    /// </summary>
    public interface IDefinedShape
    {
        int CalculateDamage(IShape shpe);
        void UpdateShape(IShape shpe);
        void Cast(IShape shpe);
        void BasicCastDraw();
    }

    public interface IPrimalShape : IDefinedShape
    {
        bool Independant { get; }
        List<RecognizedShape> Recognize(IShape _shape);
    }

    public interface ISecondaryShape : IDefinedShape
    {
        IPrimalShape shape1 { get; }
        IPrimalShape shape2 { get; }

        
    }

    public interface ITertiaryShape : IDefinedShape
    {
        IDefinedShape shape1 { get; }
        IDefinedShape shape2 { get; }
    }

    public class RecognizedShape
    {
        public RecognizedShape(IDefinedShape defined)
        {
            _definedShape = defined;
        }

        protected IDefinedShape _definedShape;
        public IDefinedShape DefinedShape
        {
            get
            {
                return _definedShape;
            }
            set
            {
                _definedShape = value;
            }
        }

        protected int[] _points;
        public int[] Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        protected int[] _originCon;
        public int[] OriginCon
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        protected int[] _destCon;
        public int[] DestCon
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }
    }
}
