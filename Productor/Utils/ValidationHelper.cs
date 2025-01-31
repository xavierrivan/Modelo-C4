using Productor.Models;

namespace Productor.Utils
{
    public static class ValidationHelper
    {
        public static bool EsAnormal(Resultado resultado)
        {
            return resultado.Hemoglobina < 12 || resultado.Hemoglobina > 16 ||
                   resultado.Colesterol > 200 ||
                   resultado.GlobulosRojos < 4.7 || resultado.GlobulosRojos > 6.1;
        }
    }
}
