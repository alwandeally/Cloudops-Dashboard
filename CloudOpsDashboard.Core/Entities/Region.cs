using System;
using System.Collections.Generic;
using System.Text;

namespace CloudOpsDashboard.Core.Entities;

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}