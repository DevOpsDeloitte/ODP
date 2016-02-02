﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ODPTaxonomyReportDAL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="ODP_Taxonomy_DEV")]
	public partial class ReportDataLinqDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ReportDataLinqDataContext() : 
				base(global::ODPTaxonomyReportDAL.Properties.Settings.Default.ODP_Taxonomy_DEVConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ReportDataLinqDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ReportDataLinqDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ReportDataLinqDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ReportDataLinqDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.rpt_IQCode_Abstracts_ByDate")]
		public ISingleResult<rpt_IQCode_Abstracts_ByDateResult> rpt_IQCode_Abstracts_ByDate([global::System.Data.Linq.Mapping.ParameterAttribute(Name="DateStart", DbType="VarChar(100)")] string dateStart, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="DateEnd", DbType="VarChar(100)")] string dateEnd)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), dateStart, dateEnd);
			return ((ISingleResult<rpt_IQCode_Abstracts_ByDateResult>)(result.ReturnValue));
		}
	}
	
	public partial class rpt_IQCode_Abstracts_ByDateResult
	{
		
		private string _Date;
		
		private string _GroupUsers;
		
		private string _Flagged_Y_N;
		
		private System.Nullable<int> _Appl_ID;
		
		private string _PI_Name;
		
		private string _Title;
		
		public rpt_IQCode_Abstracts_ByDateResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Date", DbType="VarChar(30)")]
		public string Date
		{
			get
			{
				return this._Date;
			}
			set
			{
				if ((this._Date != value))
				{
					this._Date = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GroupUsers", DbType="VarChar(500)")]
		public string GroupUsers
		{
			get
			{
				return this._GroupUsers;
			}
			set
			{
				if ((this._GroupUsers != value))
				{
					this._GroupUsers = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Flagged Y/N]", Storage="_Flagged_Y_N", DbType="VarChar(1) NOT NULL", CanBeNull=false)]
		public string Flagged_Y_N
		{
			get
			{
				return this._Flagged_Y_N;
			}
			set
			{
				if ((this._Flagged_Y_N != value))
				{
					this._Flagged_Y_N = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[Appl ID]", Storage="_Appl_ID", DbType="Int")]
		public System.Nullable<int> Appl_ID
		{
			get
			{
				return this._Appl_ID;
			}
			set
			{
				if ((this._Appl_ID != value))
				{
					this._Appl_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[PI Name]", Storage="_PI_Name", DbType="NVarChar(255)")]
		public string PI_Name
		{
			get
			{
				return this._PI_Name;
			}
			set
			{
				if ((this._PI_Name != value))
				{
					this._PI_Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Title", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				if ((this._Title != value))
				{
					this._Title = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
