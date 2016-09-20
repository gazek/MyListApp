using MyListApp.Api.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyListApp.Api.Data.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyListApp.Api.UnitTests.Fakes
{
    class AppDbContextFake : IdentityDbContext<IdentityUser>, IAppDbContext
    {
        public AppDbContextFake()
             : base("AppDbContextFake")
        {

        }

        public DbSet<InvitationModel> Invitations
        {
            get
            {
                //throw new NotImplementedException();
                return null;
            }

            set
            {
                //throw new NotImplementedException();
            }
        }

        public DbSet<ListModel> Lists
        {
            get
            {
                //throw new NotImplementedException();
                return null;
            }

            set
            {
                //throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public DbSet<T> Set<T>() where T : class
        {
            //throw new NotImplementedException();
            return null;
        }
    }
}
