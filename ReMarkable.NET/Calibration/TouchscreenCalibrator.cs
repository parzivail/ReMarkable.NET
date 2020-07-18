using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Calibration
{
    /// <summary>
    /// Provides methods to calibrate the touchscreen using minimum mean square error (MMSE)
    /// </summary>
    /// <seealso cref="https://www.analog.com/media/en/technical-documentation/application-notes/AN-1021.pdf"/>
    public class TouchscreenCalibrator
    {
        /// <summary>
        /// The calibration data currently used to transform raw input points
        /// </summary>
        public TouchscreenCalibration Calibration;

        /// <summary>
        /// Creates a new <see cref="TouchscreenCalibrator"/> with the identity transformation
        /// </summary>
        public TouchscreenCalibrator() : this(1, 0, -0.5f, 0, 1, -0.5f)
        {
        }

        public TouchscreenCalibrator(int kx1, int kx2, float kx3, int ky1, int ky2, float ky3)
        {
            Calibration = new TouchscreenCalibration
            {
                Kx1 = kx1,
                Kx2 = kx2,
                Kx3 = kx3,
                Ky1 = ky1,
                Ky2 = ky2,
                Ky3 = ky3
            };
        }

        /// <summary>
        /// Transforms the raw input point into device coordinate space using the calibration data
        /// </summary>
        /// <param name="point">The point to convert</param>
        /// <returns>A point in device coordinates</returns>
        public PointF Apply(PointF point)
        {
            var x = Calibration.Kx1 * point.X + Calibration.Kx2 * point.Y + Calibration.Kx3 + 0.5f;
            var y = Calibration.Ky1 * point.X + Calibration.Ky2 * point.Y + Calibration.Ky3 + 0.5f;

            return new PointF(x, y);
        }

        /// <summary>
        /// Calculates the translations required to map points from the reference point set to the sampled point set
        /// </summary>
        /// <param name="calibrationSet">A map of reference device coordinates to the corresponding sample point</param>
        public void Calibrate(Dictionary<PointF, PointF> calibrationSet)
        {
            if (calibrationSet.Count < 3)
                throw new ArgumentOutOfRangeException(nameof(calibrationSet), calibrationSet.Count,
                    "Insufficient number of calibration points, at least 3 required");

            var samplePoints = calibrationSet.Values.ToList();
            var referencePoint = calibrationSet.Keys.ToList();

            var numPoints = calibrationSet.Count;
            var a = new float[3];
            var b = new float[3];
            var c = new float[3];
            var d = new float[3];

            if (numPoints == 3)
            {
                for (var i = 0; i < 3; i++)
                {
                    a[i] = samplePoints[i].X;
                    b[i] = samplePoints[i].Y;
                    c[i] = referencePoint[i].X;
                    d[i] = referencePoint[i].Y;
                }
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    a[i] = 0;
                    b[i] = 0;
                    c[i] = 0;
                    d[i] = 0;
                }

                for (var i = 0; i < numPoints; i++)
                {
                    a[2] = a[2] + samplePoints[i].X;
                    b[2] = b[2] + samplePoints[i].Y;
                    c[2] = c[2] + referencePoint[i].X;
                    d[2] = d[2] + referencePoint[i].Y;
                    a[0] = a[0] + samplePoints[i].X * samplePoints[i].X;
                    a[1] = a[1] + samplePoints[i].X * samplePoints[i].Y;
                    b[0] = a[1];
                    b[1] = b[1] + samplePoints[i].Y * samplePoints[i].Y;
                    c[0] = c[0] + samplePoints[i].X * referencePoint[i].X;
                    c[1] = c[1] + samplePoints[i].Y * referencePoint[i].X;
                    d[0] = d[0] + samplePoints[i].X * referencePoint[i].Y;
                    d[1] = d[1] + samplePoints[i].Y * referencePoint[i].Y;
                }

                a[0] = a[0] / a[2];
                a[1] = a[1] / b[2];
                b[0] = b[0] / a[2];
                b[1] = b[1] / b[2];
                c[0] = c[0] / a[2];
                c[1] = c[1] / b[2];
                d[0] = d[0] / a[2];
                d[1] = d[1] / b[2];
                a[2] = a[2] / numPoints;
                b[2] = b[2] / numPoints;
                c[2] = c[2] / numPoints;
                d[2] = d[2] / numPoints;
            }

            var k = (a[0] - a[2]) * (b[1] - b[2]) - (a[1] - a[2]) * (b[0] - b[2]);
            Calibration = new TouchscreenCalibration
            {
                Kx1 = ((c[0] - c[2]) * (b[1] - b[2]) - (c[1] - c[2]) * (b[0] - b[2])) / k,
                Kx2 = ((c[1] - c[2]) * (a[0] - a[2]) - (c[0] - c[2]) * (a[1] - a[2])) / k,
                Kx3 = (b[0] * (a[2] * c[1] - a[1] * c[2]) + b[1] * (a[0] * c[2] - a[2] * c[0]) +
                       b[2] * (a[1] * c[0] - a[0] * c[1])) / k,
                Ky1 = ((d[0] - d[2]) * (b[1] - b[2]) - (d[1] - d[2]) * (b[0] - b[2])) / k,
                Ky2 = ((d[1] - d[2]) * (a[0] - a[2]) - (d[0] - d[2]) * (a[1] - a[2])) / k,
                Ky3 = (b[0] * (a[2] * d[1] - a[1] * d[2]) + b[1] * (a[0] * d[2] - a[2] * d[0]) +
                       b[2] * (a[1] * d[0] - a[0] * d[1])) / k
            };
        }
    }
}
