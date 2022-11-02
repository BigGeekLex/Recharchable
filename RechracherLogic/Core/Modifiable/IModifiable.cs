using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace MSFD
{
    public interface IModifiable<T>
    {
        /// <summary>
        /// Mods with higher priority will be called first
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        IDisposable AddMod(Func<T, T> mod, int priority = 0);
        /// <summary>
        /// This event raised on the next frame after changes happened, to prevent DDos when several modifiers were installed
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> GetObsOnModsUpdated();
        void RemoveAllMods();
        void RaiseModsUpdatedEvent();
    }
}