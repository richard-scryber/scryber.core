using System;
namespace Scryber.Drawing
{
    //Take the GDPI code from https://source.winehq.org/source/dlls/gdiplus/matrix.c

    public struct Matrix2D
    {
        private double m11, m12, m21, m22, dx, dy;

        public double[] Elements { get { return new double[] { m11, m12, m21, m22, dx, dy }; } }

        public bool IsIdentity { get { return false; } }

        public Matrix2D(Matrix2D other)
        {
            m11 = other.m11;
            m12 = other.m12;
            m21 = other.m21;
            m22 = other.m22;
            dx = other.dx;
            dy = other.dy;
        }


        public void Translate(double x, double y)
        {

        }

        public void Scale(double x, double y)
        {

        }

        public void Shear(double x, double y)
        {

        }

        public void Rotate(double angleRads)
        {

        }

        public void Reset()
        {

        }

        public Point[] TransformPoints(Point[] all)
        {
            return all;
        }


        public Point TransformPoint(Point point)
        {
            return point;
        }


        public Matrix2D Clone()
        {
            return (Matrix2D)this.MemberwiseClone();
        }


        public Matrix2D Multiply(Matrix2D other)
        {
            return this;
        }


        public static Matrix2D Identity
        {
            get { return new Matrix2D() { m11 = 1, m22 = 1 }; }
        }
        
        public static Matrix2D Create(Rect bounds, Point pos)
        {
            return Matrix2D.Identity;
        }
    }
}
