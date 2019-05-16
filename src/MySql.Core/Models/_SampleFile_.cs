using CluedIn.Core.Data;
using System.Collections.Generic;

namespace CluedIn.Crawling.MySql.Core.Models
{
    public class Model
    {
        public List<object> Columns { get; set; }

        public TableMapping TableMapping { get; set; }
    }

    public class TableMapping
    {
        public string Table { get; set; }

        public List<Column> Columns { get; set; }

        public List<Key> Keys { get; set; }

        public EntityType EntityType { get; set; }

        public bool IsJoinTable { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }

        public int OrdinalPosition { get; set; }

        public string Default { get; set; }

        public bool IsNullable { get; set; }

        public bool IsId { get; set; }

        public string DataType { get; set; }

        public int MaxLength { get; set; }

        public int OctetLength { get; set; }

        public string CluedInFieldMapping { get; set; }
    }

    public class Key
    {
        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsForeign { get; set; }

        public string Type { get; set; }

        public string FieldSource { get; set; }

        public string FieldTarget { get; set; }

        public EntityEdgeType EdgeType { get; set; }
    }
}
