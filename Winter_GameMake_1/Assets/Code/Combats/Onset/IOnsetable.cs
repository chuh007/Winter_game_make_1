using UnityEngine;

namespace Code.Combats.Onset
{
    public interface IOnsetable
    {
        /// <summary>
        /// �÷��̾ �����ߴٸ� true, �ƴϸ� false
        /// </summary>
        bool IsFindPlayer { get; }
    }
}

