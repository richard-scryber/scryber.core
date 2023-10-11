using System;
namespace Scryber.Drawing
{
    //Take the GDPI code from https://source.winehq.org/source/dlls/gdiplus/matrix.c

    public struct Matrix2D
    {
        [Flags]
        private enum MatrixTypes
        {
            IsIdentity = 0,
            IsTranslation = 1,
            IsScaling = 2,
            IsUnknown = 4
        }

        private static readonly Matrix2D _identity = new Matrix2D(1, 0, 0, 1, 0, 0);
        
        private double _m11, _m12, _m21, _m22, _dx, _dy;
        private MatrixTypes _type;

        public double[] Elements { get { return new double[] { _m11, _m12, _m21, _m22, _dx, _dy }; } }

        public bool IsIdentity => Matrix2D.Equals(this, Matrix2D.Identity);

        public Matrix2D(Matrix2D other)
        {
            _m11 = other._m11;
            _m12 = other._m12;
            _m21 = other._m21;
            _m22 = other._m22;
            _dx = other._dx;
            _dy = other._dy;
            _type = other._type;
        }

        public Matrix2D(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _dx = dx;
            _dy = dy;
            _type = MatrixTypes.IsUnknown;
        }
        
        private Matrix2D(double m11, double m12, double m21, double m22, double dx, double dy, MatrixTypes type)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _dx = dx;
            _dy = dy;
            _type = type;
        }

        public static Matrix2D Identity => Matrix2D._identity;

        public void Translate(double x, double y)
        {
            this._dx += x;
            this._dy += y;
            this._type |= MatrixTypes.IsTranslation;
        }

        public void Scale(double x, double y)
        {
            this *= CreateScaling(x, y);
        }

        public void Skew(double x, double y)
        {
            this *= CreateSkew(x, y);
        }

        public void RotateDegrees(double angleDegrees)
        {
            angleDegrees %= 360.0;
            this.Rotate(angleDegrees * (Math.PI / 180.0));
        }
        
        public void Rotate(double angleRads)
        {
            this *= CreateRotation(angleRads);
        }

        public void Reset()
        {
            var ident = Identity;
            this._m11 = ident._m11;
            this._m12 = ident._m12;
            this._m21 = ident._m21;
            this._m22 = ident._m22;
            this._type = ident._type;
            this._dx = ident._dx;
            this._dy = ident._dy;
        }

        public Point[] TransformPoints(Point[] all)
        {
            if (null == all)
                return null;
            
            var transformed = new Point[all.Length];
            
            for (int i = 0; i < all.Length; i++)
            {
                transformed[i] = TransformPoint(all[i]);
            }
            
            return transformed;
        }


        public Point TransformPoint(Point point)
        {
            double x = point.X.PointsValue;
            double y = point.Y.PointsValue;
            
            TransformPoint(ref x, ref y);
            
            return new Point(x, y);
        }
        
        
        public void TransformPoint(ref double x, ref double y)
        {
            double xadd = y * _m21 + _dx;
            double yadd = x * _m12 + _dy;
            x *= _m11;
            x += xadd;
            y *= _m22;
            y += yadd;
        }


        public Matrix2D Clone()
        {
            return (Matrix2D)this.MemberwiseClone();
        }


        public Matrix2D Multiply(Matrix2D other)
        {
            return this * other;
        }

        public override string ToString()
        {
            return $"[{Math.Round(_m11, 4)}, {Math.Round(_m12, 4)}, {Math.Round(_m21, 4)}, {Math.Round(_m22, 4)}, {Math.Round(_dx, 4)}, {Math.Round(_dy, 4)}]";
        }

        //operator methods
        public static Matrix2D operator *(Matrix2D one, Matrix2D two)
        {
            return Matrix2D.Multiply(one, two);
        }

        //static methods

        public static bool Equals(Matrix2D one, Matrix2D other)
        {
            return one._m11 == other._m11 && one._m12 == other._m12 &&
                   one._m21 == other._m21 && one._m22 == other._m22 &&
                   one._dx == other._dx && one._dy == other._dy;
        }

        public static Matrix2D Multiply(Matrix2D one, Matrix2D two)
        {
            if (two._type == MatrixTypes.IsIdentity)
                return one;

            if (one._type == MatrixTypes.IsIdentity)
                return two;

            Matrix2D result = new Matrix2D(

                (one._m11 * two._m11) + (one._m21 * two._m12),
                (one._m12 * two._m11) + (one._m22 * two._m12),

                (one._m11 * two._m21) + (one._m21 * two._m22),
                (one._m12 * two._m21) + (one._m22 * two._m22),

                (one._m11 * two._dx) + (one._m21 * two._dy) + one._dx,
                (one._m12 * two._dx) + (one._m22 * two._dy) + one._dy);
                //one._dx * two._m11 + one._dy * two._m21 + two._dx,
                //one._dx * two._m12 + one._dy * two._m22 + two._dy);

            return result;
        }


        private static Matrix2D CreateScaling(double scaleX, double scaleY)
        {
            var result = new Matrix2D(
                scaleX, 0, 
                0, scaleY, 
                0, 0, 
                MatrixTypes.IsScaling);
            return result;
        }

        private static Matrix2D CreateSkew(double skewX, double skewY)
        {
            var result = new Matrix2D(
                1.0, skewY,
                skewX, 1.0,
                0, 0, 
                MatrixTypes.IsUnknown);
            return result;
        }


        private static Matrix2D CreateRotation(double angleRads)
        {
            return CreateRotation(angleRads, 0.0, 0.0);
        }
        private static Matrix2D CreateRotation(double angleRads, double centreX, double centreY)
        {
            double sin = Math.Sin(angleRads);
            double cos = Math.Cos(angleRads);

            double dx  = (centreX * (1.0 - cos)) + (centreY * sin);
            double dy  = (centreY * (1.0 - cos)) - (centreX * sin);

            var result = new Matrix2D(
                cos, sin,
                -sin, cos,
                dx, dy,
                MatrixTypes.IsUnknown);

            return result;
        }


        
        

    }
}
