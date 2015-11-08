<<<<<<< HEAD:sources/Services.Server/Server/User/UserHttpService.cs
﻿using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
using System.Linq;
=======
﻿using Queue.Services.Contracts;
>>>>>>> origin/master:sources/Services.Server/Server/Workplace/ServerWorkplaceHttpService.cs
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class UserHttpService : UserService, IUserHttpService
    {
    }
}