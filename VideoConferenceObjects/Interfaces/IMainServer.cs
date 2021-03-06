﻿using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace VideoConferenceObjects.Interfaces
{
    public interface IMainServer : IDisposable
    {
        /// <summary>
        /// Запущен ли сервер
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Порт прослушивания
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Адрес сервера
        /// </summary>
        IPAddress Address { get; }

        /// <summary>
        /// Сертификат
        /// </summary>
        X509Certificate2 Certificate { get; }

        /// <summary>
        /// Запустить сервер
        /// </summary>
        void RunServer();

        /// <summary>
        /// Остановить сервер
        /// </summary>
        void StopServer();
    }
}
