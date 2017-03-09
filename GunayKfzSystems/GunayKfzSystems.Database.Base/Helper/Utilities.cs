using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using GunayKfzSystems.Database.Base.Attribute;
using Prism.Logging;



namespace GunayKfzSystems.Database.Base.Helper
{
    public static class Utilities
    {
        public static List<T> DataTableToList<T>(this DataTable table, ILoggerFacade _logger) where T : class, new()
        {
            try
            {
                T tempT = new T();
                var tType = tempT.GetType();
                List<T> list = new List<T>();
                foreach (var row in table.Rows.Cast<DataRow>())
                {
                    T obj = new T();

                    var propList =
                        obj.GetType()
                           .GetProperties().Where(m => !m.GetCustomAttributes<IgnorableAttribute>().Any());

                   
                    foreach (var prop in propList)
                    {
                        var propertyInfo = tType.GetProperty(prop.Name);
                        var rowValue = row[prop.Name];
                        var t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                        try
                        {
                            object safeValue = (rowValue == null || DBNull.Value.Equals(rowValue)) ? null : Convert.ChangeType(rowValue, t);
                            propertyInfo.SetValue(obj, safeValue, null);

                        }
                        catch (Exception ex)
                        {
                            _logger.Log(ex.Message, Category.Exception, Priority.None);
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

    }
}


