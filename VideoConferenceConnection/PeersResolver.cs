﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.PeerToPeer;
using System.ServiceModel;
using NLog;
using VideoConferenceCommon;
using VideoConferenceConnection.Interfaces;
using VideoConferenceResources;

namespace VideoConferenceConnection
{
    /// <summary>
    /// Класс работы со сиском пиров, синглтон
    /// </summary>
    public class PeersResolver : IPeersResolver
    {
        #region Логгирование
        private static Logger log = LogManager.GetCurrentClassLogger();
        #endregion

        private PeerNameResolver _resolver;
        private List<Peer> _peerList;
        private static IPeersResolver _peersResolver;
        private List<Constants.VoidCallback> _callbacks; 

        private PeersResolver()
        {
            _resolver = new PeerNameResolver();
            _resolver.ResolveProgressChanged += resolver_ResolveProgressChanged;
            _resolver.ResolveCompleted += resolver_ResolveCompleted;

            _peerList = new List<Peer>();
            _callbacks = new List<Constants.VoidCallback>();
        }

        public static IPeersResolver Instance
        {
            get { return _peersResolver ?? (_peersResolver = new PeersResolver()); }
        }

        /// <summary>
        /// Список пиров
        /// </summary>
        public List<Peer> Peers
        {
            get { return _peerList; }
        }

        /// <summary>
        /// Обновить список пиров
        /// </summary>
        public void ReloadPeers()
        {
            ReloadPeers(null);
        }

        /// <summary>
        /// Обновление списка пиров, с методом обратного вызова
        /// </summary>
        /// <param name="callback"></param>
        public void ReloadPeers(Constants.VoidCallback callback)
        {
            if (callback != null)
                _callbacks.Add(callback);
            _peerList.Clear();
            _resolver.ResolveAsync(new PeerName("0." + ConnectionConstants.MainPeerName), 1);
        }

        /// <summary>
        /// Обрабработка найденного объекта
        /// </summary>
        private void resolver_ResolveProgressChanged(object sender, ResolveProgressChangedEventArgs e)
        {
            var peer = e.PeerNameRecord;

            foreach (IPEndPoint ep in peer.EndPointCollection)
            {
                if (ep.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) 
                    continue;

                try
                {
                    var endpointUrl = string.Format("net.tcp://{0}:{1}/ContentReceiver", ep.Address, ep.Port);
                    var binding = new NetTcpBinding {Security = {Mode = SecurityMode.None}};
                    
                    var contentReceiver = ChannelFactory<IContentReceiver>.CreateChannel(
                        binding, new EndpointAddress(endpointUrl));

                    _peerList.Add(new Peer
                    {
                        PeerName = peer.PeerName, 
                        ContentReceiver = contentReceiver,
                        ReceiverName = contentReceiver.GetName()
                    });
                }
                catch (EndpointNotFoundException ex)
                {
                    log.Warn(ex, "Ошибка во время обновления пиров. Неизвестный пир");
                }
            }
        }
        
        /// <summary>
        /// Обработка завершения разрешения
        /// </summary>
        void resolver_ResolveCompleted(object sender, ResolveCompletedEventArgs e)
        {
            InvokeCallbacks(e.Error);
        }

        /// <summary>
        /// Вызов методов обратного вызова
        /// </summary>
        /// <param name="e"></param>
        private void InvokeCallbacks(Exception e)
        {
            foreach (var voidCallback in _callbacks)
                voidCallback.Invoke(e);

            _callbacks.Clear();
        }
    }
}
