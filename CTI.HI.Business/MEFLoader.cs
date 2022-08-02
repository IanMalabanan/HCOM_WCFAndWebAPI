
 
using CTI.HI.Business.BusinessEngines;
using CTI.HI.Data.Contracts.Frebas;
using System.ComponentModel.Composition.Hosting;

namespace CTI.HI.Business
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            //take note of this :: it must be data projects/repositories
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IProjectRepository).Assembly));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(UserEngine).Assembly));
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(IUserProjectRepository).Assembly));
            //catalog.Catalogs.Add(new AssemblyCatalog(typeof(ConstructionMilestoneEngine).Assembly));
            CompositionContainer container = new CompositionContainer(catalog, true);

            return container;
        } 
    }
}
