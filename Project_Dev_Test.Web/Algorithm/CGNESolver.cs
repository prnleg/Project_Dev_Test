using MathNet.Numerics.LinearAlgebra;

namespace Project_Dev_Test.Web.Algorithm
{
    public class CGNESolver
    {
        public static (Vector<double> result, uint iterations) Solve(Vector<double> g)
        {
            uint i;
            Matrix<double> H = Helpers.MatrixModel.H1;
            Matrix<double> Ht = Helpers.MatrixModel.H1t;
            Vector<double> f = Vector<double>.Build.Dense(H.ColumnCount, 0.0);
            Vector<double> r = g - H * f;
            Vector<double> p = Ht * r;

            Vector<double> output = f;
            double bestError = double.MaxValue;
            double rOldNorm = r.Norm(2);

            double alphaNum;
            double alpha, beta;
            double rNorm, error;

            for (i = 0; i < 300; i++)
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
                if (error < 1e-8)
                    break;

                beta = r.DotProduct(r) / alphaNum;
                p = Ht * r + beta * p;

                rOldNorm = rNorm;
            }

            if (i >= 300)
            {
                i = 300 - 1;
            }

            return (output, i + 1);
        }

    }
}
