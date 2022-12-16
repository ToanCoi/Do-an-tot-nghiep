using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PG.Library.Service
{
    public class TypeService : ITypeService
    {
        public PropertyInfo GetKeyField(Type type)
        {
            return this.GetKeyFields(type)?.FirstOrDefault();
        }

        public List<PropertyInfo> GetKeyFields(Type type)
        {
            var keys = this.GetProperties<PrimaryKey>(type);
            return keys.Select(x => x.Key).ToList();
        }

        public List<string> GetTableColumns(Type type)
        {
            if(type == null)
            {
                return null;
            }

            var columns = new List<string>();
            var prs = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach ( var p in prs)
            {
                // Loại ignoreField
                var ignore = p.GetCustomAttribute<IgnoreField>(true);
                if (ignore == null)
                {
                    columns.Add(p.Name);
                }
            }

            return columns;
        }

        public string GetTableName(Type type)
        {
            var attr = type.GetCustomAttribute<TableAttribute>(false);
            if(attr == null)
            {
                return null;
            }

            return attr.Name;
        }

        public Dictionary<PropertyInfo, TAttribute> GetProperties<TAttribute>(Type type) where TAttribute : Attribute
        {
            if(type == null)
            {
                return null;
            }

            var props = new Dictionary<PropertyInfo, TAttribute>();
            var prs = type.GetProperties();
            foreach( var p in prs )
            {
                var attr = p.GetCustomAttribute<TAttribute>(true);
                if(attr != null)
                {
                    props.Add(p, attr);
                }
            }

            return props;
        }
    }
}
