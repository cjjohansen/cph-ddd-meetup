using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cph.DDD.Meetup.Logistics.Domain.Common
{
    public interface IVersioned
    {
        ulong Version { get; set; }
    }
}
