﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1022
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ODPTaxonomyAccountDAL
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
	public partial class AccountDataLinqDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public AccountDataLinqDataContext() : 
				base(global::ODPTaxonomyAccountDAL.Properties.Settings.Default.ODP_Taxonomy_DEVConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AccountDataLinqDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountDataLinqDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountDataLinqDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountDataLinqDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.select_users")]
		public ISingleResult<select_usersResult> select_users([global::System.Data.Linq.Mapping.ParameterAttribute(Name="SortBy", DbType="VarChar(20)")] string sortBy, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="SortDirection", DbType="VarChar(4)")] string sortDirection)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), sortBy, sortDirection);
			return ((ISingleResult<select_usersResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.update_userProfileByID")]
		public int update_userProfileByID([global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserId", DbType="UniqueIdentifier")] System.Nullable<System.Guid> userId, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="FirstName", DbType="VarChar(50)")] string firstName, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="LastName", DbType="VarChar(50)")] string lastName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userId, firstName, lastName);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.select_userByID")]
		public ISingleResult<select_userByIDResult> select_userByID([global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserID", DbType="UniqueIdentifier")] System.Nullable<System.Guid> userID)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID);
			return ((ISingleResult<select_userByIDResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.select_userByUserName")]
		public ISingleResult<select_userByUserNameResult> select_userByUserName([global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserName", DbType="VarChar(200)")] string userName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userName);
			return ((ISingleResult<select_userByUserNameResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.select_activeTeamByUserName")]
		public ISingleResult<select_activeTeamByUserNameResult> select_activeTeamByUserName([global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserName", DbType="VarChar(200)")] string userName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userName);
			return ((ISingleResult<select_activeTeamByUserNameResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.create_user")]
		public ISingleResult<create_userResult> create_user([global::System.Data.Linq.Mapping.ParameterAttribute(Name="FirstName", DbType="VarChar(50)")] string firstName, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="LastName", DbType="VarChar(50)")] string lastName, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="Email", DbType="NVarChar(256)")] string email, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="ClearTextPassword", DbType="NVarChar(128)")] string clearTextPassword, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="IsApproved", DbType="Bit")] System.Nullable<bool> isApproved, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(500)")] string roleList, [global::System.Data.Linq.Mapping.ParameterAttribute(Name="UserNameOutput", DbType="NVarChar(256)")] ref string userNameOutput)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), firstName, lastName, email, clearTextPassword, isApproved, roleList, userNameOutput);
			userNameOutput = ((string)(result.GetParameterValue(6)));
			return ((ISingleResult<create_userResult>)(result.ReturnValue));
		}
	}
	
	public partial class select_usersResult
	{
		
		private System.Guid _UserID;
		
		private string _UserName;
		
		private string _UserFirstName;
		
		private string _UserLastName;
		
		private string _Email;
		
		private System.Nullable<bool> _IsApproved;
		
		private System.Nullable<bool> _IsLockedOut;
		
		public select_usersResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this._UserID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserFirstName", DbType="NVarChar(50)")]
		public string UserFirstName
		{
			get
			{
				return this._UserFirstName;
			}
			set
			{
				if ((this._UserFirstName != value))
				{
					this._UserFirstName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserLastName", DbType="NVarChar(50)")]
		public string UserLastName
		{
			get
			{
				return this._UserLastName;
			}
			set
			{
				if ((this._UserLastName != value))
				{
					this._UserLastName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(256)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this._Email = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsApproved", DbType="Bit")]
		public System.Nullable<bool> IsApproved
		{
			get
			{
				return this._IsApproved;
			}
			set
			{
				if ((this._IsApproved != value))
				{
					this._IsApproved = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsLockedOut", DbType="Bit")]
		public System.Nullable<bool> IsLockedOut
		{
			get
			{
				return this._IsLockedOut;
			}
			set
			{
				if ((this._IsLockedOut != value))
				{
					this._IsLockedOut = value;
				}
			}
		}
	}
	
	public partial class select_userByIDResult
	{
		
		private System.Guid _userid;
		
		private string _UserName;
		
		private string _UserFirstName;
		
		private string _UserLastName;
		
		private string _Email;
		
		private System.Nullable<bool> _IsApproved;
		
		public select_userByIDResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_userid", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid userid
		{
			get
			{
				return this._userid;
			}
			set
			{
				if ((this._userid != value))
				{
					this._userid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserFirstName", DbType="NVarChar(50)")]
		public string UserFirstName
		{
			get
			{
				return this._UserFirstName;
			}
			set
			{
				if ((this._UserFirstName != value))
				{
					this._UserFirstName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserLastName", DbType="NVarChar(50)")]
		public string UserLastName
		{
			get
			{
				return this._UserLastName;
			}
			set
			{
				if ((this._UserLastName != value))
				{
					this._UserLastName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(256)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this._Email = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsApproved", DbType="Bit")]
		public System.Nullable<bool> IsApproved
		{
			get
			{
				return this._IsApproved;
			}
			set
			{
				if ((this._IsApproved != value))
				{
					this._IsApproved = value;
				}
			}
		}
	}
	
	public partial class select_userByUserNameResult
	{
		
		private System.Guid _userid;
		
		private string _UserName;
		
		private string _UserFirstName;
		
		private string _UserLastName;
		
		private string _Email;
		
		private System.Nullable<bool> _IsApproved;
		
		public select_userByUserNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_userid", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid userid
		{
			get
			{
				return this._userid;
			}
			set
			{
				if ((this._userid != value))
				{
					this._userid = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserFirstName", DbType="NVarChar(50)")]
		public string UserFirstName
		{
			get
			{
				return this._UserFirstName;
			}
			set
			{
				if ((this._UserFirstName != value))
				{
					this._UserFirstName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserLastName", DbType="NVarChar(50)")]
		public string UserLastName
		{
			get
			{
				return this._UserLastName;
			}
			set
			{
				if ((this._UserLastName != value))
				{
					this._UserLastName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Email", DbType="NVarChar(256)")]
		public string Email
		{
			get
			{
				return this._Email;
			}
			set
			{
				if ((this._Email != value))
				{
					this._Email = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsApproved", DbType="Bit")]
		public System.Nullable<bool> IsApproved
		{
			get
			{
				return this._IsApproved;
			}
			set
			{
				if ((this._IsApproved != value))
				{
					this._IsApproved = value;
				}
			}
		}
	}
	
	public partial class select_activeTeamByUserNameResult
	{
		
		private int _TeamID;
		
		private string _TeamCode;
		
		private System.Nullable<int> _TeamTypeID;
		
		private int _StatusID;
		
		private System.Nullable<System.DateTime> _CreatedDateTime;
		
		private System.Nullable<System.Guid> _Createdby;
		
		private System.Nullable<System.Guid> _UpdatedBy;
		
		private System.Nullable<System.DateTime> _UpdatedDateTime;
		
		private int _TeamID1;
		
		private System.Guid _UserId;
		
		private System.Guid _ApplicationId;
		
		private System.Guid _UserId1;
		
		private string _UserName;
		
		private string _LoweredUserName;
		
		private string _MobileAlias;
		
		private bool _IsAnonymous;
		
		private System.DateTime _LastActivityDate;
		
		private string _UserFirstName;
		
		private string _UserLastName;
		
		public select_activeTeamByUserNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TeamID", DbType="Int NOT NULL")]
		public int TeamID
		{
			get
			{
				return this._TeamID;
			}
			set
			{
				if ((this._TeamID != value))
				{
					this._TeamID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TeamCode", DbType="NVarChar(50)")]
		public string TeamCode
		{
			get
			{
				return this._TeamCode;
			}
			set
			{
				if ((this._TeamCode != value))
				{
					this._TeamCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TeamTypeID", DbType="Int")]
		public System.Nullable<int> TeamTypeID
		{
			get
			{
				return this._TeamTypeID;
			}
			set
			{
				if ((this._TeamTypeID != value))
				{
					this._TeamTypeID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StatusID", DbType="Int NOT NULL")]
		public int StatusID
		{
			get
			{
				return this._StatusID;
			}
			set
			{
				if ((this._StatusID != value))
				{
					this._StatusID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreatedDateTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> CreatedDateTime
		{
			get
			{
				return this._CreatedDateTime;
			}
			set
			{
				if ((this._CreatedDateTime != value))
				{
					this._CreatedDateTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Createdby", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> Createdby
		{
			get
			{
				return this._Createdby;
			}
			set
			{
				if ((this._Createdby != value))
				{
					this._Createdby = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedBy", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> UpdatedBy
		{
			get
			{
				return this._UpdatedBy;
			}
			set
			{
				if ((this._UpdatedBy != value))
				{
					this._UpdatedBy = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdatedDateTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> UpdatedDateTime
		{
			get
			{
				return this._UpdatedDateTime;
			}
			set
			{
				if ((this._UpdatedDateTime != value))
				{
					this._UpdatedDateTime = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TeamID1", DbType="Int NOT NULL")]
		public int TeamID1
		{
			get
			{
				return this._TeamID1;
			}
			set
			{
				if ((this._TeamID1 != value))
				{
					this._TeamID1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this._UserId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ApplicationId", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ApplicationId
		{
			get
			{
				return this._ApplicationId;
			}
			set
			{
				if ((this._ApplicationId != value))
				{
					this._ApplicationId = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId1", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid UserId1
		{
			get
			{
				return this._UserId1;
			}
			set
			{
				if ((this._UserId1 != value))
				{
					this._UserId1 = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this._UserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LoweredUserName", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string LoweredUserName
		{
			get
			{
				return this._LoweredUserName;
			}
			set
			{
				if ((this._LoweredUserName != value))
				{
					this._LoweredUserName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MobileAlias", DbType="NVarChar(16)")]
		public string MobileAlias
		{
			get
			{
				return this._MobileAlias;
			}
			set
			{
				if ((this._MobileAlias != value))
				{
					this._MobileAlias = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsAnonymous", DbType="Bit NOT NULL")]
		public bool IsAnonymous
		{
			get
			{
				return this._IsAnonymous;
			}
			set
			{
				if ((this._IsAnonymous != value))
				{
					this._IsAnonymous = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastActivityDate", DbType="DateTime NOT NULL")]
		public System.DateTime LastActivityDate
		{
			get
			{
				return this._LastActivityDate;
			}
			set
			{
				if ((this._LastActivityDate != value))
				{
					this._LastActivityDate = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserFirstName", DbType="NVarChar(50)")]
		public string UserFirstName
		{
			get
			{
				return this._UserFirstName;
			}
			set
			{
				if ((this._UserFirstName != value))
				{
					this._UserFirstName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserLastName", DbType="NVarChar(50)")]
		public string UserLastName
		{
			get
			{
				return this._UserLastName;
			}
			set
			{
				if ((this._UserLastName != value))
				{
					this._UserLastName = value;
				}
			}
		}
	}
	
	public partial class create_userResult
	{
		
		private string _Name;
		
		public create_userResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
