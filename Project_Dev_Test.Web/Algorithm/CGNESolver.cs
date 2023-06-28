using MathNet.Numerics.LinearAlgebra;

namespace Project_Dev_Test.Web.Algorithm
{
    public class CGNESolver
    {
        public static (Vector<double> result, uint iterations) Solve(Vector<double> g)
        {
            uint i;
            Matrix<double> H = Helpers.MatrixModel.H;
            Matrix<double> Ht = Helpers.MatrixModel.Ht;
            Vector<double> f = Vector<double>.Build.Dense(H.ColumnCount, 0.0);
            Vector<double> r = g - H * f;
            Vector<double> p = Ht * r;

            Vector<double> output = f;
            double bestError = double.MaxValue;
            double rOldNorm = r.Norm(2);

            for (i = 0; i < 300; i++)
            {
                double alphaNum = r.DotProduct(r);
                double alphaDen = p.DotProduct(p);
                double alpha = alphaNum / alphaDen;
                f = f + alpha * p;
                r = r - alpha * H * p;
                double error = Math.Abs(r.Norm(2) - rOldNorm);
                if (error < bestError)
                {
                    bestError = error;
                    output = f;
                }
                if (error < 1e-8)
                    break;
                double betaNum = r.DotProduct(r);
                double betaDen = alphaNum;
                double beta = betaNum / betaDen;
                p = Ht * r + beta * p;
                rOldNorm = r.Norm(2);
            }

            if (i >= 300)
            {
                i = 300 - 1;
            }

            return (output, i + 1);
        }

    }
}
