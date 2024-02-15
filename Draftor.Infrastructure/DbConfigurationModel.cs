using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draftor.Infrastructure;
public record DbConfigurationModel
{
    public required string DatabasePath { get; init; }
}
