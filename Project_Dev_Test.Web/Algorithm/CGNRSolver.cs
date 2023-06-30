using MathNet.Numerics.LinearAlgebra;

namespace Project_Dev_Test.Web.Algorithm
{
    public class CGNRSolver : AlgorithmBase
    {
        public static (Vector<double> result, uint iterations) Solve(Vector<double> g, int gSize)
        {
            uint i;
            Matrix<double> H = gSize == 60 ? Helpers.MatrixModel.H1 : Helpers.MatrixModel.H2;
            Matrix<double> Ht = gSize == 60 ? Helpers.MatrixModel.H1t : Helpers.MatrixModel.H2t;
            Vector<double> f = Vector<double>.Build.Dense(H.ColumnCount, 0.0);
            Vector<double> r = g - H * f;
            Vector<double> z = Ht * r;
            Vector<double> p = z;

            Vector<double> outVector = f;
            double bestError = double.MaxValue;
            double rOldNorm = r.L2Norm();

            double zNorm, rNorm;
            Vector<double> w;
            double alpha;
            double error;

            for (i = 0; i < MAX_ITERATIONS; i++)
            {
                w = H * p;
                zNorm = Math.Pow(z.L2Norm(), 2);
                alpha = zNorm / Math.Pow(w.L2Norm(), 2);
                f += alpha * p;
                r -= alpha * w;

                rNorm = r.L2Norm();
                error = Math.Abs(rNorm - rOldNorm);

                if (error < bestError)
                {
                    bestError = error;
                    outVector = f;
                }
                if (error < TARGET_ERROR) break;

                z = Ht * r;
                p = z + (Math.Pow(z.L2Norm(), 2) / zNorm) * p;
                rOldNorm = rNorm;
            }

            if (i >= MAX_ITERATIONS)
            {
                i = MAX_ITERATIONS - 1;
            }

            return (outVector, i + 1);
        }
    }

}
