using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppPARI_elementary_;
internal class Stage
{
    public string Name { get; set; }
    public string Performer { get; set; }
    public Approval? Approval { get; set; }
    public string? Comment { get; set; }
}

internal enum Approval
{
    Approval,
    NotApproval,
    Skipped
}