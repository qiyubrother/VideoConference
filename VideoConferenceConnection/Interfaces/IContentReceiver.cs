﻿using System.ServiceModel;
using VideoConferenceCommon;
using VideoConferenceObjects.Interfaces;

namespace VideoConferenceConnection.Interfaces
{
    [ServiceContract]
    public interface IContentReceiver
    {
        [OperationContract]
        string GetName();

        [OperationContract(IsOneWay = true)]
        void SendMessage(IPackage package, string from);
    }
}
