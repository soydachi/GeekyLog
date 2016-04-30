using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SampleUwp.ViewModels;

namespace SampleUwp
{
    public class ViewModelLocator
    {
        readonly IContainer container;
        
        public ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainViewModel>();

            container = builder.Build();
        }

        public MainViewModel MainViewModel => container.Resolve<MainViewModel>();
    }
}
