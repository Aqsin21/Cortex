using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Application.WorkSpaces.GetWorkSpaces
{
    public class GetWorkSpaceQuery: IRequest<List<WorkSpaceDto>>
    {
        public required string UserId { get; set; }

    }
    public class WorkSpaceDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string RoleName { get; set; }  
    }
}
