<<<<<<< HEAD:sources/Services.Server/Server/Template/TemplateHttpService.cs
﻿using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
=======
﻿using Queue.Services.Contracts;
>>>>>>> origin/master:sources/Services.Server/Server/Template/ServerTemplateHttpService.cs
using System.IO;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class TemplateHttpService : TemplateService, ITemplateHttpService
    {
        public Stream GetTemplate(string app, string theme, string template)
        {
            return base.ReadTemplate(app, theme, template);
        }
    }
}