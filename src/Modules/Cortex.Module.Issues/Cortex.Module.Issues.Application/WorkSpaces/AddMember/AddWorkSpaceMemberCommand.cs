using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Application.WorkSpaces.AddMember
{
    public class AddWorkspaceMemberCommand : IRequest<AddWorkspaceMemberResult>
    {
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }  
        public required string TargetUserId { get; set; }       
        public required string TargetFullName { get; set; }
        public required string RoleName { get; set; }            
    }

    public class AddWorkspaceMemberResult
    {
        public bool Succeeded { get; set; }
        public Guid? WorkSpaceMemberId { get; set; }
        public string? Error { get; set; }
    }
}
