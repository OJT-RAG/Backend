using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;

namespace OJT_RAG.Repositories.Enums
{
    public enum DocumentTagType
    {
        [PgName("company")]
        Company,

        [PgName("university")]
        University,

        [PgName("system")]
        System
    }
}
