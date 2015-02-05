using System;
using System.Data;

namespace NHibernate.SqlTypes
{
	[Serializable]
	public class StructuredSqlType : SqlType
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StringSqlType"/> class.
		/// </summary>
		public StructuredSqlType(string typeName) : base(DbType.Object)
		{
			this.TypeName = typeName ?? string.Empty;
		}

		public string TypeName { get; private set; }

		public override bool Equals(object obj)
		{
			var other = obj as StructuredSqlType;

			return other != null && other.TypeName == this.TypeName;
		}

		public override int GetHashCode()
		{
			return this.TypeName.GetHashCode();
		}
	}
}
