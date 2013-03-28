using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBooking.Data.Entities;

namespace EventBooking.Data.Repositories
{
    public interface IUserActivityItemRepository
    {
        void CreateOrUpdate(UserActivityItem item);
        void Delete(int itemId);
    }
}
