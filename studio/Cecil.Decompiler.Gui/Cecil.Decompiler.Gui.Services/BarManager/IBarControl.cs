﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cecil.Decompiler.Gui.Services
{
    public interface IBarControl
    {
       event EventHandler Click;
       void PerformClick();
    }
}
