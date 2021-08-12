using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 8618
namespace DataMonitoring.Storage.MongoDb
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

    }
}
