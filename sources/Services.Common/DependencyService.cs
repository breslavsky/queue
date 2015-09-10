using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Common
{
    public abstract class DependencyService
    {
        public DependencyService()
        {
            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this.GetType(), this);
        }
    }
}