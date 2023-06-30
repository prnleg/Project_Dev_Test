using MathNet.Numerics.LinearAlgebra;

namespace Project_Dev_Test.Web.Algorithm
{
    public class CGNESolver : AlgorithmBase
    {
        public static (Vector<double> result, uint iterations) Solve(Vector<double> g, int gSize)
        {
            uint i;
            Matrix<double> H = gSize == 60 ? Helpers.MatrixModel.H1 : Helpers.MatrixModel.H2;
            Matrix<double> Ht = gSize == 60 ? Helpers.MatrixModel.H1t : Helpers.MatrixModel.H2t;
            Vector<double> f = Vector<double>.Build.Dense(H.ColumnCount, 0.0);
            Vector<double> r = g - H * f;
            Vector<double> p = Ht * r;

            Vector<double> output = f;
            double bestError = double.MaxValue;
            double rOldNorm = r.Norm(2);

            double alphaNum;
            double alpha, beta;
            double rNorm, error;

            for (i = 0; i < MAX_ITERATIONS; i++)
            {
                alphaNum = r.DotProduct(r);
                alpha = alphaNum / p.DotProduct(p);

                f = f + alpha * p;
                r = r - alpha * H * p;
                rNorm = r.Norm(2);

                error = Math.Abs(rNorm - rOldNorm);
                if (error < bestError)
                {
                    bestError = error;
                    output = f;
                }
                if (error < TARGET_ERROR)
                    break;

                beta = r.DotProduct(r) / alphaNum;
                p = Ht * r + beta * p;

                rOldNorm = rNorm;
            }

            if (i >= MAX_ITERATIONS)
            {
                i = MAX_ITERATIONS - 1;
            }

            return (output, i + 1);
        }

    }
}
