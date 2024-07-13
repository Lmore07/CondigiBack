using Microsoft.EntityFrameworkCore;

namespace CondigiBack.Contexts
{
    public class AppDBContext: DbContext
    {
        
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options)
        {
        }
       
    }
}
