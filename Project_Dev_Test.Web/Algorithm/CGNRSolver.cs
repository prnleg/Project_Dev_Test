using MathNet.Numerics.LinearAlgebra;

namespace Project_Dev_Test.Web.Algorithm
{
    public class CGNRSolver
    {
        public static (Vector<double> result, uint iterations) Solve(Vector<double> g)
        {
            uint i;
            Matrix<double> H = Helpers.MatrixModel.H;
            Matrix<double> Ht = Helpers.MatrixModel.Ht;
            Vector<double> f = Vector<double>.Build.Dense(H.ColumnCount, 0.0);
            Vector<double> r = g - H * f;
            Vector<double> z = Ht * r;
            Vector<double> p = z;

            Vector<double> outVector = f;
            double bestError = double.MaxValue;
            double rOldNorm = r.L2Norm();

            for (i = 0; i < 300; i++)
            {
                Vector<double> w = H * p;
                double zNorm = Math.Pow(z.L2Norm(), 2);
                double alpha = zNorm / Math.Pow(w.L2Norm(), 2);
                f = f + alpha * p;
                r = r - alpha * w;
                double error = Math.Abs(r.L2Norm() - rOldNorm);
                if (error < bestError)
                {
                    bestError = error;
                    outVector = f;
                }
                if (error < 1e-8) break;
                z = Ht * r;
                double beta = Math.Pow(z.L2Norm(), 2) / zNorm;
                p = z + beta * p;
                rOldNorm = r.L2Norm();
            }

            if (i >= 300)
            {
                i = 300 - 1;
            }

            return (outVector, i + 1);
        }
    }

}
