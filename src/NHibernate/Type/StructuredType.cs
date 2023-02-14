using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace NHibernate.Type
{
	[Serializable]
	public class StructuredType : MutableType, IParameterizedType
	{
		public const string TypeNameParameter = "TypeName";

		public string TypeName { get; private set; }

		public StructuredType() : this(string.Empty)
		{
		}

		public StructuredType(string typeName) : base(SqlTypeFactory.Structured(typeName))
		{
			this.TypeName = typeName ?? string.Empty;
		}

		public override object DeepCopyNotNull(object value)
		{
			var table = value as DataTable;

			if (table != null)
			{
				return table.Clone();
			}
			else
			{
				return null;
			}
		}

		public override void Set(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			var parameter = (IDbDataParameter) cmd.Parameters[index];
			parameter.Value = value;
		}

		public override object Get(DbDataReader rs, int index, ISessionImplementor session)
		{
			return rs[index] as DataTable;
		}

		public override object Get(DbDataReader rs, string name, ISessionImplementor session)
		{
			return rs[name] as DataTable;
		}

		[Obsolete("This method has no more usages and will be removed in a future version. Override ToLoggableString instead.")]
		public override string ToString(object val)
		{
			var table = val as DataTable;

			if (table != null)
			{
				var builder = new StringBuilder();
				using (var stream = new StringWriter(builder))
				{
					table.WriteXml(stream);
					return builder.ToString();
				}
			}
			else
			{
				return null;
			}
		}

		[Obsolete("This method has no more usages and will be removed in a future version.")]
		public override object FromStringValue(string xml)
		{
			using (var stream = new StringReader(xml))
			{
				var table = new DataTable();
				table.ReadXml(stream);
				return table;
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as StructuredType;

			return other != null && other.TypeName == this.TypeName;
		}

		public override int GetHashCode()
		{
			return this.TypeName.GetHashCode();
		}

		public override string Name
		{
			get { return "Structured"; }
		}

		public override System.Type ReturnedClass
		{
			get { return typeof(DataTable); }
		}

		#region IParameterizedType Members

		public void SetParameterValues(System.Collections.Generic.IDictionary<string, string> parameters)
		{
			var typeName = string.Empty;

			if (parameters.TryGetValue(TypeNameParameter, out typeName) == true)
			{
				this.TypeName = typeName;
			}
		}

		#endregion
	}
}
