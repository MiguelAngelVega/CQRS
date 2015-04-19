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
using Cqrs.Domain;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cqrs.Commands;

namespace MyCompany.MyProject.Domain.Authentication.Commands
{
	/// <summary>
	/// A <see cref="ICommand{TAuthenticationToken}"/> that logically deletes an instance of a <see cref="User"/> aggregate
	/// </summary>
	[GeneratedCode("CQRS UML Code Generator", "1.500.508.396")]
	public  partial class DeleteUserCommand : ICommand<Cqrs.Authentication.ISingleSignOnToken>
	{
		#region Implementation of ICommand

		[DataMember]
		public int ExpectedVersion { get; set; }

		#endregion

		#region Implementation of IMessageWithAuthenticationToken<Cqrs.Authentication.ISingleSignOnToken>

		[DataMember]
		public Cqrs.Authentication.ISingleSignOnToken AuthenticationToken { get; set; }

		#endregion

		#region Implementation of IMessage

		[DataMember]
		public Guid CorrolationId { get; set; }

		#endregion

		[DataMember]
		public Guid Rsn { get; set; }

		public DeleteUserCommand()
		{
		}

		public DeleteUserCommand(Guid rsn)
		{
			Rsn = rsn;
		}
	}
}
