﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Services.Interfaces
{
    public interface INeopixelService
    {
        Task SendNeopixelUpdate(SortedDictionary<int, string> colorDict);
    }
}
