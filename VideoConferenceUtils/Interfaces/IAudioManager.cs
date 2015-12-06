﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoConferenceConnection.Interfaces;
using VideoConferenceObjects.Interfaces;

namespace VideoConferenceUtils.Interfaces
{
    public interface IAudioManager : IContentManager, IAudioWorker
    {

    }
}
