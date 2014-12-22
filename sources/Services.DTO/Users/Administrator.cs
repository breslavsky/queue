using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    public class Administrator : User
    {
        [DataMember]
        public virtual AdministratorPermissions Permissions { get; set; }
    }
}