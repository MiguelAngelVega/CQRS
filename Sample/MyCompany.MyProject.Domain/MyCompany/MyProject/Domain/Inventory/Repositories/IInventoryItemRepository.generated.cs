﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#region Copyright
// -----------------------------------------------------------------------
// <copyright company="cdmdotnet Limited">
//     Copyright cdmdotnet Limited. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#endregion
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using Cqrs.Repositories;
using Cqrs.Repositories.Queries;
using MyCompany.MyProject.Domain.Factories;
using MyCompany.MyProject.Domain.Inventory.Repositories.Queries.Strategies;

namespace MyCompany.MyProject.Domain.Inventory.Repositories
{
	[GeneratedCode("CQRS UML Code Generator", "1.500.508.396")]
	public partial interface IInventoryItemRepository : IRepository<InventoryItemQueryStrategy, Entities.InventoryItemEntity>
	{
	}
}

namespace MyCompany.MyProject.Domain.Inventory.Repositories.Queries.Strategies
{
	public partial class InventoryItemQueryStrategy : QueryStrategy
	{
	}
}
