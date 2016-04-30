using Autofac;
using GeekyLogViewer.ViewModels;

namespace GeekyLogViewer
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
