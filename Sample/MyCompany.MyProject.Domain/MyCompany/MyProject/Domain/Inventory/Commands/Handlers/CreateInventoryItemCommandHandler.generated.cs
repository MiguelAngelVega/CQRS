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
using Cqrs.Commands;
using Cqrs.Configuration;
using Cqrs.Domain;
using Cqrs.Logging;

namespace MyCompany.MyProject.Domain.Inventory.Commands.Handlers
{
	[GeneratedCode("CQRS UML Code Generator", "1.500.508.396")]
	public  partial class CreateInventoryItemCommandHandler : ICommandHandler<Cqrs.Authentication.ISingleSignOnToken, CreateInventoryItemCommand>
	{
		protected IUnitOfWork<Cqrs.Authentication.ISingleSignOnToken> UnitOfWork { get; private set; }

		protected IDependencyResolver DependencyResolver { get; private set; }

		protected ILog Log { get; private set; }

		public CreateInventoryItemCommandHandler(IUnitOfWork<Cqrs.Authentication.ISingleSignOnToken> unitOfWork, IDependencyResolver dependencyResolver, ILog log)
		{
			UnitOfWork = unitOfWork;
			DependencyResolver = dependencyResolver;
			Log = log;
		}

		public string Name { get; set; }


		#region Implementation of ICommandHandler<in CreateInventoryItem>

		public void Handle(CreateInventoryItemCommand command)
		{
			InventoryItem item = null;
			OnCreateInventoryItem(command, ref item);
			if (item == null)
				item = new InventoryItem(DependencyResolver, Log, command.Rsn == Guid.Empty ? Guid.NewGuid() : command.Rsn);
			item.CreateInventoryItem(command.Name);
			OnCreateInventoryItemDone(command, item);
			OnCommit(command, item);
			UnitOfWork.Commit();
			OnCommited(command, item);
		}

		#endregion
		partial void OnCreateInventoryItem(CreateInventoryItemCommand command, ref InventoryItem item);

		partial void OnCreateInventoryItemDone(CreateInventoryItemCommand command, InventoryItem item);

		partial void OnCommit(CreateInventoryItemCommand command, InventoryItem item);

		partial void OnCommited(CreateInventoryItemCommand command, InventoryItem item);

	}
}
