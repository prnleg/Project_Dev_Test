namespace Project_Dev_Test.Web.Algorithm
{
    public class CGNESolver
    {
        // Tolerância para a convergência
        // Número máximo de iterações

        // Baixa
        private const double Tolerance = 1e-4;
        private const int MaxIterations = 20;

        // Média
        //private const double Tolerance = 1e-8;
        //private const int MaxIterations = 50;

        // Alta
        //private const double Tolerance = 1e-10;
        //private const int MaxIterations = 100;

        public static double[] Solve(double[,] A, double[] b)
        {
            int n = b.Length;
            int m = A.GetLength(1);
            double[] x = new double[n];
            double[] r = new double[n];
            double[] p = new double[n];
            double[] Ap = new double[n];
            double rr = 0;
            double pAp = 0;

            // Inicialização
            for (int i = 0; i < n; i++)
            {
                x[i] = 0;
                r[i] = b[i];
                p[i] = r[i];
            }

            for (int iteration = 0; iteration < MaxIterations; iteration++)
            {
                double[] rOld = (double[])r.Clone();

                // Computar Ap = A * p
                for (int i = 0; i < n; i++)
                {
                    Ap[i] = 0;
                    for (int j = 0; j < m; j++)
                    {
                        Ap[i] += A[i, j] * p[i];
                    }
                }

                // Computar alpha
                for (int i = 0; i < n; i++)
                {
                    rr += r[i] * r[i];
                    pAp += p[i] * Ap[i];
                }
                double alpha = rr / pAp;

                // Atualizar x e r
                for (int i = 0; i < n; i++)
                {
                    x[i] += alpha * p[i];
                    r[i] -= alpha * Ap[i];
                }

                // Verificar critério de parada
                double residualNorm = 0;
                for (int i = 0; i < n; i++)
                {
                    residualNorm += r[i] * r[i];
                }
                if (Math.Sqrt(residualNorm) < Tolerance)
                {
                    break;
                }

                // Computar beta
                double beta = 0;
                for (int i = 0; i < n; i++)
                {
                    beta += r[i] * r[i] / (rr + 1e-10); // Adicionar 1e-10 para evitar divisão por zero
                }

                // Atualizar p
                for (int i = 0; i < n; i++)
                {
                    p[i] = r[i] + beta * p[i];
                }

                // Atualizar rr
                rr = residualNorm;
            }

            return x;
        }
    }
}
