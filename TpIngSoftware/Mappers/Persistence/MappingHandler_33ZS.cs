using System;
using System.Collections.Generic;
using System.Data;

namespace Mappers.Persistence
{
    public static class MappingHandler_33ZS
    {
        public static List<T> MapearFilas_33ZS<T>(DataSet dataSet, Func<DataRow, T> constructor)
        {
            List<T> entidades = new List<T>();

            if (dataSet == null || dataSet.Tables.Count == 0)
                return entidades;

            foreach (DataRow fila in dataSet.Tables[0].Rows)
                entidades.Add(constructor(fila));

            return entidades;
        }
    }
}
